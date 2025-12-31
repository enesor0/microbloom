using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using microbloom.Data;
using microbloom.Entities;

namespace microbloom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly KariyerDBContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            KariyerDBContext context,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

    [HttpPost("register")]
    [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var isCompanyAccount = string.Equals(registerDto.AccountType, "Employer", StringComparison.OrdinalIgnoreCase);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (isCompanyAccount && string.IsNullOrWhiteSpace(registerDto.CompanyName))
                {
                    ModelState.AddModelError("CompanyName", "Şirket kaydı için şirket adı gereklidir.");
                    return BadRequest(ModelState);
                }

                var user = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Title = registerDto.AccountType == "Mentor" && !string.IsNullOrEmpty(registerDto.Workplace) 
                        ? $"{registerDto.Title} @ {registerDto.Workplace}" 
                        : registerDto.Title,
                    Skills = registerDto.Skills,
                    Bio = registerDto.Bio,
                    LinkedInUrl = registerDto.LinkedInUrl
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                string targetRole;
                if (isCompanyAccount) targetRole = "Employer";
                else if (registerDto.AccountType == "Mentor") targetRole = "Mentor";
                else targetRole = "JobSeeker";

                if (!await _roleManager.RoleExistsAsync(targetRole))
                {
                    await _roleManager.CreateAsync(new IdentityRole(targetRole));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, targetRole);
                if (!roleResult.Succeeded)
                {
                    // Clean up user if role assignment fails
                    await _userManager.DeleteAsync(user);
                     foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                if (isCompanyAccount)
                {
                    var company = new Company
                    {
                        Name = registerDto.CompanyName!.Trim(),
                        Description = string.IsNullOrWhiteSpace(registerDto.CompanyDescription)
                            ? null
                            : registerDto.CompanyDescription.Trim(),
                        LogoUrl = string.IsNullOrWhiteSpace(registerDto.CompanyLogoUrl)
                            ? null
                            : registerDto.CompanyLogoUrl.Trim()
                    };

                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();

                    user.CompanyId = company.Id;
                    await _userManager.UpdateAsync(user);
                }

                // Kayıt sonrası otomatik giriş yap
                await _signInManager.SignInAsync(user, isPersistent: false);
        
                string finalReturnUrl;
                if (isCompanyAccount) finalReturnUrl = "/company-dashboard";
                else if (registerDto.AccountType == "Mentor") finalReturnUrl = "/mentor-dashboard";
                else finalReturnUrl = "/";
        
                _logger.LogInformation("New user registered and logged in: {Email}", user.Email);
                return Ok(new { success = true, returnUrl = finalReturnUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, new { message = "Kayıt sırasında bir hata oluştu: " + ex.Message });
            }
        }

    [HttpPost("login")]
    [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, [FromQuery] string? returnUrl = null)
        {
            var targetUrl = NormalizeReturnUrl(returnUrl);

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            // Find user by email to get their UserName (required for PasswordSignInAsync)
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                // User not found
                _logger.LogWarning("Invalid credentials for {Email} via API login.", loginDto.Email);
                return Unauthorized(new { message = "E-posta veya şifre hatalı." });
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                loginDto.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (string.Equals(targetUrl, "/", StringComparison.OrdinalIgnoreCase))
                {
                    if (await _userManager.IsInRoleAsync(user, "Employer"))
                    {
                        targetUrl = "/company-dashboard";
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Mentor"))
                    {
                        targetUrl = "/mentor-dashboard";
                    }
                }

                _logger.LogInformation("User {Email} signed in via API.", loginDto.Email);
                return Ok(new { returnUrl = targetUrl });
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("Locked out user {Email} attempted to sign in via API.", loginDto.Email);
                return Unauthorized(new { message = "Hesabınız geçici olarak kilitli. Lütfen daha sonra tekrar deneyin." });
            }

            _logger.LogWarning("Invalid credentials for {Email} via API login.", loginDto.Email);
            return Unauthorized(new { message = "E-posta veya şifre hatalı." });
        }

    [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(new { Message = "Çıkış başarılı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Çıkış işlemi sırasında hata oluştu.");
                return BadRequest(new { Message = $"Hata: {ex.Message}" });
            }
        }

        // POST: /api/account/promote-to-employer
        [HttpPost("promote-to-employer")]
        [Authorize] // Admin tarafından çağrılabilir
        public async Task<IActionResult> PromoteToEmployer([FromBody] PromoteUserDto promoteDto)
        {
            try
            {
                if (string.IsNullOrEmpty(promoteDto.UserId) || promoteDto.CompanyId <= 0)
                {
                    return BadRequest("UserId ve CompanyId gereklidir.");
                }

                var user = await _userManager.FindByIdAsync(promoteDto.UserId);
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }

                // Kullanıcıyı Employer rolüne ekle
                var result = await _userManager.AddToRoleAsync(user, "Employer");
                if (!result.Succeeded)
                {
                    return BadRequest(new { Message = "Rol atanırken hata oluştu.", Errors = result.Errors });
                }

                // CompanyId'yi ata
                user.CompanyId = promoteDto.CompanyId;
                await _userManager.UpdateAsync(user);

                return Ok(new { Message = $"Kullanıcı başarıyla Employer olarak yükseltildi.", UserId = user.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcıyı işveren olarak terfi ettirme sırasında hata oluştu.");
                return BadRequest(new { Message = $"Hata: {ex.Message}" });
            }
        }

        // ✅ YENİ: Test için basit login endpoint
        [HttpGet("test-cookie")]
        public IActionResult TestCookie()
        {
            var isAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            var userName = User?.Identity?.Name ?? "NULL";
            var authType = User?.Identity?.AuthenticationType ?? "NULL";

            return Ok(new
            {
                Message = "Cookie kontrol endpoint'i",
                IsAuthenticated = isAuthenticated,
                UserName = userName,
                AuthenticationType = authType,
                HasIdentityCookie = Request.Cookies.ContainsKey(".AspNetCore.Identity.Application")
            });
        }

        private string NormalizeReturnUrl(string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return Url.Content("~/");
            }

            return Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Content("~/");
        }

        // Email ile kullanıcıyı Employer yap ve şirket oluştur (geliştirme amaçlı)
        [HttpPost("make-employer")]
        [AllowAnonymous]
        public async Task<IActionResult> MakeEmployer([FromBody] MakeEmployerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new { Message = "Email gereklidir." });
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return NotFound(new { Message = "Kullanıcı bulunamadı." });
                }

                // Mevcut rolleri kontrol et
                var currentRoles = await _userManager.GetRolesAsync(user);

                // JobSeeker rolünü kaldır
                if (currentRoles.Contains("JobSeeker"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "JobSeeker");
                }

                // Employer rolü yoksa ekle
                if (!currentRoles.Contains("Employer"))
                {
                    var result = await _userManager.AddToRoleAsync(user, "Employer");
                    if (!result.Succeeded)
                    {
                        return BadRequest(new { Message = "Rol atanamadı: " + string.Join(", ", result.Errors.Select(e => e.Description)) });
                    }
                }

                // Şirket yoksa oluştur ve bağla
                if (user.CompanyId == null)
                {
                    var company = new Company
                    {
                        Name = dto.CompanyName ?? user.FirstName + " " + user.LastName + " Şirketi",
                        Description = "Şirket açıklaması henüz eklenmedi."
                    };
                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();

                    user.CompanyId = company.Id;
                    await _userManager.UpdateAsync(user);
                }

                return Ok(new { Message = $"{dto.Email} hesabına Employer rolü ve şirket atandı. Yeniden giriş yapın.", CompanyId = user.CompanyId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Hata: {ex.Message}" });
            }
        }

        // Şifre sıfırlama - email ile
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            if (string.IsNullOrWhiteSpace(resetDto.Email) || string.IsNullOrWhiteSpace(resetDto.NewPassword))
            {
                return BadRequest(new { Message = "Email ve yeni şifre gereklidir." });
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(resetDto.Email);
                if (user == null)
                {
                    return NotFound(new { Message = "Bu email adresiyle kayıtlı kullanıcı bulunamadı." });
                }

                // Şifreyi sıfırla
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, resetDto.NewPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(new { Message = $"Şifre sıfırlama başarısız: {errors}" });
                }

                return Ok(new { Message = "Şifre başarıyla sıfırlandı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre sıfırlama sırasında hata oluştu.");
                return BadRequest(new { Message = $"Hata: {ex.Message}" });
            }
        }
    [HttpGet("mentors")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMentors()
    {
        var mentors = await _userManager.GetUsersInRoleAsync("Mentor");
        
        var mentorDtos = mentors.Select(u => new MentorDto
        {
            Id = u.Id,
            UserName = u.UserName ?? "",
            FirstName = u.FirstName ?? "",
            LastName = u.LastName ?? "",
            Title = u.Title,
            ProfilePictureUrl = u.ProfilePictureUrl,
            Skills = u.Skills,
            Bio = u.Bio
        }).ToList();

        return Ok(mentorDtos);
    }

    public class MakeEmployerDto
    {
        public string Email { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
}
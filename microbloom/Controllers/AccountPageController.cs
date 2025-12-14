using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using microbloom.Data;
using microbloom.Entities;
using System;
using System.Linq;

namespace microbloom.Controllers
{
    [Route("account")]
    public class AccountPageController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly KariyerDBContext _context;
        private readonly ILogger<AccountPageController> _logger;

        public AccountPageController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            KariyerDBContext context,
            ILogger<AccountPageController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Login POST - Form'dan gelen verileri i�ler
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password, string? returnUrl)
        {
            try
            {
        returnUrl = NormalizeReturnUrl(returnUrl);
                _logger.LogInformation($"Login attempt for: {email}");

                var result = await _signInManager.PasswordSignInAsync(
                    email,
                    password,
                    isPersistent: false,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Login successful: {email}");
                    return Redirect(returnUrl);
                }

                _logger.LogWarning($"Login failed for: {email}");
                return Redirect($"/account/login?ReturnUrl={Uri.EscapeDataString(returnUrl)}&ErrorMessage={Uri.EscapeDataString("E-posta veya şifre hatalı.")}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Login error for: {email}");
                return Redirect($"/account/login?ReturnUrl={Uri.EscapeDataString(NormalizeReturnUrl(returnUrl))}&ErrorMessage={Uri.EscapeDataString("Giriş sırasında bir hata oluştu.")}");
            }
        }

        /// <summary>
        /// Register POST - Yeni kullan�c� kayd�
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            string email,
            string firstName,
            string lastName,
            string password,
            string confirmPassword,
            string accountType,
            string? companyName,
            string? companyDescription,
            string? companyLogoUrl)
        {
            try
            {
                _logger.LogInformation($"Register attempt for: {email}");

                if (!string.Equals(password, confirmPassword, StringComparison.Ordinal))
                {
                    return Redirect($"/account/register?AccountType={Uri.EscapeDataString(accountType ?? "JobSeeker")}&ErrorMessage={Uri.EscapeDataString("Şifreler eşleşmiyor.")}");
                }

                var isCompanyAccount = string.Equals(accountType, "Employer", StringComparison.OrdinalIgnoreCase);

                if (isCompanyAccount && string.IsNullOrWhiteSpace(companyName))
                {
                    return Redirect($"/account/register?AccountType=Employer&ErrorMessage={Uri.EscapeDataString("Şirket kaydı için şirket adı gereklidir.")}");
                }

                var user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                };

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Register failed for: {email}");
                    var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                    return Redirect($"/account/register?AccountType={Uri.EscapeDataString(accountType ?? "JobSeeker")}&ErrorMessage={Uri.EscapeDataString(errors)}");
                }

                var targetRole = isCompanyAccount ? "Employer" : "JobSeeker";
                
                // Rol yoksa oluştur
                if (!await _roleManager.RoleExistsAsync(targetRole))
                {
                    await _roleManager.CreateAsync(new IdentityRole(targetRole));
                }
                
                var roleResult = await _userManager.AddToRoleAsync(user, targetRole);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(" ", roleResult.Errors.Select(e => e.Description));
                    return Redirect($"/account/register?AccountType={Uri.EscapeDataString(accountType ?? "JobSeeker")}&ErrorMessage={Uri.EscapeDataString("Rol atanırken hata: " + errors)}");
                }

                if (isCompanyAccount)
                {
                    var company = new Company
                    {
                        Name = companyName!.Trim(),
                        Description = string.IsNullOrWhiteSpace(companyDescription) ? null : companyDescription.Trim(),
                        LogoUrl = string.IsNullOrWhiteSpace(companyLogoUrl) ? null : companyLogoUrl.Trim()
                    };

                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();

                    user.CompanyId = company.Id;
                    await _userManager.UpdateAsync(user);
                }

                _logger.LogInformation($"Register successful: {email}");
                
                // Kayıt başarılı - login sayfasına yönlendir (cookie timing sorununu önler)
                return Redirect($"/account/login?SuccessMessage={Uri.EscapeDataString("Kayıt başarılı! Lütfen giriş yapın.")}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Register error for: {email}");
                return Redirect($"/account/register?AccountType={Uri.EscapeDataString(accountType ?? "JobSeeker")}&ErrorMessage={Uri.EscapeDataString("Kayıt sırasında bir hata oluştu.")}");
            }
        }

        /// <summary>
        /// Logout - ��k�� i�lemi
        /// </summary>
        [HttpGet("logout")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out");
                return Redirect("/");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout error");
                return Redirect("/");
            }
        }

        private string NormalizeReturnUrl(string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return "/";
            }

            return Url.IsLocalUrl(returnUrl) ? returnUrl : "/";
        }
    }
}

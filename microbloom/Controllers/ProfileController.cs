using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using microbloom.DTOs;
using microbloom.Entities;

namespace microbloom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var dto = new UserProfileDto
            {
                Email = user.Email,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName,
                Title = user.Title,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                CvUrl = user.CvUrl,
                LinkedInUrl = user.LinkedInUrl,
                GitHubUrl = user.GitHubUrl,
                WebsiteUrl = user.WebsiteUrl,
                Skills = user.Skills
            };

            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto dto)
        {
            // Log model state errors for debugging
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                Console.WriteLine($"Model validation errors: {string.Join(", ", errors)}");
                return BadRequest(new { Errors = errors });
            }
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Kullanıcı bulunamadı");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Title = dto.Title;
            user.Bio = dto.Bio;
            user.LinkedInUrl = dto.LinkedInUrl;
            user.GitHubUrl = dto.GitHubUrl;
            user.WebsiteUrl = dto.WebsiteUrl;
            user.Skills = dto.Skills;
            
            // Note: Picture & CV URLs are updated via Upload endpoints, 
            // but we allow clearing them if empty string is sent? 
            // For now let's assume separate upload flow handles URLs.
            
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Profil güncellendi." });
        }

        [HttpPost("upload-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            return await UploadFile(file, "uploads/profiles", (u, path) => u.ProfilePictureUrl = path);
        }

        [HttpPost("upload-cv")]
        public async Task<IActionResult> UploadCv(IFormFile file)
        {
            return await UploadFile(file, "uploads/cvs", (u, path) => u.CvUrl = path);
        }

        private async Task<IActionResult> UploadFile(IFormFile file, string folder, Action<AppUser, string> updateAction)
        {
            if (file == null || file.Length == 0) return BadRequest("Dosya seçilmedi.");

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var uploadsRoot = Path.Combine(_environment.WebRootPath, folder);
            if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

            var fileName = $"{user.Id}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsRoot, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var publicUrl = $"/{folder}/{fileName}";
            updateAction(user, publicUrl);

            await _userManager.UpdateAsync(user);

            return Ok(new { Url = publicUrl });
        }
    }
}

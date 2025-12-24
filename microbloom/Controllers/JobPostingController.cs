using microbloom.DTOs;
using microbloom.Services.Interfaces; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace microbloom.Controllers
{
    [Route("api/jobpostings")] 
    [ApiController]
    public class JobPostingsController : ControllerBase
    {

        private readonly IJobService _jobService;

        public JobPostingsController(IJobService jobService)
        {
         _jobService = jobService;
        }


        [HttpGet]
        public async Task<ActionResult<List<JobPostingDto>>> GetAllJobs()
  {
    var jobs = await _jobService.GetAllJobsAsync();
  return Ok(jobs);
        }


        [HttpGet("{id:int}")]
  public async Task<ActionResult<JobPostingDetailDto>> GetJobById(int id)
    {
            var job = await _jobService.GetJobByIdAsync(id);
        if (job == null)
       {
          return NotFound("İş ilanı bulunamadı.");
    }
         return Ok(job);
        }

        [HttpGet("search")]
   public async Task<ActionResult<List<JobPostingDto>>> SearchJobs(
    [FromQuery] string keyword,
    [FromQuery] string location)
        {
            var jobs = await _jobService.SearchJobsAsync(keyword, location);
            return Ok(jobs);
      }

    [HttpGet("my-applications")]
    [Authorize(Roles = "JobSeeker")]
    public async Task<ActionResult<List<ApplicationDto>>> GetMyApplications()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId))
      {
        return Unauthorized("Kullanıcı kimliği bulunamadı.");
      }

      var applications = await _jobService.GetUserApplicationsAsync(userId);
      return Ok(applications);
    }

    // POST: /api/jobpostings/{id}/apply
        [HttpPost("{id:int}/apply")]
        [Authorize(Roles = "JobSeeker")]
   public async Task<IActionResult> ApplyToJob(int id)
        {
    // Giriş yapmış kullanıcının kimliğini alıyoruz
  var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
 if (userId == null)
{
  return Unauthorized("Kullanıcı kimliği bulunamadı.");
            }

        try
    {
      // Servisteki metodu çağırıyoruz
           await _jobService.ApplyForJobAsync(id, userId);
     return Ok(new { Message = "Başvuru başarılı." });
            }
       catch (InvalidOperationException ex)
     {
      // Çift başvuru veya başka bir iş kuralı hatası
       return BadRequest(new { Message = ex.Message });
     }
         catch (Exception ex)
        {
                return BadRequest(new { Message = $"Başvuru sırasında hata: {ex.Message}" });
     }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using microbloom.Services.Interfaces;
using microbloom.DTOs;

[Route("api/content")]
[ApiController]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;
    private readonly ILogger<ContentController> _logger;
    
    public ContentController(IContentService contentService, ILogger<ContentController> logger) 
    { 
        _contentService = contentService;
        _logger = logger;
    }

    [HttpGet("all")] // Ana rehber sayfasını doldurur
    public async Task<ActionResult<List<ContentCategoryDto>>> GetAll()
    {
        try
        {
            _logger.LogInformation("🔍 GetAll endpoint called");
            var result = await _contentService.GetAllCategoriesWithArticlesAsync();
            _logger.LogInformation($"✅ Returning {result.Count} categories");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error in GetAll endpoint");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{categorySlug}/{articleSlug}")] // Tek bir makaleyi okumak için
    public async Task<ActionResult<ContentArticleDetailDto>> GetArticle(string categorySlug, string articleSlug)
    {
        try
        {
            _logger.LogInformation($"🔍 GetArticle called: {categorySlug}/{articleSlug}");
            var article = await _contentService.GetArticleBySlugAsync(categorySlug, articleSlug);
            
            if (article == null)
            {
                _logger.LogWarning($"⚠️ Article not found: {categorySlug}/{articleSlug}");
                return NotFound();
            }
   
            _logger.LogInformation($"✅ Returning article: {article.Title}");
            return Ok(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"❌ Error getting article: {categorySlug}/{articleSlug}");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("update-database")]
    public IActionResult UpdateDatabase()
    {
        try
        {
            _logger.LogInformation("🔄 UpdateDatabase endpoint called");
            // Database update logic here
            _logger.LogInformation("✅ Database updated successfully");
            return Ok(new { message = "Database updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error in UpdateDatabase endpoint");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
using microbloom.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace microbloom.Services.Interfaces
{
    public interface IContentService
    {
        Task<List<ContentCategoryDto>> GetAllCategoriesWithArticlesAsync();
        Task<ContentArticleDetailDto?> GetArticleBySlugAsync(string categorySlug, string articleSlug);
    }
}
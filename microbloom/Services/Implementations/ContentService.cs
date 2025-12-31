using Microsoft.EntityFrameworkCore;
using microbloom.DTOs;
using microbloom.Data;
using microbloom.Entities;
using microbloom.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microbloom.Services.Implementations
{
    public class ContentService : IContentService
    {
        private readonly KariyerDBContext _context;
        public ContentService(KariyerDBContext context) { _context = context; }

    public async Task<List<ContentCategoryDto>> GetAllCategoriesWithArticlesAsync()
    {
        var categories = await _context.ContentCategories
            .Include(c => c.Articles)
            .ToListAsync();

        return categories.Select(c => new ContentCategoryDto
        {
            Title = c.Title,
            Slug = c.Slug,
            Articles = (c.Articles ?? Enumerable.Empty<ContentArticle>()).Select(a => new ContentArticleDto
            {
                Title = a.Title,
                Slug = a.Slug,
                Summary = a.Summary
            }).ToList()
        }).ToList();
    }

    public async Task<ContentArticleDetailDto?> GetArticleBySlugAsync(string categorySlug, string articleSlug)
    {
        return await _context.ContentArticles
       .Where(a => a.ContentCategory != null && a.ContentCategory.Slug == categorySlug && a.Slug == articleSlug)
            .Select(a => new ContentArticleDetailDto
     {
       Title = a.Title,
     Content = a.Content
            }).FirstOrDefaultAsync();
    }
    }
}
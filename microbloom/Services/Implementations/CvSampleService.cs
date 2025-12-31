using Microsoft.EntityFrameworkCore;
using microbloom.Services.Interfaces;
using microbloom.DTOs;
using microbloom.Data;
using microbloom.Entities;

namespace microbloom.Services.Implementations
{
    public class CvSampleService : ICvSampleService
    {
        private readonly KariyerDBContext _context;

        public CvSampleService(KariyerDBContext context)
      {
   _context = context;
 }

        public async Task<List<CvSampleDto>> GetAllCvSamplesAsync()
      {
       return await _context.CvSamples
    .Select(cv => new CvSampleDto
    {
   Id = cv.Id,
     Title = cv.Title,
   Description = cv.Description,
  FileDownloadUrl = cv.FileDownloadUrl,
   ThumbnailImageUrl = cv.ThumbnailImageUrl
       })
       .ToListAsync();
  }

  public async Task<CvSampleDto?> GetCvSampleByIdAsync(int id)
  {
   var cvSample = await _context.CvSamples.FindAsync(id);
  if (cvSample == null) return null;

       return new CvSampleDto
            {
   Id = cvSample.Id,
     Title = cvSample.Title,
        Description = cvSample.Description,
      FileDownloadUrl = cvSample.FileDownloadUrl,
       ThumbnailImageUrl = cvSample.ThumbnailImageUrl
   };
     }
    }
}

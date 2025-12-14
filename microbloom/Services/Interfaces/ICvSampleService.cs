using microbloom.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace microbloom.Services.Interfaces
{
    public interface ICvSampleService
    {
        Task<List<CvSampleDto>> GetAllCvSamplesAsync();
        Task<CvSampleDto?> GetCvSampleByIdAsync(int id);
    }
}

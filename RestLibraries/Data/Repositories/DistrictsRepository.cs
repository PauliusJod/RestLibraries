using RestLibraries.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace RestLibraries.Data.Repositories
{
    public interface IDistrictsRepository
    {
        Task CreateAsync(District district);
        Task DeleteAsync(District district);
        Task<District?> GetDistrictAsync(int cityid, int districtid);
        Task<List<District>> GetDistrictsAsync(int cityid);
        Task UpdateAsync(District district);
    }

    public class DistrictsRepository : IDistrictsRepository
    {

        private readonly LibrariesDbContext _librariesDbContext;

        public DistrictsRepository(LibrariesDbContext librariesDbContext)
        {
            _librariesDbContext = librariesDbContext;
        }
        public async Task<District?> GetDistrictAsync(int cityid, int districtid)
        {
            return await _librariesDbContext.Districts.FirstOrDefaultAsync(o => o.City.Id == cityid && o.Id == districtid);
        }
        public async Task<List<District>> GetDistrictsAsync(int cityid)
        {
            return await _librariesDbContext.Districts.Where(o => o.City.Id == cityid).ToListAsync();
        }

        public async Task CreateAsync(District district)
        {
            _librariesDbContext.Districts.Add(district);
            await _librariesDbContext.SaveChangesAsync();

        }
        public async Task UpdateAsync(District district)
        {
            _librariesDbContext.Districts.Update(district);
            await _librariesDbContext.SaveChangesAsync();

        }
        public async Task DeleteAsync(District district)
        {
            _librariesDbContext.Districts.Remove(district);
            await _librariesDbContext.SaveChangesAsync();

        }
    }
}

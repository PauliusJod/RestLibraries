using RestLibraries.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace RestLibraries.Data.Repositories
{
    public interface ICitiesRepository
    {
        Task CreateAsync(City city);
        Task DeleteAsync(City city);
        Task<IReadOnlyList<Book>> GetAllCityBooks(int cityId);
        Task<IReadOnlyList<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId);
        Task UpdateAsync(City city);
    }

    public class CitiesRepository : ICitiesRepository
    {
        private readonly LibrariesDbContext _librariesDbContext;

        public CitiesRepository(LibrariesDbContext librariesDbContext)
        {
            _librariesDbContext = librariesDbContext;
        }

        public async Task<City?> GetCityAsync(int cityId)
        {
            return await _librariesDbContext.Cities.FirstOrDefaultAsync(o => o.Id == cityId);
        }
        public async Task<IReadOnlyList<City>> GetCitiesAsync()
        {
            return await _librariesDbContext.Cities.ToListAsync();
        }

        public async Task CreateAsync(City city)
        {
            _librariesDbContext.Cities.Add(city);
            await _librariesDbContext.SaveChangesAsync();

        }
        public async Task UpdateAsync(City city)
        {
            _librariesDbContext.Cities.Update(city);
            await _librariesDbContext.SaveChangesAsync();

        }
        public async Task DeleteAsync(City city)
        {
            _librariesDbContext.Cities.Remove(city);
            await _librariesDbContext.SaveChangesAsync();

        }
        public async Task<IReadOnlyList<Book>> GetAllCityBooks(int cityId)
        {
            return await _librariesDbContext.Books
                .AsNoTracking()
                .Include(o => o.library)
                .ThenInclude(o => o.City)
                .Where(o => o.library.City.Id == cityId)
                .ToListAsync();
        }

    }
}

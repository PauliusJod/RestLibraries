using RestLibraries.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace RestLibraries.Data.Repositories
{
    public interface ILibrariesRepository
    {
        Task CreateAsync(Library library);
        Task DeleteAsync(Library library);
        Task<List<Library>> GetLibrariesAsync(int cityId);
        Task<Library?> GetLibraryAsync(int cityId, int libraryId);
        Task UpdateAsync(Library library);
    }

    public class LibrariesRepository : ILibrariesRepository
    {

        private readonly LibrariesDbContext _librariesDbContext;

        public LibrariesRepository(LibrariesDbContext librariesDbContext)
        {
            _librariesDbContext = librariesDbContext;
        }

        public async Task<Library?> GetLibraryAsync(int cityId, int libraryId)
        {
            return await _librariesDbContext.Libraries.FirstOrDefaultAsync(o => o.City.Id == cityId && o.Id == libraryId);
        }
        public async Task<List<Library>> GetLibrariesAsync(int cityId)
        {
            return await _librariesDbContext.Libraries.Where(o => o.City.Id == cityId).ToListAsync();
        }

        public async Task CreateAsync(Library library)
        {
            _librariesDbContext.Libraries.Add(library);
            await _librariesDbContext.SaveChangesAsync();

        }
        public async Task UpdateAsync(Library library)
        {
            _librariesDbContext.Libraries.Update(library);
            await _librariesDbContext.SaveChangesAsync();

        }
        public async Task DeleteAsync(Library library)
        {
            _librariesDbContext.Libraries.Remove(library);
            await _librariesDbContext.SaveChangesAsync();

        }
    }
}



//public class LibrariesRepository : ILibrariesRepository
//{

//    private readonly LibrariesDbContext _librariesDbContext;

//    public LibrariesRepository(LibrariesDbContext librariesDbContext)
//    {
//        _librariesDbContext = librariesDbContext;
//    }

//    public async Task<Library?> GetLibraryAsync(int cityId, int districtId, int libraryId)
//    {
//        return await _librariesDbContext.Libraries.FirstOrDefaultAsync(o => o.District.City.Id == cityId && o.District.Id == districtId && o.Id == libraryId);
//    }
//    public async Task<List<Library>> GetLibrariesAsync(int cityId, int districtId)
//    {
//        return await _librariesDbContext.Libraries.Where(o => o.District.City.Id == cityId && o.District.Id == districtId).ToListAsync();
//    }

//    public async Task CreateAsync(Library library)
//    {
//        _librariesDbContext.Libraries.Add(library);
//        await _librariesDbContext.SaveChangesAsync();

//    }
//    public async Task UpdateAsync(Library library)
//    {
//        _librariesDbContext.Libraries.Update(library);
//        await _librariesDbContext.SaveChangesAsync();

//    }
//    public async Task DeleteAsync(Library library)
//    {
//        _librariesDbContext.Libraries.Remove(library);
//        await _librariesDbContext.SaveChangesAsync();

//    }
//}
using RestLibraries.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace RestLibraries.Data.Repositories
{
    public interface IBooksRepository
    {
        Task CreateAsync(Book book);
        Task DeleteAsync(Book book);
        Task<Book?> GetBookAsync(int cityid, int libraryid, int bookid);
        Task<List<Book>> GetBooksAsync(int cityid, int libraryid);
        Task UpdateAsync(Book book);
    }

    public class BooksRepository : IBooksRepository
    {

        private readonly LibrariesDbContext _librariesDbContext;

        public BooksRepository(LibrariesDbContext librariesDbContext)
        {
            _librariesDbContext = librariesDbContext;
        }
        public async Task<Book?> GetBookAsync(int cityid, int libraryid, int bookid)
        {
            return await _librariesDbContext.Books.FirstOrDefaultAsync(o => o.library.City.Id == cityid && o.library.Id == libraryid && o.BookId == bookid);
        }
        public async Task<List<Book>> GetBooksAsync(int cityid, int libraryid)
        {
            return await _librariesDbContext.Books.Where(o => o.library.City.Id == cityid && o.library.Id == libraryid).ToListAsync();
        }

        public async Task CreateAsync(Book book)
        {
            _librariesDbContext.Books.Add(book);
            await _librariesDbContext.SaveChangesAsync();

        }
        public async Task UpdateAsync(Book book)
        {
            _librariesDbContext.Books.Update(book);
            await _librariesDbContext.SaveChangesAsync();

        }
        public async Task DeleteAsync(Book book)
        {
            _librariesDbContext.Remove(book);
            await _librariesDbContext.SaveChangesAsync();

        }
    }
}

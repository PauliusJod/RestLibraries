using Microsoft.AspNetCore.Mvc;
using RestLibraries.Data.Entities;
using RestLibraries.Data.Dtos.Libraries;
using RestLibraries.Data.Dtos.Books;
using RestLibraries.Data.Repositories;
using AutoMapper;


namespace RestLibraries.Controllers
{

    [ApiController]
    [Route("api/cities/{cityId}/libraries/{libraryId}/books")]
    public class BooksController : ControllerBase
    {
        /*
             * api/v1/cities        GET List 200
             * api/v1/cities/{id}   GET One 200
             * api/v1/cities        POST Create 201
             * api/v1/cities/{id}   PUT/PATCH Modify 200
             * api/v1/cities/{id}   DELETE Remove 200/204
             */


        private readonly ILibrariesRepository _librariesRepository;
        private readonly IBooksRepository _booksRepository;
        private readonly ICitiesRepository _citiesRepository;

        public BooksController(ILibrariesRepository librariesRepository, IBooksRepository booksRepository, ICitiesRepository citiesRepository)
        {
            _librariesRepository = librariesRepository;
            _booksRepository = booksRepository;
            _citiesRepository = citiesRepository;
        }


        //api/v1/cities
        [HttpGet]
        public async Task<IEnumerable<BookDto>> GetBooks(int cityId, int libraryId)
        {
            var books = await _booksRepository.GetBooksAsync(cityId, libraryId);

            return books.Select(o => new BookDto(o.BookId, o.BookAuthor, o.BookName, o.BookDesc));
        }

        //api/v1/cities/{id}
        [HttpGet]
        [Route("{bookId}")]
        public async Task<ActionResult<BookDto>> GetBook(int cityId, int libraryId, int bookId)
        {
            var book = await _booksRepository.GetBookAsync(cityId, libraryId, bookId);

            // 404
            if (book == null)
                return NotFound();

            return new BookDto(book.BookId, book.BookAuthor, book.BookName, book.BookDesc);
        }
        //api/v1/cities
        [HttpPost]
        public async Task<ActionResult<BookDto>> Create(int cityId, int libraryId, CreateBookDto createBookDto)
        {

            var city = await _citiesRepository.GetCityAsync(cityId);
            var library = await _librariesRepository.GetLibraryAsync(cityId, libraryId);
            if (city == null || library == null)
                return NotFound($"ERROR city{cityId} or library{libraryId}");

            var book = new Book
            {
                BookAuthor = createBookDto.BookAuthor,
                BookName = createBookDto.BookName,
                BookDesc = createBookDto.BookDesc
            };

            book.library = library; //SVARBU
            await _booksRepository.CreateAsync(book);


            // 201
            return Created($"api/cities/{cityId}/libraries/{library.Id}/books/{book.BookId}", new CreateBookDto(book.BookAuthor, book.BookName, book.BookDesc));
        }

        //api/v1/cities/{id}
        [HttpPut]
        [Route("{bookId}")]
        public async Task<ActionResult<BookDto>> Update(int libraryId, int cityId, int bookId, UpdateBookDto updateBookDto)
        {
            //return NotFound($"ERROR aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa {bookId}");
            var book = await _booksRepository.GetBookAsync(cityId, libraryId, bookId);

            // 404
            if (book == null)
                return NotFound($"ERROR book {bookId}");

            book.BookDesc = updateBookDto.BookDesc;
            await _booksRepository.UpdateAsync(book);

            return Ok(new BookDto(book.BookId, book.BookAuthor, book.BookName, book.BookDesc));
        }

        //api/v1/cities/{id}
        [HttpDelete]
        [Route("{bookId}")]
        public async Task<ActionResult> Remove(int libraryId, int cityId, int bookId)
        {
            var book = await _booksRepository.GetBookAsync(cityId, libraryId, bookId);

            // 404
            if (book == null)
                return NotFound();
            await _booksRepository.DeleteAsync(book);

            // 204
            return NoContent();
        }


    }
}

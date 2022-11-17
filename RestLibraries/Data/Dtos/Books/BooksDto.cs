namespace RestLibraries.Data.Dtos.Books
{
    public record BookDto(int BookId, string BookAuthor, string BookName, string BookDesc);
    public record CreateBookDto(string BookAuthor, string BookName, string BookDesc);
    public record UpdateBookDto(string BookDesc);
}

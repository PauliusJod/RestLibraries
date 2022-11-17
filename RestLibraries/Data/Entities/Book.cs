namespace RestLibraries.Data.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookAuthor { get; set; }
        public string BookName { get; set; }
        public string BookDesc { get; set; }

        public Library library { get; set; }
    }
}

namespace RestLibraries.Data.Entities
{
    public class Library
    {
        public int Id { get; set; }
        public string LibraryName { get; set; }
        public int LibraryBookedBooks { get; set; }
        
        public District District { get; set; }


    }
}

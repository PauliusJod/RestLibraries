namespace RestLibraries.Data.Dtos.Libraries
{

    /*
             public int Id { get; set; }
        public string LibraryName { get; set; }
        public int LibraryBookedBooks { get; set; }
    */

    public record LibraryDto(int Id, string LibraryName, int LibraryBookedBooks);
    public record CreateLibraryDto(string LibraryName);
    public record UpdateLibraryDto(string LibraryName);


}

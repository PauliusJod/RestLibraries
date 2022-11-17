namespace RestLibraries.Auth
{
    public class LibrariesRoles
    {
        public const string Admin = nameof(Admin);
        public const string LibraryUser = nameof(LibraryUser);


        public static readonly IReadOnlyCollection<string> All = new[] { Admin, LibraryUser };

    }
}

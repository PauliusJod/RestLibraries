using RestLibraries.Data.Entities;
using Microsoft.EntityFrameworkCore;
using RestLibraries.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RestLibraries.Data
{
    public class LibrariesDbContext : IdentityDbContext<LibrariesUser>
    {
        public DbSet<City> Cities { get; set; }
        //public DbSet<District> Districts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Libraries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Server = localhost\\MSSQLSERVER01; Database = warehouseapi; Trusted_Connection = True;
            //optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLSERVER01;Database=LibrariesDb;Trusted_Connection=True;");
            //optionsBuilder.UseSqlServer("Server =DESKTOP-A2LFQQD; Database=LibrariesDb; Trusted_Connection = True");
            optionsBuilder.UseSqlServer("Server=tcp:battleshipserver.database.windows.net,1433;Initial Catalog=LibrariesDb;Persist Security Info=False;User ID=admin1;Password=Battleship1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}

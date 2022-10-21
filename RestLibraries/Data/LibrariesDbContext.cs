using RestLibraries.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace RestLibraries.Data
{
    public class LibrariesDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Library> Libraries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Server = localhost\\MSSQLSERVER01; Database = warehouseapi; Trusted_Connection = True;
            //optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLSERVER01;Database=LibrariesDb;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer("Server =DESKTOP-A2LFQQD; Database=LibrariesDb; Trusted_Connection = True");
        }
    }
}

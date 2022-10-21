using RestLibraries.Data;
using RestLibraries.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Microsoft.EntityFrameworkCore.SqlServer
// dotnet tool install --global dotnet -ef

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<LibrariesDbContext>();
builder.Services.AddTransient<ICitiesRepository, CitiesRepository>();
builder.Services.AddTransient<IDistrictsRepository, DistrictsRepository>();
builder.Services.AddTransient<ILibrariesRepository, LibrariesRepository>();


var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();

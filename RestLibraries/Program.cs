using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestLibraries.Auth;
using RestLibraries.Data;
using RestLibraries.Data.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
// Microsoft.EntityFrameworkCore.SqlServer
// dotnet tool install --global dotnet -ef

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddIdentity<LibrariesUser, IdentityRole>()
    .AddEntityFrameworkStores<LibrariesDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<LibrariesDbContext>();
builder.Services.AddTransient<ICitiesRepository, CitiesRepository>();
//builder.Services.AddTransient<IDistrictsRepository, DistrictsRepository>();
builder.Services.AddTransient<ILibrariesRepository, LibrariesRepository>();
builder.Services.AddTransient<IBooksRepository, BooksRepository>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();


builder.Services.AddScoped<AuthDbSeeder>();
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(PolicyNames.ResourceOwner, policy => policy.Requirements.Add(new ResourceOwnerRequirement()));
    });
builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerAuthorizationHandler>();



builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:ValidAudience"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:ValidIssuer"];
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));
    });



var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

var dbSeeder = app.Services.CreateScope().ServiceProvider.GetRequiredService<AuthDbSeeder>();
await dbSeeder.SeedAsync();

app.Run();

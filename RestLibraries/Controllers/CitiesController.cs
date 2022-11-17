using Microsoft.AspNetCore.Mvc;
using RestLibraries.Data.Entities;
using RestLibraries.Data.Dtos.Cities;
using RestLibraries.Data.Repositories;
using RestLibraries.Data.Dtos.Libraries;
using RestLibraries.Data.Dtos.Books;
using Microsoft.AspNetCore.Authorization;
using RestLibraries.Auth;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace RestLibraries.Controllers
{

    /*
     * api/v1/cities        GET List 200
     * api/v1/cities/{id}   GET One 200
     * api/v1/cities        POST Create 201
     * api/v1/cities/{id}   PUT/PATCH Modify 200
     * api/v1/cities/{id}   DELETE Remove 200/204
     */



    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {

        private readonly ICitiesRepository _citiesRepository;
        private readonly IAuthorizationService _authorizationService;

        public CitiesController(ICitiesRepository citiesRepository, IAuthorizationService authorizationService)
        {
            _citiesRepository = citiesRepository;
            _authorizationService = authorizationService;
        }


        //api/v1/cities
        [HttpGet]
        public async Task<IEnumerable<CityDto>> GetCities()
        {
            var cities = await _citiesRepository.GetCitiesAsync();

            return cities.Select(o => new CityDto(o.Id, o.Name, o.Description, o.AmountOfLibraries));
        }

        //api/v1/cities/{id}
        [HttpGet]
        [Route("{cityId}", Name = "GetCity")]
        public async Task<ActionResult<CityDto>> GetCity(int cityid)
        {
            var city = await _citiesRepository.GetCityAsync(cityid);

            // 404
            if (city == null)
                return NotFound();

            return new CityDto(city.Id, city.Name, city.Description, city.AmountOfLibraries);
        }





        //api/v1/cities
        [HttpPost]
        [Authorize(Roles = LibrariesRoles.LibraryUser)]
        public async Task<ActionResult<CityDto>> Create(CreateCityDto createCityDto)
        {
            var city = new City
            {
                Name = createCityDto.Name,
                Description = createCityDto.Description,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) //"397cf6b0-c64a-46df-9861-c05bbdb612a8"      -Paulius1
            };
            await _citiesRepository.CreateAsync(city);


            // 201
            return CreatedAtAction("GetCity", new { cityId = city.Id }, new CreateCityDto(city.Name, city.Description));

        }

        //api/v1/cities/{id}
        [HttpPut]
        [Route("{cityid}")]
        [Authorize(Roles = LibrariesRoles.LibraryUser)]
        public async Task<ActionResult<CityDto>> Update(int cityid, UpdateCityDto updateCityDto)
        {
            var city = await _citiesRepository.GetCityAsync(cityid);

            // 404
            if (city == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, city, PolicyNames.ResourceOwner);

            if (!authorizationResult.Succeeded)
                return Forbid("Not available for you!");    //402 arba NotFound() 404



            city.Description = updateCityDto.Description;
            await _citiesRepository.UpdateAsync(city);

            return Ok(new CityDto(city.Id, city.Name, city.Description, city.AmountOfLibraries));
        }

        //api/v1/cities/{id}
        [HttpDelete]
        [Route("{cityid}")]
        [Authorize(Roles = LibrariesRoles.LibraryUser)]
        public async Task<ActionResult> Remove(int cityid)
        {
            var city = await _citiesRepository.GetCityAsync(cityid);

            // 404
            if (city == null)
                return NotFound();
            await _citiesRepository.DeleteAsync(city);




            // 204
            return NoContent();
        }


        [HttpGet]
        [Route("{cityId}/cityBooks")]
        public async Task<IEnumerable<BookDto>> GetCityBooks(int cityId)
        {
            var books = await _citiesRepository.GetAllCityBooks(cityId);

            return books.Select(o => new BookDto(o.BookId,o.BookAuthor, o.BookName, o.BookDesc));

        }





    }
}

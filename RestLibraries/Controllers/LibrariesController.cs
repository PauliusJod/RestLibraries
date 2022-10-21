using Microsoft.AspNetCore.Mvc;
using RestLibraries.Data.Entities;
using RestLibraries.Data.Dtos.Libraries;
using RestLibraries.Data.Repositories;
using AutoMapper;

namespace RestLibraries.Controllers
{

    [ApiController]
    [Route("api/cities/{cityId}/districts/{districtId}/libraries")]
    public class LibrariesController : ControllerBase
    {
        /*
             * api/v1/cities        GET List 200
             * api/v1/cities/{id}   GET One 200
             * api/v1/cities        POST Create 201
             * api/v1/cities/{id}   PUT/PATCH Modify 200
             * api/v1/cities/{id}   DELETE Remove 200/204
             */


        private readonly ILibrariesRepository _librariesRepository;
        private readonly IDistrictsRepository _districtsRepository;
        private readonly ICitiesRepository _citiesRepository;

        public LibrariesController(ILibrariesRepository librariesRepository, IDistrictsRepository districtsRepository, ICitiesRepository citiesRepository)
        {
            _librariesRepository = librariesRepository;
            _districtsRepository = districtsRepository;
            _citiesRepository = citiesRepository;
        }


        //api/v1/cities
        [HttpGet]
        public async Task<IEnumerable<LibraryDto>> GetLibraries(int cityId, int districtId)
        {
            var libraries = await _librariesRepository.GetLibrariesAsync(cityId, districtId);

            return libraries.Select(o => new LibraryDto(o.Id, o.LibraryName, o.LibraryBookedBooks));
        }

        //api/v1/cities/{id}
        [HttpGet]
        [Route("{libraryId}")]
        public async Task<ActionResult<LibraryDto>> GetLibrary(int cityId, int districtId, int libraryId)
        {
            var libraries = await _librariesRepository.GetLibraryAsync(cityId, districtId, libraryId);

            // 404
            if (libraries == null)
                return NotFound();

            return new LibraryDto(libraries.Id, libraries.LibraryName, libraries.LibraryBookedBooks);
        }
        //api/v1/cities
        [HttpPost]
        public async Task<ActionResult<LibraryDto>> Create(int cityId, int districtId, CreateLibraryDto createLibraryDto)
        {

            var city = await _citiesRepository.GetCityAsync(cityId);
            var district = await _districtsRepository.GetDistrictAsync(cityId, districtId);
            if (city == null || district == null)
                return NotFound($"ERROR city{cityId} or district{districtId}");

            var library = new Library
            {
                LibraryName = createLibraryDto.LibraryName
            };

            library.District = district; //SVARBU
            await _librariesRepository.CreateAsync(library);


            // 201
            return Created($"api/cities/{cityId}/districts/{district.Id}/libraries/{library.Id}", new CreateLibraryDto(library.LibraryName));
        }

        //api/v1/cities/{id}
        [HttpPut]
        [Route("{libraryId}")]
        public async Task<ActionResult<LibraryDto>> Update(int libraryId, int cityId, int districtId, UpdateLibraryDto updateLibraryDto)
        {
            var library = await _librariesRepository.GetLibraryAsync(cityId, districtId, libraryId);

            // 404
            //if (city == null || district == null || library == null)
            //    return NotFound($"ERROR city{cityId} or district{districtId} or library{libraryId}");

            //if (city == null)
            //    return NotFound($"ERROR city{cityId}");
            //if (district == null)
            //    return NotFound($"ERROR district{districtId}");
            if(library == null)
                return NotFound($"ERROR library {libraryId}");

            library.LibraryName = updateLibraryDto.LibraryName;
            await _librariesRepository.UpdateAsync(library);

            return Ok(new LibraryDto(library.Id, library.LibraryName, library.LibraryBookedBooks));
        }

        //api/v1/cities/{id}
        [HttpDelete]
        [Route("{libraryId}")]
        public async Task<ActionResult> Remove(int libraryId, int cityId, int districtId)
        {
            var library = await _librariesRepository.GetLibraryAsync(cityId, districtId, libraryId);

            // 404
            if (library == null)
                return NotFound();
            await _librariesRepository.DeleteAsync(library);

            // 204
            return NoContent();
        }



    }
}

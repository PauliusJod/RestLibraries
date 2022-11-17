//using Microsoft.AspNetCore.Mvc;
//using RestLibraries.Data.Entities;
//using RestLibraries.Data.Dtos.Districts;
//using RestLibraries.Data.Repositories;
//using AutoMapper;

//namespace RestLibraries.Controllers
//{    
//    [ApiController]
//    [Route("api/cities/{cityId}/districts")]
//    public class DistrictsController : ControllerBase
//    {
//        /*
//             * api/cities/{cityid}/districts         GET List 200
//             * api/cities/{cityid}/districts/{id}    GET One 200
//             * api/cities/{cityid}/districts         POST Create 201
//             * api/cities/{cityid}/districts/{id}    PUT/PATCH Modify 200
//             * api/cities/{cityid}/districts/{id}    DELETE Remove 200/204
//             */


//        private readonly IDistrictsRepository _districtsRepository;
//        private readonly ICitiesRepository _citiesRepository;
//        private readonly IMapper _mapper;

//        public DistrictsController(IDistrictsRepository districtsRepository, ICitiesRepository citiesRepository, IMapper mapper)
//        {
//            _districtsRepository = districtsRepository;
//            _citiesRepository = citiesRepository;
//            _mapper = mapper;
//        }


//        //api/v1/cities
//        [HttpGet]
//        public async Task<IEnumerable<DistrictDto>> GetDistricts(int cityId)
//        {
//            var districts = await _districtsRepository.GetDistrictsAsync(cityId);

//            return districts.Select(o => new DistrictDto(o.Id, o.Name,o.Description, o.DistLibraries));
//        }
//        //api/v1/cities/{id}
//        [HttpGet("{districtId}")]
//        //[Route()]
//        public async Task<ActionResult<DistrictDto>> GetDistrict(int cityId, int districtId)
//        {
//        //var city = await _districtsRepository
//            var district = await _districtsRepository.GetDistrictAsync(cityId, districtId);
//            // 404
//            if(district == null /*|| district.City.Id == null*/)
//                return NotFound();
//            //return new CityDto(city.Id, city.Name, city.Description, city.AmountOfLibraries);
//            return new DistrictDto(district.Id, district.Name, district.Description, district.DistLibraries);
//        }






//        //api/v1/cities
//        [HttpPost]
//        public async Task<ActionResult<DistrictDto>> Create(int cityId, CreateDistrictDto districtDto)
//        {
//            var city = await _citiesRepository.GetCityAsync(cityId);
//            if (city == null)
//                return NotFound($"erorrasss");

//            var district = new District
//            {
//                //Id = districtDto.Id,
//                Name = districtDto.Name,
//                Description = districtDto.Description,
//                DistLibraries = districtDto.DistLibraries
//                //cityId = cityId
//                //City = city
//            };

//            district.City = city; //SVARBU
//            await _districtsRepository.CreateAsync(district);

//            // 201 /{district.Id}
//            //return CreatedAtAction($"/api/cities/{cityId}/districts", new { districtId = district.Id }, new CreateDistrictDto(district.City.Id, district.DistrictName, district.DistrictDescription));
//            return Created($"api/cities/{cityId}/districts/{district.Id}", new CreateDistrictDto(district.Name, district.Description,district.DistLibraries));
//            //new { cityId = city.Id }, new CreateCityDto(city.Name, city.Description)
//        }










//        //api/v1/cities/{id}
//        [HttpPut("{districtId}")]
//        //[Route]
//        public async Task<ActionResult<DistrictDto>> Update(int cityid, int districtId, UpdateDistrictDto updateDistrictDto)
//        {
//            var district = await _districtsRepository.GetDistrictAsync(cityid,districtId);

//            // 404
//            if (district == null)
//                return NotFound();

//            district.Description = updateDistrictDto.Description;
//            await _districtsRepository.UpdateAsync(district);

//            return Ok(new DistrictDto(district.Id, district.Name, district.Description, district.DistLibraries));
//        }

















//        //api/v1/cities/{id}
//        [HttpDelete("{districtId}")]
//        //[Route]
//        public async Task<ActionResult> Remove(int cityId, int districtId)
//        {
//            var district = await _districtsRepository.GetDistrictAsync(cityId, districtId);

//            // 404
//            if (district == null)
//                return NotFound();
//            await _districtsRepository.DeleteAsync(district);

//            // 204
//            return NoContent();
//        }



//    }
//}

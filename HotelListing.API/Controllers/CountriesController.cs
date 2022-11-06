using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Data.Dtos;
using HotelListing.API.EF;
using HotelListing.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CountriesController> _logger;

        public ICountriesRepository _CountriesRepository { get; }

        public CountriesController(IMapper mapper, ICountriesRepository countriesRepository, ILogger<CountriesController> logger)
        {
            _mapper = mapper;
            _CountriesRepository = countriesRepository;
            _logger = logger;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<PagedResult<GetCountryDto>>> GetCountries([FromQuery] QueryParameters parameters)
        {
            return await _CountriesRepository.GetAllAsync<GetCountryDto>(parameters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCountryDetailsDto>> GetCountry(int id)
        {
            var country = await _CountriesRepository.GetDetailsAsync(id);
            if (country == null)
            {
                _logger.LogWarning($"No record found in {nameof(GetCountry)} with id: {id}.");
                return NotFound();
            }
                

            return _mapper.Map<GetCountryDetailsDto>(country);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updatedCountry)
        {
            if (id != updatedCountry.Id)
            {
                return BadRequest();
            }

            //_context.Entry(country).State = EntityState.Modified;

            var country = await _CountriesRepository.GetAsync(id);
            if (country == null)
                return NotFound();

            _mapper.Map(updatedCountry, country);
            await _CountriesRepository.UpdateAsync(country);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createdCountry)
        {
            var country = _mapper.Map<Country>(createdCountry);

            await _CountriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _CountriesRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await _CountriesRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}

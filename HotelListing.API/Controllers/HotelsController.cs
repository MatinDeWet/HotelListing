using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Data.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHotelsRepository _hotelsContext;

        public HotelsController(IMapper mapper, IHotelsRepository hotelsContext)
        {
            _hotelsContext = hotelsContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GetHotelDto>>> GetHotels(QueryParameters parameters)
        {
            return await _hotelsContext.GetAllAsync<GetHotelDto>(parameters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelsContext.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetHotelDto>(hotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelDto updatedHotel)
        {
            if (id != updatedHotel.Id)
            {
                return BadRequest();
            }

            var hotel = await _hotelsContext.GetAsync(updatedHotel.Id);

            _mapper.Map(updatedHotel, hotel);

            await _hotelsContext.UpdateAsync(hotel);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto createdHotel)
        {

            await _hotelsContext.AddAsync(_mapper.Map<Hotel>(createdHotel));

            return CreatedAtAction("GetHotel", new { id = createdHotel.Id }, createdHotel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelsContext.GetAsync(id);
            if (hotel == null)
                return NotFound();

            await _hotelsContext.DeleteAsync(id);
            return NoContent();
        }
    }
}

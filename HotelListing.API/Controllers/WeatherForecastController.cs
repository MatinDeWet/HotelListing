using HotelListing.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "{id}")]
        public IEnumerable<WeatherForecast> GetWeatherForecast(int id)
        {
            throw new NotFoundException(nameof(GetWeatherForecast),null);
        }
    }
}
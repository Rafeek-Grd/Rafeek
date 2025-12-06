using Microsoft.AspNetCore.Mvc;

namespace Rafeek.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("greet/{name}")]
        public IActionResult Greet(string name)
        {
            var greeting = $"Hello, {name}! Welcome to the Weather Forecast API.";
            return Ok(greeting);
        }

        [HttpGet("temperature/{celsius}")]
        public IActionResult ConvertToFahrenheit(int celsius)
        {
            var fahrenheit = (celsius * 9 / 5) + 32;
            return Ok(fahrenheit);
        }
    }
}

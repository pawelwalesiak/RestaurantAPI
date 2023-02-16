using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers
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

        private readonly IWeatherForcastService _service;

        // obiek service jest zaleznoscia dla klasy WeatherForecastSERVICEprzez to jestesmy silnie powiazani SERVICE JEST ZALEZNOSCIA DLA KONROLERA 
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForcastService service)
        {
            _logger = logger;
            //dodanie nowego obiektu w kontrolerze klasy weatherforecast
            _service = service;
        }

        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Generate([FromQuery] int count,
            [FromBody] TemperatureRequest request)
        {
            if (count < 0 || request.Max < request.Min)
            {
                return BadRequest();
            }

            var result = _service.Get(count, request.Min, request.Max);
            return Ok(result);
        }


    }

}
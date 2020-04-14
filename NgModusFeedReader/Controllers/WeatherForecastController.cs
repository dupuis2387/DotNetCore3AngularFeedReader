using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModusCreateSampleApp.Data;
using ModusCreateSampleApp.Data.Entities;

namespace NgModusFeedReader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IAppDbRepository _repo;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IAppDbRepository repo, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _repo = repo;
        }

        /*[HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }*/
        [HttpGet]
        public IEnumerable<FeedItem> Get()
        {
            var item1 = _repo.GetFeedItemsByFeedId(1);
            var item2 = _repo.GetFeedItemsByFeedId(2);
            return new FeedItem[] { item1.First(), item2.First() };
        }

    }
}

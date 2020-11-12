using System;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Web.Data
{
    public class WeatherForecastService
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            (
                startDate.AddDays(index),
                rng.Next(-20, 55),
                Summaries[rng.Next(Summaries.Length)]
            )).ToArray());
        }
    }
}
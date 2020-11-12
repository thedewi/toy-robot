using System;

namespace Robot.Web.Data
{
    public class WeatherForecast
    {
        public DateTime Date { get; }

        public int TemperatureC { get; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public string Summary { get; }

        public WeatherForecast(DateTime date, int temperatureC, string summary)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }
    }
}
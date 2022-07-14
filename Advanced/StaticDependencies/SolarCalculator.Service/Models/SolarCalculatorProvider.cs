using Innovative.SolarCalculator;
using System;

namespace SolarCalculator.Service.Models
{
    //{
    //    "results": {
    //        "sunrise":"6:39:53 AM",
    //        "sunset":"6:48:32 PM",
    //        "solar_noon":"12:44:12 PM",
    //        "day_length":"12:08:38.8300000" },
    //    "status":"OK"
    //}

    public class SolarCalculatorProvider
    {
        public static SolarCalculatorResult GetSolarTimes(
            DateTime date, double latitude, double longitude)
        {
            var result = new SolarCalculatorResult();

            try
            {
                var solarTimes = new SolarTimes(date, latitude, longitude);
                result.results.sunrise = solarTimes.Sunrise.ToLongTimeString();
                result.results.sunset = solarTimes.Sunset.ToLongTimeString();
                result.results.solar_noon = solarTimes.SolarNoon.ToLongTimeString();
                result.results.day_length = solarTimes.SunlightDuration.ToString();
                result.status = "OK";
            }
            catch (Exception)
            {
                result.results = null;
                result.status = "ERROR";
            }
            return result;
        }
    }
}
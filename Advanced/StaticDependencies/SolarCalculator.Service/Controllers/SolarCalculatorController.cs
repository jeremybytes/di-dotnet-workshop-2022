using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolarCalculator.Service.Models;

namespace SolarCalculator.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolarCalculatorController : ControllerBase
    {
        [HttpGet("")]
        public SolarCalculatorResult GetWithQueryString(double lat, double lng, DateTime date)
        {
            var result = SolarCalculatorProvider.GetSolarTimes(date, lat, lng);
            return result;
        }

        [HttpGet("{lat}/{lng}/{date}")]
        public SolarCalculatorResult GetWithRoute(double lat, double lng, DateTime date)
        {
            var result = SolarCalculatorProvider.GetSolarTimes(date, lat, lng);
            return result;
        }
    }
}

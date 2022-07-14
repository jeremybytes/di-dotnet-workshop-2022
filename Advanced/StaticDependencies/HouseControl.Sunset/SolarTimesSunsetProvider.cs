﻿using Innovative.SolarCalculator;
using System;

namespace HouseControl.Sunset
{
    public class SolarTimesSunsetProvider : ISunsetProvider
    {
        private readonly double latitude;
        private readonly double longitude;

        public SolarTimesSunsetProvider(LatLongLocation latLong)
        {
            latitude = latLong.Latitude;
            longitude = latLong.Longitude;
        }

        public DateTimeOffset GetSunset(DateTime date)
        {
            var solarTimes = new SolarTimes(date, latitude, longitude);
            return new DateTimeOffset(solarTimes.Sunset);
        }

        public DateTimeOffset GetSunrise(DateTime date)
        {
            var solarTimes = new SolarTimes(date, latitude, longitude);
            return new DateTimeOffset(solarTimes.Sunrise);
        }
    }
}

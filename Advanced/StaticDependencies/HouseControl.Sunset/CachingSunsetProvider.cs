using System;
using System.Collections.Generic;
using System.Text;

namespace HouseControl.Sunset
{
    public class CachingSunsetProvider : ISunsetProvider
    {
        private DateTime dataDate;
        private DateTimeOffset sunrise;
        private DateTimeOffset sunset;

        ISunsetProvider wrappedProvider;

        public CachingSunsetProvider(ISunsetProvider wrappedSunsetProvider)
        {
            wrappedProvider = wrappedSunsetProvider;
        }

        public DateTimeOffset GetSunrise(DateTime date)
        {
            ValidateCache(date);
            return sunrise;
        }

        public DateTimeOffset GetSunset(DateTime date)
        {
            ValidateCache(date);
            return sunset;
        }

        private void ValidateCache(DateTime date)
        {
            if (dataDate != date)
            {
                sunrise = wrappedProvider.GetSunrise(date);
                sunset = wrappedProvider.GetSunset(date);
                dataDate = date;
            }
        }
    }
}

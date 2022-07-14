using Innovative.SolarCalculator;

namespace HouseControl.Sunset;

public class SolarTimesSunsetProvider : ISunsetProvider
{
    public DateTimeOffset GetSunset(DateTime date)
    {
        var solarTimes = new SolarTimes(date, 38.1884, -85.9569);
        return new DateTimeOffset(solarTimes.Sunset);
    }

    public DateTimeOffset GetSunrise(DateTime date)
    {
        var solarTimes = new SolarTimes(date, 38.1884, -85.9569);
        return new DateTimeOffset(solarTimes.Sunrise);
    }
}

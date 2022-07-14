namespace HouseControl.Sunset;

public interface ISunsetProvider
{
    DateTimeOffset GetSunset(DateTime date);
    DateTimeOffset GetSunrise(DateTime date);
}

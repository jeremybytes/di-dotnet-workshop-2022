using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseControl.Sunset
{
    public interface ISunsetProvider
    {
        DateTimeOffset GetSunset(DateTime date);
        DateTimeOffset GetSunrise(DateTime date);
    }
}

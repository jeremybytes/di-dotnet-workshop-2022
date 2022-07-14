using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseControl.Library
{
    public interface ITimeProvider
    {
        DateTimeOffset Now();
    }
}

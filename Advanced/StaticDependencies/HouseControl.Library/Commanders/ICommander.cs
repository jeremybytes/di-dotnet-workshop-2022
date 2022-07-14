using System;
namespace HouseControl.Library
{
    public interface ICommander
    {
        void SendCommand(string message);
    }
}

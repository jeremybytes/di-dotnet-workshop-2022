using System;

namespace HouseControl.Library
{
    public class FakeCommander : ICommander
    {
        public void SendCommand(string message)
        {
        #if DEBUG
            Console.WriteLine(message);
        #endif
        }
    }
}

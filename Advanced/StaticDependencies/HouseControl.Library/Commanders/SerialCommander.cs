using System;
using System.Configuration;
using System.IO.Ports;

namespace HouseControl.Library
{
    public class SerialCommander : ICommander
    {
        private SerialPort serialPort;
        private int baud = 115200;
        private int databits = 8;
        private Parity parity = Parity.None;
        private StopBits stopbits = StopBits.One;

        public SerialCommander()
        {
            string port = ConfigurationManager.AppSettings["ComPort"] ?? "COM3";
            serialPort = new SerialPort(port, baud, parity, databits, stopbits);
        }

        public void SendCommand(string message)
        {
            serialPort.Open();
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;

            foreach (var bit in message)
            {
                switch (bit)
                {
                    case '0':
                        serialPort.RtsEnable = false;
                        serialPort.RtsEnable = true;
                        break;
                    case '1':
                        serialPort.DtrEnable = false;
                        serialPort.DtrEnable = true;
                        break;
                    default:
                        throw new ArgumentException(
                            "Message can only contain '1' and '0' characters");
                }
            }

            serialPort.RtsEnable = false;
            serialPort.DtrEnable = false;
            serialPort.Close();
        }
    }
}

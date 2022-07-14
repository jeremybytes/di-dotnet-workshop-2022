using HouseControl.Library;
using HouseControl.Sunset;
using Ninject;
using System;

namespace HouseControlAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Test");

            var controller = InitializeHouseController();

            controller.SendCommand(5, DeviceCommands.On);
            controller.SendCommand(5, DeviceCommands.Off);

            var currentTime = DateTime.Now;
            controller.ScheduleOneTimeItem(currentTime.AddMinutes(1), 1, DeviceCommands.On);
            controller.ScheduleOneTimeItem(currentTime.AddMinutes(2), 2, DeviceCommands.On);
            controller.ScheduleOneTimeItem(currentTime.AddMinutes(3), 1, DeviceCommands.Off);
            controller.ScheduleOneTimeItem(currentTime.AddMinutes(4), 2, DeviceCommands.Off);

            Console.WriteLine("Test Completed");

            string command = "";
            while (command != "q")
            {
                command = Console.ReadLine();
                if (command == "s")
                {
                    var schedule = controller.GetCurrentScheduleItems();
                    foreach (var item in schedule)
                    {
                        Console.WriteLine("{0} - {1} ({2}), Device: {3}, Command: {4}",
                            item.Info.EventTime.ToString("G"),
                            item.Info.TimeType.ToString(),
                            item.Info.RelativeOffset.ToString(),
                            item.Device.ToString(),
                            item.Command.ToString());
                    }
                }
                if (command == "r")
                {
                    controller.ReloadSchedule();
                }
            }
        }

        private static HouseController InitializeHouseController()
        {
            IKernel container = new StandardKernel();

            container.Bind<LatLongLocation>()
                .ToConstant(new LatLongLocation(51.5202, -0.0959));

            container.Bind<ISunsetProvider>()
                .To<CachingSunsetProvider>()
                .InSingletonScope()
                .WithConstructorArgument<ISunsetProvider>(
                    container.Get<SolarTimesSunsetProvider>());

            var sunset = container.Get<ISunsetProvider>()
                .GetSunset(DateTime.Today.AddDays(1));
            Console.WriteLine($"Sunset Tomorrow: {sunset:G}");

            container.Bind<ScheduleFileName>().ToConstant(
                new ScheduleFileName(
                    AppDomain.CurrentDomain.BaseDirectory + "ScheduleData"));

            container.Bind<HouseController>().ToSelf()
                .WithPropertyValue("Commander", container.Get<FakeCommander>());

            var controller = container.Get<HouseController>();

            return controller;
        }

    }
}

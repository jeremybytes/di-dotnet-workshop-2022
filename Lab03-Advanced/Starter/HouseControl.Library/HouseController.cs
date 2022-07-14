using System.Timers;

namespace HouseControl.Library;

public class HouseController
{
    private ICommander? commander;
    public ICommander Commander
    {
        get => commander ??= new SerialCommander();
        set => commander = value;
    }

    private System.Timers.Timer scheduler = new(60000);
    private Schedule schedule;

    public HouseController(Schedule schedule)
    {
        this.schedule = schedule;

        scheduler.Elapsed += scheduler_Elapsed;
        scheduler.AutoReset = true;
        scheduler.Start();
    }

    private void scheduler_Elapsed(object? sender, ElapsedEventArgs e)
    {
        var itemsToProcess = schedule.GetCurrentScheduleItems();

        foreach (var item in itemsToProcess)
            SendCommand(item.Device, item.Command);

#if DEBUG
        Console.Write("Schedule Items Processed: {0} - ",
            itemsToProcess.Count().ToString());
#endif

        schedule.RollSchedule();

#if DEBUG
        Console.WriteLine("Total Items: {0} - Active Items: {1}",
            schedule.Count.ToString(),
            schedule.Count(si => si.IsEnabled));
#endif
    }

    public void ResetAll()
    {
        for (int i = 1; i <= 8; i++)
        {
            SendCommand(i, DeviceCommands.Off);
        }
    }

    public void ScheduleOneTimeItem(DateTimeOffset time, int device,
        DeviceCommands command)
    {
        var scheduleItem = new ScheduleItem(
            device,
            command,
            new ScheduleInfo()
            {
                EventTime = time,
                Type = ScheduleType.Once,
            },
            true,
            ""
        );
        schedule.Add(scheduleItem);
    }

    public void SendCommand(int device, DeviceCommands command)
    {
        var message = MessageGenerator.GetMessage(device, command);
        Commander.SendCommand(message);
        Console.WriteLine("{0} - Device: {1}, Command: {2}",
            DateTime.Now.ToString("G"), device.ToString(), command.ToString());
    }

    public List<ScheduleItem> GetCurrentScheduleItems()
    {
        return schedule.Where(i => i.IsEnabled).ToList();
    }

    public void ReloadSchedule()
    {
        schedule.LoadSchedule();
    }

    public void SaveSchedule()
    {
        schedule.SaveSchedule();
    }
}

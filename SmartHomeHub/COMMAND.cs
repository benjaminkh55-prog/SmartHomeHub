public interface ICommand
{
    void Execute();
}

public class TurnOnLampCommand : ICommand
{
    private Lamp lamp;

    public TurnOnLampCommand(Lamp lamp)
    {
        this.lamp = lamp;
    }

    public void Execute()
    {
        lamp.TurnOn();
    }
}

public class SetTemperatureCommand : ICommand
{
    private Thermostat thermostat;
    private int temperature;

    public SetTemperatureCommand(Thermostat thermostat, int temp)
    {
        this.thermostat = thermostat;
        this.temperature = temp;
    }

    public void Execute()
    {
        thermostat.SetTemperature(temperature);
    }
}

public class LockDoorCommand : ICommand
{
    private DoorLock door;

    public LockDoorCommand(DoorLock door)
    {
        this.door = door;
    }

    public void Execute()
    {
        door.Lock();
    }
}

// =========================
// INVOKER
// =========================

public class CommandInvoker
{
    private Queue<ICommand> queue = new();
    private List<ICommand> history = new();

    public void AddCommand(ICommand cmd)
    {
        queue.Enqueue(cmd);
        Logger.Instance.Log("Command added");
    }

    public void Run()
    {
        while (queue.Count > 0)
        {
            var cmd = queue.Dequeue();
            cmd.Execute();
            history.Add(cmd);
        }
    }

    public void ReplayLast(int count)
    {
        Console.WriteLine($"Replaying last {count} commands...");
        foreach (var cmd in history.TakeLast(count))
        {
            cmd.Execute();
        }
    }
}
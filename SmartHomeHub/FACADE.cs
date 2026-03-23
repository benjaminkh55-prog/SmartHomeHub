public class SmartHomeFacade
{
    private List<IDevice> devices = new();
    private CommandInvoker invoker = new();
    private IModeStrategy mode = new NormalMode();

    public void AddDevice(IDevice device)
    {
        devices.Add(device);
        Logger.Instance.Log($"{device.Name} added");
    }

    public void SetMode(IModeStrategy newMode)
    {
        mode = newMode;
        Logger.Instance.Log($"Mode changed to {newMode.GetType().Name}");
    }

    public void RunCommand(ICommand cmd, string action)
    {
        if (mode.CanExecute(action))
        {
            invoker.AddCommand(cmd);
        }
        else
        {
            Console.WriteLine("Action blocked by current mode");
        }
    }

    public void ExecuteAll()
    {
        invoker.Run();
    }

    public void ReplayLast(int count)
    {
        invoker.ReplayLast(count);
    }

    public void MorningRoutine(Lamp lamp, Thermostat thermostat)
    {
        Console.WriteLine("Morning routine...");

        RunCommand(new TurnOnLampCommand(lamp), "ON");
        RunCommand(new SetTemperatureCommand(thermostat, 22), "TEMP");

        ExecuteAll();
    }
}
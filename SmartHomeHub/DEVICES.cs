public interface IDevice
{
    string Name { get; }
}

public class Lamp : IDevice
{
    public string Name => "Lamp";

    public void TurnOn()
    {
        DeviceEventManager.Notify($"{Name} turned ON");
    }

    public void TurnOff()
    {
        DeviceEventManager.Notify($"{Name} turned OFF");
    }
}

public class Thermostat : IDevice
{
    public string Name => "Thermostat";

    public void SetTemperature(int temp)
    {
        DeviceEventManager.Notify($"{Name} set to {temp}°C");
    }
}

public class DoorLock : IDevice
{
    public string Name => "DoorLock";

    public void Lock()
    {
        DeviceEventManager.Notify($"{Name} LOCKED");
    }

    public void Unlock()
    {
        DeviceEventManager.Notify($"{Name} UNLOCKED");
    }
}

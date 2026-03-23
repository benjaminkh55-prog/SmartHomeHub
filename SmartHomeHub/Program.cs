using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHomeHub
{
    // =========================
    // OBSERVER
    // =========================

    public interface IObserver
    {
        void Update(string message);
    }

    public static class DeviceEventManager
    {
        private static List<IObserver> observers = new();

        public static void Subscribe(IObserver observer)
            => observers.Add(observer);

        public static void Notify(string message)
        {
            foreach (var obs in observers)
                obs.Update(message);
        }
    }

    public class Dashboard : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"[Dashboard] {message}");
        }
    }

    public class AuditLogger : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"[Audit] {message}");
        }
    }

    public class SystemLoggerObserver : IObserver
    {
        public void Update(string message)
        {
            Logger.Instance.Log(message);
        }
    }

    // =========================
    // SINGLETON
    // =========================

    public class Logger
    {
        private static Logger instance;

        private Logger() { }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                    instance = new Logger();
                return instance;
            }
        }

        public void Log(string message)
        {
            Console.WriteLine($"[LOG] {message}");
        }
    }

    // =========================
    // DEVICES
    // =========================

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

    // =========================
    // COMMAND
    // =========================

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

    // =========================
    // STRATEGY
    // =========================

    public interface IModeStrategy
    {
        bool CanExecute(string action);
    }

    public class NormalMode : IModeStrategy
    {
        public bool CanExecute(string action) => true;
    }

    public class EcoMode : IModeStrategy
    {
        public bool CanExecute(string action)
        {
            if (action == "ALL_ON")
                return false;

            return true;
        }
    }

    public class PartyMode : IModeStrategy
    {
        public bool CanExecute(string action)
        {
            return true;
        }
    }

    // =========================
    // FACADE
    // =========================

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

    // =========================
    // PROGRAM
    // =========================

    class Program
    {
        static void Main(string[] args)
        {
            var facade = new SmartHomeFacade();

            var lamp = new Lamp();
            var thermostat = new Thermostat();
            var door = new DoorLock();

            // Add devices
            facade.AddDevice(lamp);
            facade.AddDevice(thermostat);
            facade.AddDevice(door);

            // Observers
            DeviceEventManager.Subscribe(new Dashboard());
            DeviceEventManager.Subscribe(new AuditLogger());
            DeviceEventManager.Subscribe(new SystemLoggerObserver());

            // Mode
            facade.SetMode(new NormalMode());

            // Commands
            facade.RunCommand(new TurnOnLampCommand(lamp), "ON");
            facade.RunCommand(new SetTemperatureCommand(thermostat, 25), "TEMP");
            facade.RunCommand(new LockDoorCommand(door), "LOCK");

            facade.ExecuteAll();

            // Replay
            facade.ReplayLast(2);

            // Change mode
            facade.SetMode(new EcoMode());

            facade.RunCommand(new TurnOnLampCommand(lamp), "ALL_ON"); // block
            facade.ExecuteAll();

            // Routine
            facade.MorningRoutine(lamp, thermostat);
        }
    }
}



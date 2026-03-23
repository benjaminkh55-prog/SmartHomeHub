using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHomeHub
{class Program
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



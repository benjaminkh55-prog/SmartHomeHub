using SmartHomeHub;

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
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

internal class ConsoleInput : IInput
{
    public Log Log { get; }

    public ConsoleInput(Log log)
    {
        Log = log;
    }

    public string Get()
    {
        string input = Console.ReadLine()!;
        Log.WriteToLog(LogType.Prod, $"Input received: {input}");
        Console.Clear();
        return input;
    }

    public int GetActionIndex()
    {
        string input = Get();
        int.TryParse(input, out int result);
        return result;
    }
}
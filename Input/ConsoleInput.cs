internal class ConsoleInput : IInput
{
    public Log ProdLog { get; }

    public ConsoleInput(Log prodLog)
    {
        ProdLog = prodLog;
    }

    public string Get()
    {
        string input = Console.ReadLine()!;
        ProdLog.WriteInProductionLog($"Input received: {input}");
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
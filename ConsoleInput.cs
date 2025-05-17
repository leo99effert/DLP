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
        return input;
    }
}
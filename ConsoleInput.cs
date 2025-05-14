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
        ProdLog.Write($"Input received: {input}");
        return input;
    }
}
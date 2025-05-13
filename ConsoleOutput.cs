class ConsoleOutput : IOutput
{
    public void Send(string text)
    {
        Console.WriteLine(text);
    }
}
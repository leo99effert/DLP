class ConsoleInput : IInput
{
    public string Get()
    {
        return Console.ReadLine()!;
    }
}
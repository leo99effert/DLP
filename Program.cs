bool isShutdownInitiated = false;
IInput input = new ConsoleInput();
IOutput output = new ConsoleOutput();

while (!isShutdownInitiated)
{
    output.Send("DLP is running...");
    output.Send("Would you like to exit the application?(y/n)");
    if (input.Get().ToLower()[0] == 'y')
    {
        isShutdownInitiated = true;
        output.Send("Shutting down DLP...");
    }
}
Console.ReadKey();

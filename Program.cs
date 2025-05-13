bool isShutdownInitiated = false;
while (!isShutdownInitiated)
{
    Console.WriteLine("DLP is running...");
    Console.WriteLine("Would you like to exit the application?(y/n)");
    if (Console.ReadLine()!.ToLower()[0] == 'y')
    {
        isShutdownInitiated = true;
        Console.WriteLine("Shutting down DLP...");
    }
}

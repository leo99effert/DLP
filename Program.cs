IInteraction interaction = new ConsoleInteraction();
Application application = new Application(
    interaction,
    new Session(),
    new List<IData>
    {
        new Countries("https://restcountries.com/v3.1/all?fields=name,capital")
    }
);

await application.Run();
Console.ReadKey();
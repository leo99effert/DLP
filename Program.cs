Log log = new Log();
Session session = new Session(log);

List<IData> data = new List<IData>
{
    new Countries(log, "https://restcountries.com/v3.1/all?fields=name")
};

IInteraction interaction = new ConsoleInteraction(log, data);


Application application = new Application(interaction, session);
await application.Run();
Console.ReadKey();
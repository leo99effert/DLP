IInteraction interaction = new ConsoleInteraction();
Session session = new Session();
Log log = new Log();
List<IData> data = new List<IData>
{
    new Countries("https://restcountries.com/v3.1/all?fields=name")
};

Application application = new Application(interaction, session, log, data);
await application.Run();
Console.ReadKey();
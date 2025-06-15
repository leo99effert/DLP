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

// TODO: Remove camelCase from menu actions
// TODO: Login failed if empty username or with whitespace
// TODO: Login using ConsoleInteraction not Application
// TODO: Put methods in right order
// TODO: Display asyncronously
// TODO: Move data fetching from ConsoleInteraction to Application
Log log = new Log();
Session session = new Session(log);
IInteraction interaction = new ConsoleInteraction(log);

Application application = new Application(interaction, session);
application.Run();
Console.ReadKey();

// TODO: Ability to pick which log to see
// TODO: Imlement countries view

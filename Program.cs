Log log = new Log();
Session session = new Session(log);
IInteraction interaction = new ConsoleInteraction(log);

Application application = new Application(interaction, session);
application.Run();
Console.ReadKey();

// TODO: Imlement countries view
// TODO: Remove camelCase from menu actions
// TODO: Login failed if empty username or with whitespace
// TODO: Login using ConsoleInteraction not Application
// TODO: Put methods in right order
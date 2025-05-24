Log log = new Log();
Session session = new Session(log);
IInput input = new ConsoleInput(log);
IOutput output = new ConsoleOutput(log, session);

Application application = new Application(input, output, session);
application.Run();
Console.ReadKey();
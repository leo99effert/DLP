// TODO: change public classes to internal classes

Log log = new Log();
log.Write("DLP started");
IInput input = new ConsoleInput(log);
IOutput output = new ConsoleOutput(log);
Session Session = new Session(log);

Application application = new Application(input, output, Session);
application.Run();

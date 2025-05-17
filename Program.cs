Log log = new Log();
log.WriteInProductionLog("DLP started");
IInput input = new ConsoleInput(log);
IOutput output = new ConsoleOutput(log);
Session Session = new Session(log);

Application application = new Application(input, output, Session);
application.Run();


// TODO new log file if its to long
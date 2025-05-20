internal class ConsoleOutput : IOutput
{
    public Log Log { get; }
    public ConsoleOutput(Log log)
    {
        Log = log;
    }

    private void Display(string text)
    {
        Console.WriteLine(text + Environment.NewLine);
    }
    public void DisplayStart()
    {
        Log.WriteToLog(LogType.Prod, "DLP started");
        Display("Welcome to DPL!");
    }

    public void DisplayExit()
    {
        Log.WriteToLog(LogType.Prod, "DLP exited");
        Display("Exiting TPL...");
        Console.ReadKey();
    }

    public void DisplayInvalidAction()
    {
        Log.WriteToLog(LogType.Prod, "Invalid action");
        Display("Invalid action. Please try again.");
    }

    public void DisplayNotImplemented()
    {
        Log.WriteToLog(LogType.Prod, "Not implemented");
        Display("Not implemented...");
    }
    public void DisplayMenu(Session session)
    {
        string menu = "";
        foreach (Action action in Enum.GetValues(typeof(Action)))
        {
            menu += $"{(int)action} - {action.ToString()}" + Environment.NewLine;
        }
        Display(menu);
        Log.WriteToLog(LogType.Prod, "Menu is displayed");
    }

    public void DisplaySession(Session session)
    {
        string text = $"Session start: {session.SessionStartTime}" + Environment.NewLine;
        if (session.IsLoggedIn)
        {
            text += $"Username: {session.User?.Username}" + Environment.NewLine +
                    $"User ID: {session.User?.Id}" + Environment.NewLine +
                    $"Login time: {session.LoginTime}" + Environment.NewLine;
        }
        Display(text);
        Log.WriteToLog(LogType.Prod, "Session is displayed");
    }

    public void DisplayProdLog(int lines)
    {
        string text = Log.ReadLogAsString(LogType.Prod, lines);
        Display(text);
        Log.WriteToLog(LogType.Prod, "Production log is displayed");
    }

    public void DisplayLoginPrompt()
    {
        Display("Enter username:");
    }

    public void DisplayLoggedOut()
    {
        Display("Logged out.");
    }

    public void DisplayAlreadyLoggedIn(User user)
    {
        Display($"{user.Username} already logged in.");
    }

    public void DisplayNotLoggedIn()
    {
        Display("Not logged in.");
    }
}
internal class ConsoleOutput : IOutput
{
    public Log ProdLog { get; }
    public ConsoleOutput(Log prodLog)
    {
        ProdLog = prodLog;
    }

    private void Display(string text)
    {
        Console.WriteLine(text + Environment.NewLine);
    }
    public void DisplayStart()
    {
        ProdLog.WriteInProductionLog("DLP started");
        Display("Welcome to DPL!");
    }

    public void DisplayExit()
    {
        ProdLog.WriteInProductionLog("DLP exited");
        Display("Exiting TPL...");
        Console.ReadKey();
    }

    public void DisplayInvalidAction()
    {
        ProdLog.WriteInProductionLog("Invalid action");
        Display("Invalid action. Please try again.");
    }

    public void DisplayNotImplemented()
    {
        ProdLog.WriteInProductionLog("Not implemented");
        Display("Not implemented...");
    }
    public void DisplayMenu(Session session)
    {
        string text = "Enter action:" + Environment.NewLine +
                      "View session (1)" + Environment.NewLine +
                      "Exit TPL (2)" + Environment.NewLine +
                      "Read prod.log (3)" + Environment.NewLine +
                      "Read countries (4)" + Environment.NewLine;

        if (session.IsLoggedIn)
        {
            text += "Logout (5)" + Environment.NewLine;
        }
        else
        {
            text += "Login (5)" + Environment.NewLine;
        }
        Display(text);
        ProdLog.WriteInProductionLog("Menu is displayed");
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
        ProdLog.WriteInProductionLog("Session is displayed");
    }

    public void DisplayProdLog(int lines)
    {
        string text = ProdLog.ReadFromProductionLogAsString(lines);
        Display(text);
        ProdLog.WriteInProductionLog("Production log is displayed");
    }

    public void DisplayLoginPrompt()
    {
        Display("Enter username:");
    }

    public void DisplayLoggedOut()
    {
        Display("Logged out.");
    }
}
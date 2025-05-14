internal class ConsoleOutput : IOutput
{
    public Log ProdLog { get; }
    public ConsoleOutput(Log prodLog)
    {
        ProdLog = prodLog;
    }

    public void Display(string text)
    {
        Console.WriteLine(text + Environment.NewLine);
    }

    public void DisplayMenu(Session session)
    {
        string text = "Enter action:" + Environment.NewLine +
                      "View session (1)" + Environment.NewLine +
                      "Exit TPL (2)" + Environment.NewLine +
                      "Read prod.log (3)" + Environment.NewLine;

        if (session.IsLoggedIn)
        {
            text += "Logout (4)" + Environment.NewLine;
        }
        else
        {
            text += "Login (4)" + Environment.NewLine;
        }
        Display(text);
        ProdLog.Write("Menu is displayed");
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
        ProdLog.Write("Session is displayed");
    }
}
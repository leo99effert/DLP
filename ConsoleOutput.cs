class ConsoleOutput : IOutput
{
    public void Display(string text)
    {
        Console.WriteLine(text + Environment.NewLine);
    }

    public void DisplayMenu(Session session)
    {
        string text = "Enter action:" + Environment.NewLine +
                      "View session (1)" + Environment.NewLine +
                      "Exit TPL (2)" + Environment.NewLine;
        if (session.IsLoggedIn)
        {
            text += "Logout (3)" + Environment.NewLine;
        }
        else
        {
            text += "Login (3)" + Environment.NewLine;
        }
        Display(text);
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
    }
}
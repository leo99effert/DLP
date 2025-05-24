internal class ConsoleOutput : IOutput
{
    public Log Log { get; }
    public int CurrentMenuOption { get; private set; } = 0;
    public Session Session { get; private set; }
    private int _actionDisplayLength = Enum.GetNames(typeof(Action)).Max(action => action.Length) + 4;
    private ConsoleColor _defaultTextColor = Console.ForegroundColor;
    private ConsoleColor _highlightedTextColor = ConsoleColor.Blue;
    private const int ViewWidth = 80;
    private const int ViewHeight = 12;

    public ConsoleOutput(Log log, Session session)
    {
        Log = log;
        Session = session;
    }
    public void DisplayView(ViewState viewState, Session session)
    {
        Session = session;
        Console.Clear();
        Console.WriteLine($"┌{new string('─', ViewWidth)}┐");
        List<string> lines = GetLinesToDisplay(viewState);
        for (int i = 0; i < ViewHeight; i++)
        {
            if (i < lines.Count)
            {
                string line = lines[i].PadRight(ViewWidth);
                Console.WriteLine($"│{line}│");
            }
            else
            {
                Console.WriteLine($"│{new string(' ', ViewWidth)}│");
            }
        }
        Console.WriteLine($"└{new string('─', ViewWidth)}┘");
    }

    private List<string> GetLinesToDisplay(ViewState viewState) => viewState switch
    {
        ViewState.Welcome => WelcomeText(),
        ViewState.Exit => ExitText(),
        ViewState.Session => SessionText(),
        ViewState.ProdLog => ProdLogText(10),
        ViewState.Countries => CountriesText(),
        ViewState.AlreadyLoggedIn => AlreadyLoggedInText(),
        ViewState.NotLoggedIn => NotLoggedInText(),
        ViewState.LoggingIn => new List<string> { "Logging in..." },
        ViewState.LoggingOut => new List<string> { "Logging out..." },
        ViewState.Invalid => new List<string> { "Invalid action." },
        _ => new List<string> { "Unknown view state." }
    };

    private void Display(string text)
    {
        Console.Write(text);
    }
    private List<string> WelcomeText() => new List<string> { "Welcome to DLP!" };
    private List<string> ExitText() => new List<string> { "Exiting DLP..." };
    private List<string> AlreadyLoggedInText() => new List<string> { $"{Session.User!.Username} already logged in." };
    private List<string> NotLoggedInText() => new List<string> { "Not logged in." };
    private List<string> CountriesText() => new List<string> { "Not implemented yet" };
    private List<string> ProdLogText(int lines) => Log.ReadLog(LogType.Prod, lines);

    private List<string> SessionText()
    {
        return new List<string>
        {
            $"Session start: {Session.SessionStartTime}",
            Session.IsLoggedIn ? $"Username: {Session.User?.Username}" : "Not logged in",
            Session.IsLoggedIn ? $"User ID: {Session.User?.Id}" : "",
            Session.IsLoggedIn ? $"Login time: {Session.LoginTime}" : ""
        };
    }

    public void DisplayInvalidAction()
    {
        Log.WriteToLog(LogType.Prod, "Invalid action");
        Display("Invalid action. Please try again." + Environment.NewLine);
    }

    public void DisplayNotImplemented()
    {
        Log.WriteToLog(LogType.Prod, "Not implemented");
        Display("Not implemented..." + Environment.NewLine);
    }
    public void DisplayMenu(Session session)
    {
        foreach (Action action in Enum.GetValues(typeof(Action)))
        {
            if (((int)action) == CurrentMenuOption)
            {
                Log.WriteToLog(LogType.Debug, $"{(int)action}");
                ApplyHighlightTextColor();
            }
            string displayAction = "";
            displayAction += "┌" + new string('─', _actionDisplayLength) + "┐" + Environment.NewLine;
            string displayText = ($"{(int)action} - {action.ToString()}").PadRight(_actionDisplayLength);
            displayAction += $"│{displayText}│" + Environment.NewLine;
            displayAction += "└" + new string('─', _actionDisplayLength) + "┘" + Environment.NewLine;
            Display(displayAction);
            ApplyDefaultTextColor();
        }
        Log.WriteToLog(LogType.Prod, "Menu is displayed");
    }
    private void ApplyDefaultTextColor()
    {
        Console.ForegroundColor = _defaultTextColor;
    }

    private void ApplyHighlightTextColor()
    {
        Console.ForegroundColor = _highlightedTextColor;
    }

    public void DisplayLoginPrompt()
    {
        Display("Enter username:" + Environment.NewLine);
    }

    public void DisplayLoggedOut()
    {
        Display("Logged out." + Environment.NewLine);
    }

    public void DisplayAlreadyLoggedIn(User user)
    {
        Display($"{user.Username} already logged in." + Environment.NewLine);
    }

    public void DisplayNotLoggedIn()
    {
        Display("Not logged in." + Environment.NewLine);
    }
}
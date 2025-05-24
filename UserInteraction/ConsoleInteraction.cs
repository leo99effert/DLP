internal class ConsoleInteraction : IInteraction
{
    public Log Log { get; }
    public int CurrentMenuOption { get; private set; } = 0;
    public Session Session { get; private set; }
    private int _actionDisplayLength = Enum.GetNames(typeof(Action)).Max(action => action.Length);
    private ConsoleColor _defaultTextColor = Console.ForegroundColor;
    private ConsoleColor _highlightedTextColor = ConsoleColor.Blue;
    private int ViewHeight = 12;
    private int ViewWidth = (Enum.GetNames(typeof(Action)).Max(action => action.Length) + 2) * Enum.GetNames(typeof(Action)).Length - 2;

    public ConsoleInteraction(Log log, Session session)
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
        ViewState.LoggingIn => LoggingInText(),
        ViewState.LoggingOut => LoggingOutText(),
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
    private List<string> LoggingInText() => new List<string> { "Logging in..." };
    private List<string> LoggingOutText() => new List<string> { "Loggin out..." };
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

    public void DisplayNotImplemented()
    {
        Log.WriteToLog(LogType.Prod, "Not implemented");
        Display("Not implemented..." + Environment.NewLine);
    }
    private void DisplayMenu(Session session)
    {
        DisplayMenuTop();
        Display(Environment.NewLine);
        DisplayMenuMiddle(session);
        Display(Environment.NewLine);
        DisplayMenuBottom();
    }
    private void DisplayMenuTop()
    {
        foreach (Action action in Enum.GetValues(typeof(Action)))
        {
            if (((int)action) == CurrentMenuOption)
            {
                ApplyHighlightTextColor();
            }
            string text = "┌" + new string('─', _actionDisplayLength) + "┐";
            Display(text);
            ApplyDefaultTextColor();
        }
    }
    private void DisplayMenuMiddle(Session session)
    {
        foreach (Action action in Enum.GetValues(typeof(Action)))
        {
            if (((int)action) == CurrentMenuOption)
            {
                ApplyHighlightTextColor();
            }
            string displayAction = action.ToString().PadRight(_actionDisplayLength);
            string text = $"│{displayAction}│";
            Display(text);
            ApplyDefaultTextColor();
        }
    }
    private void DisplayMenuBottom()
    {
        foreach (Action action in Enum.GetValues(typeof(Action)))
        {
            if (((int)action) == CurrentMenuOption)
            {
                ApplyHighlightTextColor();
            }
            string text = "└" + new string('─', _actionDisplayLength) + "┘";
            Display(text);
            ApplyDefaultTextColor();
        }
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
        Display(Environment.NewLine + "Enter username:" + Environment.NewLine);
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
    public string Get()
    {
        string input = Console.ReadLine()!;
        Log.WriteToLog(LogType.Prod, $"Input received: {input}");
        Console.Clear();
        return input;
    }

    public int GetActionIndex(Session session)
    {
        while (true)
        {
            Console.SetCursorPosition(0, ViewHeight + 2);
            DisplayMenu(session);
            ConsoleNavigateAction navigateAction = GetConsoleNavigateAction();
            if (navigateAction == ConsoleNavigateAction.PickOption)
            {
                return CurrentMenuOption;
            }
            else if (navigateAction == ConsoleNavigateAction.Left && (CurrentMenuOption > 0))
            {
                CurrentMenuOption--;
            }
            else if (navigateAction == ConsoleNavigateAction.Left && (CurrentMenuOption > 0))
            {
                CurrentMenuOption--;
            }
            else if (navigateAction == ConsoleNavigateAction.Right && (CurrentMenuOption < Enum.GetNames(typeof(Action)).Length - 1))
            {
                CurrentMenuOption++;
            }
        }
    }
    public ConsoleNavigateAction GetConsoleNavigateAction()
    {
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                return ConsoleNavigateAction.Left;
            }
            if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                return ConsoleNavigateAction.Right;
            }
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                return ConsoleNavigateAction.PickOption;
            }
        }
    }
}
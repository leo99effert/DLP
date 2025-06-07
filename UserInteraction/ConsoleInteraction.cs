internal class ConsoleInteraction : IInteraction
{
    public Log Log { get; }
    public int CurrentMenuOption { get; private set; }
    private int _actionDisplayLength = Enum.GetNames(typeof(Action)).Max(action => action.Length);
    private ConsoleColor _defaultTextColor = Console.ForegroundColor;
    private ConsoleColor _highlightedTextColor = ConsoleColor.Blue;
    private int ViewHeight = 12;
    private int ViewWidth = (Enum.GetNames(typeof(Action)).Max(action => action.Length) + 2) * Enum.GetNames(typeof(Action)).Length - 2;

    public ConsoleInteraction(Log log)
    {
        Log = log;
    }
    public void DisplayView(ViewState viewState, Session session)
    {
        Console.Clear();
        Console.WriteLine($"┌{new string('─', ViewWidth)}┐");
        List<string> lines = GetLinesToDisplay(viewState, session);
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

    private List<string> GetLinesToDisplay(ViewState viewState, Session session) => viewState switch
    {
        ViewState.Welcome => WelcomeText(),
        ViewState.Countries => CountriesText(),
        ViewState.Session => SessionText(session),
        ViewState.ProdLog => LogText(LogType.Prod, 10),
        ViewState.DebugLog => LogText(LogType.Debug, 10),
        ViewState.ErrorLog => LogText(LogType.Error, 10),
        ViewState.LoggingIn => LoggingInText(),
        ViewState.AlreadyLoggedIn => AlreadyLoggedInText(session),
        ViewState.LoggingOut => LoggingOutText(),
        ViewState.NotLoggedIn => NotLoggedInText(),
        ViewState.Exit => ExitText(),
        _ => new List<string> { "Unknown view state." }
    };

    private void Display(string text)
    {
        Console.Write(text);
    }
    private List<string> WelcomeText() => new List<string> { "Welcome to DLP!" };
    private List<string> CountriesText() => new List<string> { "Not implemented yet" };
    private List<string> SessionText(Session session)
    {
        return new List<string>
        {
            $"Session start: {session.SessionStartTime}",
            session.IsLoggedIn ? $"Username: {session.User?.Username}" : "Not logged in",
            session.IsLoggedIn ? $"User ID: {session.User?.Id}" : "",
            session.IsLoggedIn ? $"Login time: {session.LoginTime}" : ""
        };
    }
    private List<string> LogText(LogType logType, int lines) => Log.ReadLog(logType, lines);
    private List<string> LoggingInText() => new List<string> { "Logging in..." };
    private List<string> AlreadyLoggedInText(Session session) => new List<string> { $"{session.User!.Username} already logged in." };
    private List<string> LoggingOutText() => new List<string> { "Loggin out..." };
    private List<string> NotLoggedInText() => new List<string> { "Not logged in." };
    private List<string> ExitText() => new List<string> { "Exiting DLP..." };

    private void DisplayMenu<T>() where T : Enum
    {
        HideMenu();
        DisplayMenuTop<T>();
        Display(Environment.NewLine);
        DisplayMenuMiddle<T>();
        Display(Environment.NewLine);
        DisplayMenuBottom<T>();
    }

    private void DisplayMenuTop<T>() where T : Enum
    {
        Console.SetCursorPosition(0, ViewHeight + 2);
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            if (Convert.ToInt32(value) == CurrentMenuOption)
            {
                ApplyHighlightTextColor();
            }
            string text = "┌" + new string('─', _actionDisplayLength) + "┐";
            Display(text);
            ApplyDefaultTextColor();
        }
    }
    private void DisplayMenuMiddle<T>() where T : Enum
    {
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            if (Convert.ToInt32(value) == CurrentMenuOption)
            {
                ApplyHighlightTextColor();
            }
            string displayAction = value.ToString().PadRight(_actionDisplayLength);
            string text = $"│{displayAction}│";
            Display(text);
            ApplyDefaultTextColor();
        }
    }
    private void DisplayMenuBottom<T>() where T : Enum
    {
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            if (Convert.ToInt32(value) == CurrentMenuOption)
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

    public void PromptLogin()
    {
        ClearViewWindow();
        HideMenu();
        Console.SetCursorPosition(1, 1);
        Display("Enter username: ");
    }

    private void ClearViewWindow()
    {
        int menuLeftPosition = 1;
        int menuTopPosition = 1;
        int menuHeight = ViewHeight;
        int menuWidth = ViewWidth;
        ClearSection(menuLeftPosition, menuTopPosition, menuHeight, menuWidth);
    }

    private void HideMenu()
    {
        int menuLeftPosition = 0;
        int menuTopPosition = ViewHeight + 2;
        int menuHeight = 3;
        int menuWidth = ViewWidth + 2;
        ClearSection(menuLeftPosition, menuTopPosition, menuHeight, menuWidth);
    }

    private void ClearSection(int left, int top, int height, int width)
    {
        Console.SetCursorPosition(left, top);
        for (int i = 0; i < height; i++)
        {
            Console.SetCursorPosition(left, top + i);
            for (int j = 0; j < width; j++)
            {
                Display(" ");
            }
        }
    }

    public string ReadUsername()
    {
        string input = Console.ReadLine()!;
        Console.Clear();
        return input;
    }

    public T GetInput<T>() where T : Enum
    {
        while (true)
        {
            Console.SetCursorPosition(0, ViewHeight + 2);
            DisplayMenu<T>();
            ConsoleNavigateAction navigateAction = GetConsoleNavigateAction();
            if (navigateAction == ConsoleNavigateAction.PickOption)
            {
                return (T)Enum.ToObject(typeof(T), CurrentMenuOption);
            }
            else if (navigateAction == ConsoleNavigateAction.Left && (CurrentMenuOption > 0))
            {
                CurrentMenuOption--;
            }
            else if (navigateAction == ConsoleNavigateAction.Left && (CurrentMenuOption > 0))
            {
                CurrentMenuOption--;
            }
            else if (navigateAction == ConsoleNavigateAction.Right && (CurrentMenuOption < Enum.GetNames(typeof(T)).Length - 1))
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
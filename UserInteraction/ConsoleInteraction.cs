internal class ConsoleInteraction : IInteraction
{
    public Log Log { get; }
    public List<IData> Data { get; set; }
    public int CurrentMenuOption { get; private set; }
    private readonly ConsoleColor _defaultTextColor = Console.ForegroundColor;
    private readonly ConsoleColor _highlightedTextColor = ConsoleColor.Blue;
    private readonly List<Type> _menuTypes = new List<Type> { typeof(Action), typeof(LogType) };
    private readonly int _viewHeight = 12;
    private int ViewWidth => GetViewWidth();
    private int MenuItemDisplayLength => GetMenuItemDisplayLength();

    public ConsoleInteraction(Log log, List<IData> data)
    {
        Log = log;
        Data = data;
    }

    private int GetViewWidth()
    {
        List<int> menuLengths = new List<int>();
        foreach (Type type in _menuTypes)
        {
            menuLengths.Add((Enum.GetNames(type).Max(item => item.Length) + 2) * Enum.GetNames(type).Length - 2);
        }
        return menuLengths.Max();
    }

    private int GetMenuItemDisplayLength()
    {
        List<int> menuItemLengths = new List<int>();
        foreach (Type type in _menuTypes)
        {
            menuItemLengths.Add(Enum.GetNames(type).Max(item => item.Length));
        }
        return menuItemLengths.Max();
    }
    public async Task DisplayView(ViewState viewState, Session session)
    {
        Console.Clear();
        Display($"┌{new string('─', ViewWidth)}┐" + Environment.NewLine);
        List<string> lines = await GetLinesToDisplay(viewState, session);
        for (int i = 0; i < _viewHeight; i++)
        {
            if (i < lines.Count)
            {
                string line = lines[i].PadRight(ViewWidth);
                Display($"│{line}│" + Environment.NewLine);
            }
            else
            {
                Display($"│{new string(' ', ViewWidth)}│" + Environment.NewLine);
            }
        }
        Display($"└{new string('─', ViewWidth)}┘" + Environment.NewLine);
    }

    private async Task<List<string>> GetLinesToDisplay(ViewState viewState, Session session) => viewState switch
    {
        ViewState.Welcome => WelcomeText(),
        ViewState.Countries => await CountriesText(),
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
    private async Task<List<string>> CountriesText()
    {
        Countries model = Data.OfType<Countries>().FirstOrDefault();
        List<Country> countries = await model.Get();
        return countries.Select(country => country.Name).ToList();
    }
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
        Console.SetCursorPosition(0, _viewHeight + 2);
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            if (Convert.ToInt32(value) == CurrentMenuOption)
            {
                ApplyHighlightTextColor();
            }
            string text = "┌" + new string('─', MenuItemDisplayLength) + "┐";
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
            string displayAction = value.ToString().PadRight(MenuItemDisplayLength);
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
            string text = "└" + new string('─', MenuItemDisplayLength) + "┘";
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
        int menuHeight = _viewHeight;
        int menuWidth = ViewWidth;
        ClearSection(menuLeftPosition, menuTopPosition, menuHeight, menuWidth);
    }

    private void HideMenu()
    {
        int menuLeftPosition = 0;
        int menuTopPosition = _viewHeight + 2;
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
        CurrentMenuOption = 0;
        while (true)
        {
            Console.SetCursorPosition(0, _viewHeight + 2);
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
internal class ConsoleInteraction : IInteraction
{
    public int CurrentMenuOption { get; private set; }
    private int MenuItemDisplayLength => GetMenuItemDisplayLength();
    private int ViewWidth => GetViewWidth();
    private readonly int _viewHeight = 12;
    private readonly List<Type> _menuTypes = new List<Type> { typeof(Action), typeof(LogType) };
    private readonly ConsoleColor _defaultTextColor = Console.ForegroundColor;
    private readonly ConsoleColor _highlightedTextColor = ConsoleColor.Blue;


    private void Display(string text)
    {
        Console.Write(text);
    }


    // --- Get widths below ---
    private int GetMenuItemDisplayLength()
    {
        List<int> menuItemLengths = new List<int>();
        foreach (Type type in _menuTypes)
        {
            menuItemLengths.Add(Enum.GetNames(type).Max(item => item.Length));
        }
        return menuItemLengths.Max();
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
    // --- Get widths above ---

    // --- None-standard interaction below ---
    public void PromptLogin()
    {
        ClearViewWindow();
        HideMenu();
        Console.SetCursorPosition(1, 1);
        Display("Enter username: ");
    }
    public string ReadUsername()
    {
        string input = Console.ReadLine()!;
        Console.Clear();
        return input;
    }
    // --- None-standard interaction above ---

    // --- Interact with menu below ---
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
    private ConsoleNavigateAction GetConsoleNavigateAction()
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
    // --- Interact with menu above ---

    // --- Print menu below ---
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
    // --- Print menu above ---

    // --- Print view below ---
    public void DisplayView(List<string> lines)
    {
        Console.Clear();
        Display($"┌{new string('─', ViewWidth)}┐" + Environment.NewLine);
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
    // --- Print view above --- 

    // --- Clear section below ---
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
    // --- Clear section above ---
}
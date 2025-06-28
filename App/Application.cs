internal class Application : Log
{
    public bool IsShutdownInitiated { get; private set; }
    public IInteraction Interaction { get; }
    public Session Session { get; }
    public List<IData> Data { get; set; }
    public Application(IInteraction interaction, Session session, List<IData> data)
    {
        Interaction = interaction;
        Session = session;
        Data = data;
    }

    // --- Main loop below ---
    public async Task Run()
    {
        Interaction.DisplayView(new List<string> { "Welcome to DLP!" });
        while (!IsShutdownInitiated)
        {
            Action action = MenuSelection<Action>();
            await PerformAction(action);
        }
    }
    private T MenuSelection<T>() where T : Enum
    {
        T value = Interaction.GetInput<T>();
        if (!Enum.IsDefined(typeof(T), value))
        {
            WriteToLog(LogType.Error, $"Invalid option selected: {value}");
            throw new ArgumentOutOfRangeException(nameof(value), value, $"Option {value} was not found");
        }
        return value;
    }

    private async Task PerformAction(Action action)
    {
        switch (action)
        {
            case Action.ReadCountries:
                Countries model = Data.OfType<Countries>().FirstOrDefault()!;
                List<Country> countries = await model.Get();
                List<string> lines = countries.Select(country => country.Name).ToList();
                Interaction.DisplayView(lines);
                break;
            case Action.ViewSession:
                List<string> logLines = new List<string>
                {
                    $"Session start: {Session.SessionStartTime}",
                    Session.IsLoggedIn ? $"Username: {Session.User?.Username}" : "Not logged in",
                    Session.IsLoggedIn ? $"User ID: {Session.User?.Id}" : "",
                    Session.IsLoggedIn ? $"Login time: {Session.LoginTime}" : ""
                };
                Interaction.DisplayView(logLines);
                break;
            case Action.ReadLog:
                ViewLog();
                break;
            case Action.Login:
                Login();
                break;
            case Action.Logout:
                Logout();
                break;
            case Action.Exit:
                Interaction.DisplayView(new List<string> { "Exiting DLP..." });
                IsShutdownInitiated = true;
                break;
            default:
                WriteToLog(LogType.Error, $"Invalid action selected: {action}");
                throw new ArgumentOutOfRangeException(nameof(action), action, $"Action {action} was not found");
        }
        string log = $"Action performed: {action}, by " + (Session.IsLoggedIn ? Session.User!.Username : "guest");
        WriteToLog(LogType.Prod, log);
    }
    // --- Main loop above ---

    // --- Action methods below ---
    private void ViewLog()
    {
        LogType logType = MenuSelection<LogType>();
        List<string> lines = ReadLog(logType, 10);
        Interaction.DisplayView(lines);
        string log = $"Action performed: read {logType}-log, by " + (Session.IsLoggedIn ? Session.User!.Username : "guest");
        WriteToLog(LogType.Prod, log);
    }
    private void Login()
    {
        if (Session.IsLoggedIn)
        {
            Interaction.DisplayView(new List<string> { $"{Session!.User!.Username} already logged in." });
            return;
        }
        Interaction.PromptLogin();
        string username = Interaction.ReadUsername();
        Interaction.DisplayView(new List<string> { "Logging in..." });
        Session.Login(new User(username));
    }
    private void Logout()
    {
        if (!Session.IsLoggedIn)
        {
            Interaction.DisplayView(new List<string> { "Not logged in." });
            return;
        }
        Interaction.DisplayView(new List<string> { "Loggin out..." });
        Session.Logout();
    }
    // --- Action methods above ---
}
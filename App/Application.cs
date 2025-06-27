internal class Application
{
    public bool IsShutdownInitiated { get; private set; }
    public IInteraction Interaction { get; }
    public Session Session { get; }
    public Log Log { get; }
    public List<IData> Data { get; set; }
    public Application(IInteraction interaction, Session session, Log log, List<IData> data)
    {
        Interaction = interaction;
        Session = session;
        Log = log;
        Data = data;
    }

    public async Task Run()
    {
        Interaction.Home();
        while (!IsShutdownInitiated)
        {
            Action action = MenuSelection<Action>();
            await PerformAction(action);
        }
    }

    private async Task PerformAction(Action action)
    {
        switch (action)
        {
            case Action.ReadCountries:
                Countries model = Data.OfType<Countries>().FirstOrDefault()!;
                List<Country> countries = await model.Get();
                Interaction.Countries(countries);
                break;
            case Action.ViewSession:
                Interaction.Session(Session);
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
                Interaction.Exit();
                IsShutdownInitiated = true;
                break;
            default:
                Log.WriteToLog(LogType.Error, $"Invalid action selected: {action}");
                throw new ArgumentOutOfRangeException(nameof(action), action, $"Action {action} was not found");
        }
        string log = $"Action performed: {action}, by " + (Session.IsLoggedIn ? Session.User!.Username : "guest");
        Log.WriteToLog(LogType.Prod, log);
    }

    private void Login()
    {
        if (Session.IsLoggedIn)
        {
            Interaction.AlreadyLoggedIn(Session!.User!.Username);
            return;
        }
        Interaction.PromptLogin();
        string username = Interaction.ReadUsername();
        Interaction.LoggingIn();
        Session.Login(new User(username));
    }

    private void Logout()
    {
        if (!Session.IsLoggedIn)
        {
            Interaction.NotLoggedIn();
            return;
        }
        Interaction.LoggingOut();
        Session.Logout();
    }

    private T MenuSelection<T>() where T : Enum
    {
        T value = Interaction.GetInput<T>();
        if (!Enum.IsDefined(typeof(T), value))
        {
            Log.WriteToLog(LogType.Error, $"Invalid option selected: {value}");
            throw new ArgumentOutOfRangeException(nameof(value), value, $"Option {value} was not found");
        }
        return value;
    }

    private void ViewLog()
    {
        LogType logType = MenuSelection<LogType>();
        Interaction.ReadLog(Log, logType);
        string log = $"Action performed: read {logType}-log, by " + (Session.IsLoggedIn ? Session.User!.Username : "guest");
        Log.WriteToLog(LogType.Prod, log);
    }
}
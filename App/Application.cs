internal class Application
{
    public bool IsShutdownInitiated { get; private set; }
    public IInteraction Interaction { get; }
    public Session Session { get; }
    public Log Log { get; } = new Log();
    public ViewState ViewState { get; private set; } = ViewState.Welcome;
    public List<Action> ActionsThatRequireSubActions { get; set; } = new List<Action>
    {
        Action.ReadLog,
        Action.Login,
        Action.Logout
    };
    public Application(IInteraction interaction, Session session)
    {
        Interaction = interaction;
        Session = session;
    }

    public async Task Run()
    {
        await Interaction.DisplayView(ViewState, Session);
        while (!IsShutdownInitiated)
        {
            Action action = MenuSelection<Action>();
            PerformAction(action);
            await Interaction.DisplayView(ViewState, Session);
        }
    }

    private void PerformAction(Action action)
    {
        switch (action)
        {
            case Action.ReadCountries:
                ViewState = ViewState.Countries;
                break;
            case Action.ViewSession:
                ViewState = ViewState.Session;
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
                ViewState = ViewState.Exit;
                IsShutdownInitiated = true;
                break;
            default:
                Log.WriteToLog(LogType.Error, $"Invalid action selected: {action}");
                throw new ArgumentOutOfRangeException(nameof(action), action, $"Action {action} was not found");
        }
        if (!ActionsThatRequireSubActions.Contains(action))
        {
            string log = $"Action performed: {action}, by " + (Session.IsLoggedIn ? Session.User!.Username : "guest");
            Log.WriteToLog(LogType.Prod, log);
        }
    }

    private void Login()
    {
        if (Session.IsLoggedIn)
        {
            ViewState = ViewState.AlreadyLoggedIn;
            return;
        }
        ViewState = ViewState.LoggingIn;
        Interaction.PromptLogin();
        string username = Interaction.ReadUsername();
        Session.Login(new User(username));
    }

    private void Logout()
    {
        if (!Session.IsLoggedIn)
        {
            ViewState = ViewState.NotLoggedIn;
            return;
        }
        ViewState = ViewState.LoggingOut;
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
        switch (logType)
        {
            case LogType.Prod:
                ViewState = ViewState.ProdLog;
                break;
            case LogType.Debug:
                ViewState = ViewState.DebugLog;
                break;
            case LogType.Error:
                ViewState = ViewState.ErrorLog;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(logType), logType, $"Logtype {logType} was not found");
        }
        string log = $"Action performed: read {logType}-log, by " + (Session.IsLoggedIn ? Session.User!.Username : "guest");
        Log.WriteToLog(LogType.Prod, log);
    }
}
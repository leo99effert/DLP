internal class Application
{
    public bool IsShutdownInitiated { get; private set; }
    public IInteraction Interaction { get; }
    public Session Session { get; }
    public Log Log { get; } = new Log();
    public ViewState ViewState { get; private set; } = ViewState.Welcome;
    public List<Action> ActionsThatRequireSubActions { get; set; } = new List<Action>
    {
        //Action.ReadLog,
        Action.Login,
        Action.Logout
    };
    public Application(IInteraction interaction, Session session)
    {
        Interaction = interaction;
        Session = session;
    }

    public void Run()
    {
        Interaction.DisplayView(ViewState, Session);
        while (!IsShutdownInitiated)
        {
            Action action = MenuSelection();
            PerformAction(action);
            Interaction.DisplayView(ViewState, Session);
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
                ViewState = ViewState.ProdLog;
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

    private Action MenuSelection()
    {
        Action action = Interaction.GetAction(Session);
        if (!Enum.IsDefined(typeof(Action), action))
        {
            Log.WriteToLog(LogType.Error, $"Invalid action selected: {action}");
            throw new ArgumentOutOfRangeException(nameof(action), action, $"Action {action} was not found");
        }
        return action;
    }
}
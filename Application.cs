internal class Application
{
    public bool IsShutdownInitiated { get; private set; }
    public IInput Input { get; }
    public IOutput Output { get; }
    public Session Session { get; }
    public Log Log { get; } = new Log();
    public ViewState ViewState { get; private set; } = ViewState.Welcome;
    public Application(IInput input, IOutput output, Session session)
    {
        Input = input;
        Output = output;
        Session = session;
    }

    public void Run()
    {
        Output.DisplayView(ViewState, Session);
        while (!IsShutdownInitiated)
        {
            Action action = MenuSelection();
            PerformAction(action);
            Output.DisplayView(ViewState, Session);
        }
    }

    private void PerformAction(Action action)
    {
        switch (action)
        {
            case Action.Invalid:
                ViewState = ViewState.Invalid;
                break;
            case Action.ViewSession:
                ViewState = ViewState.Session;
                break;
            case Action.Exit:
                ViewState = ViewState.Exit;
                IsShutdownInitiated = true;
                break;
            case Action.ReadProdLog:
                ViewState = ViewState.ProdLog;
                break;
            case Action.ReadCountries:
                ViewState = ViewState.Countries;
                break;
            case Action.Login:
                Login();
                break;
            case Action.Logout:
                Logout();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, $"Action {action} was not found");
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
        Output.DisplayLoginPrompt();
        string username = Input.Get();
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
        Output.DisplayMenu(Session);
        Action action = (Action)Input.GetActionIndex();
        if (!Enum.IsDefined(typeof(Action), action))
        {
            action = Action.Invalid;
        }
        return action;
    }
}
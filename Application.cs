internal class Application
{
    public bool IsShutdownInitiated { get; private set; }
    public IInput Input { get; }
    public IOutput Output { get; }
    public Session Session { get; }
    public Log Log { get; } = new Log();
    public Application(IInput input, IOutput output, Session session)
    {
        Input = input;
        Output = output;
        Session = session;
    }
    public void Run()
    {
        OutputInfoOnApplicationStart();
        MainLoop();
    }

    private void MainLoop()
    {
        while (!IsShutdownInitiated)
        {
            Action action = MenuSelection();
            PerformAction(action);
        }
    }

    private void PerformAction(Action action)
    {
        switch (action)
        {
            case Action.Invalid:
                Output.DisplayInvalidAction();
                break;
            case Action.ViewSession:
                Output.DisplaySession(Session);
                break;
            case Action.Exit:
                ShutDown();
                break;
            case Action.ReadProdLog:
                Output.DisplayProdLog(10);
                break;
            case Action.ReadCountries:
                Output.DisplayNotImplemented();
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

    private void ShutDown()
    {
        Output.DisplayExit();
        IsShutdownInitiated = true;
    }

    private void Login()
    {
        if (Session.IsLoggedIn)
        {
            Output.DisplayAlreadyLoggedIn(Session.User!);
            return;
        }
        Output.DisplayLoginPrompt();
        string username = Input.Get();
        Session.Login(new User(username));
    }

    private void Logout()
    {
        if (!Session.IsLoggedIn)
        {
            Output.DisplayNotLoggedIn();
            return;
        }
        Session.Logout();
        Output.DisplayLoggedOut();
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

    private void OutputInfoOnApplicationStart()
    {
        Output.DisplayStart();
    }
}
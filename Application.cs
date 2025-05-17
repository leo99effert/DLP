// TODO: change public classes to internal classes

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
        OutputInfoOnApplicationExit();
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
                Output.Display("Invalid action. Please try again.");
                break;
            case Action.ViewSession:
                Output.DisplaySession(Session);
                break;
            case Action.Exit:
                ShutDown();
                break;
            case Action.ReadProdLog:
                Output.Display(Log.ReadFromProductionLogAsString(10));
                break;
            case Action.ReadCountries:
                Output.Display("Not implemented...");
                break;
            case Action.ChangeLoginState:
                ChangeLoginState();
                break;
            default:
                Output.Display("Invalid action. Please try again.");
                break;
        }
    }

    private void ChangeLoginState()
    {
        if (Session.IsLoggedIn)
        {
            Logout();
        }
        else
        {
            Login();
        }
    }

    private void ShutDown()
    {
        Output.Display("Exiting TPL...");
        IsShutdownInitiated = true;
    }

    private void Login()
    {
        Output.Display("Enter username:");
        string username = Input.Get();
        Session.Login(new User(username));
    }

    private void Logout()
    {
        Session.Logout();
        Output.Display("Logged out.");
    }

    private Action MenuSelection()
    {
        Output.DisplayMenu(Session);
        Action action = (Action)Input.GetActionIndex();
        return action;
    }

    private void OutputInfoOnApplicationExit()
    {
        Log.WriteInProductionLog("DLP ended");
        Output.Display("Press any key to exit...");
        Console.ReadKey();
    }

    private void OutputInfoOnApplicationStart()
    {
        Output.Display("DLP is running...");
        Output.DisplaySession(Session);
    }
}
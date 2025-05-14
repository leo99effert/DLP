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
            string action = MenuSelection();
            PerformAction(action);
        }
    }

    private void PerformAction(string action)
    {
        switch (action)
        {
            case "1":
                Output.DisplaySession(Session);
                break;
            case "2":
                ShutDown();
                break;
            case "3":
                Output.Display(Log.ReadJoined(10));
                break;
            case "4":
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

    private string MenuSelection()
    {
        Output.DisplayMenu(Session);
        string action = Input.Get();
        return action;
    }

    private void OutputInfoOnApplicationExit()
    {
        Log.Write("DLP ended");
        Output.Display("Press any key to exit...");
        Console.ReadKey();
    }

    private void OutputInfoOnApplicationStart()
    {
        Output.Display("DLP is running...");
        Output.DisplaySession(Session);
    }
}
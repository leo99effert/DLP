// TODO: change public classes to internal classes

class Application
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
        Output.Display("DLP is running...");
        Output.DisplaySession(Session);
        while (!IsShutdownInitiated)
        {
            Output.DisplayMenu(Session);
            string action = Input.Get();
            if (action == "1")
            {
                Output.DisplaySession(Session);
            }
            else if (action == "2")
            {
                Output.Display("Exiting TPL...");
                IsShutdownInitiated = true;
            }
            else if (action == "3")
            {
                string[] logs = Log.Read(10);
                Output.Display(string.Join(Environment.NewLine, logs));
            }
            else if (action == "4")
            {
                if (Session.IsLoggedIn)
                {
                    Session.Logout();
                    Output.Display("Logged out.");
                }
                else
                {
                    Output.Display("Enter username:");
                    string username = Input.Get();
                    Session.Login(new User(username));
                }
            }
            else
            {
                Output.Display("Invalid action. Please try again.");
            }
        }
        Log.Write("DLP ended");
        Output.Display("Press any key to exit...");
        Console.ReadKey();
    }
}
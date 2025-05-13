Log log = new Log();
log.Write("DLP started");
IInput input = new ConsoleInput(log);
IOutput output = new ConsoleOutput(log);
Session session = new Session(log);
bool isShutdownInitiated = false;

output.Display("DLP is running...");
output.DisplaySession(session);

while (!isShutdownInitiated)
{
    output.DisplayMenu(session);
    string action = input.Get();
    if (action == "1")
    {
        output.DisplaySession(session);
    }
    else if (action == "2")
    {
        output.Display("Exiting TPL...");
        isShutdownInitiated = true;
    }
    else if (action == "3")
    {
        string[] logs = log.Read(10);
        output.Display(string.Join(Environment.NewLine, logs));
    }
    else if (action == "4")
    {
        if (session.IsLoggedIn)
        {
            session.Logout();
            output.Display("Logged out.");
        }
        else
        {
            output.Display("Enter username:");
            string username = input.Get();
            session.Login(new User(username));
        }
    }
    else
    {
        output.Display("Invalid action. Please try again.");
    }
}
log.Write("DLP ended");
output.Display("Press any key to exit...");
Console.ReadKey();

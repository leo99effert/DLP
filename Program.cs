IInput input = new ConsoleInput();
IOutput output = new ConsoleOutput();
Session session = new Session();
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
Console.ReadKey();

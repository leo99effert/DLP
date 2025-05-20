internal class Session
{
    public bool IsLoggedIn { get; private set; }
    public User? User { get; set; }
    public DateTime SessionStartTime { get; } = DateTime.Now;
    public DateTime? LoginTime { get; private set; } = null;
    public Log Log { get; }
    public Session(Log log)
    {
        Log = log;
    }
    public void Login(User user)
    {
        IsLoggedIn = true;
        User = user;
        LoginTime = DateTime.Now;
        Log.WriteToLog(LogType.Prod, $"user logged in: {User.Username}");
    }
    public void Logout()
    {
        IsLoggedIn = false;
        LoginTime = null;
        Log.WriteToLog(LogType.Prod, $"user logged out: {User!.Username}");
        User = null;
    }
}
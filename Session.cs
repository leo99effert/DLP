internal class Session
{
    public bool IsLoggedIn { get; private set; }
    public User? User { get; set; }
    public DateTime SessionStartTime { get; } = DateTime.Now;
    public DateTime? LoginTime { get; private set; } = null;
    public Log ProdLog { get; }
    public Session(Log prodLog)
    {
        ProdLog = prodLog;
    }
    public void Login(User user)
    {
        IsLoggedIn = true;
        User = user;
        LoginTime = DateTime.Now;
        ProdLog.Write($"user logged in: {User.Username}");
    }
    public void Logout()
    {
        IsLoggedIn = false;
        LoginTime = null;
        ProdLog.Write($"user logged out: {User!.Username}");
        User = null;
    }
}
class Session
{
    public bool IsLoggedIn { get; private set; }
    public User? User { get; set; }
    public DateTime SessionStartTime { get; } = DateTime.Now;
    public DateTime? LoginTime { get; private set; } = null;
    public void Login(User user)
    {
        IsLoggedIn = true;
        User = user;
        LoginTime = DateTime.Now;
    }
    public void Logout()
    {
        IsLoggedIn = false;
        User = null;
        LoginTime = null;
    }
}
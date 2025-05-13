class User
{
    public string Username { get; }
    public string Id { get; }
    public User(string username)
    {
        Username = username;
        Id = Guid.NewGuid().ToString();
    }

    public User(string id, string username)
    {
        Username = username;
        Id = id;
    }
}

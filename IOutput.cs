interface IOutput
{
    public Log ProdLog { get; }
    void Display(string text);
    void DisplaySession(Session session);
    void DisplayMenu(Session session);
}
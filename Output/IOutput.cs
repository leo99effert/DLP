internal interface IOutput
{
    public Log ProdLog { get; }
    void DisplayStart();
    void DisplayExit();
    void DisplayInvalidAction();
    void DisplayNotImplemented();
    void DisplaySession(Session session);
    void DisplayMenu(Session session);
    void DisplayProdLog(int lines);
    void DisplayLoginPrompt();
    void DisplayLoggedOut();
    void DisplayAlreadyLoggedIn(User user);
    void DisplayNotLoggedIn();
}
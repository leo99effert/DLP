internal interface IOutput
{
    public Log Log { get; }
    void DisplayView(ViewState viewState, Session session);
    void DisplayMenu(Session session);
    void DisplayLoginPrompt();
    //string DisplayWelcome();
    //void DisplayExit();
    //void DisplayInvalidAction();
    //void DisplayNotImplemented();
    //void DisplaySession(Session session);
    //void DisplayProdLog(int lines);
    //void DisplayLoggedOut();
    //void DisplayAlreadyLoggedIn(User user);
    //void DisplayNotLoggedIn();
}
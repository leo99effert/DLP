internal interface IInteraction
{
    public Log Log { get; }
    string Get();
    int GetActionIndex(Session session);
    void DisplayView(ViewState viewState, Session session);
    void DisplayLoginPrompt();
}
internal interface IInteraction
{
    public Log Log { get; }
    void DisplayView(ViewState viewState, Session session);
    void PromptLogin();
    Action GetAction(Session session);
    string ReadUsername();
}
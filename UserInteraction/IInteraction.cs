internal interface IInteraction
{
    public Log Log { get; }
    void DisplayView(ViewState viewState, Session session);
    void PromptLogin();
    int GetActionIndex(Session session);
    string ReadUsername();
}
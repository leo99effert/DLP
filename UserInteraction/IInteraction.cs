internal interface IInteraction
{
    public Log Log { get; }
    void DisplayView(ViewState viewState, Session session);
    void PromptLogin();
    T GetInput<T>() where T : Enum;
    string ReadUsername();
}
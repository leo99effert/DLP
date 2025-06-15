internal interface IInteraction
{
    public Log Log { get; }
    Task DisplayView(ViewState viewState, Session session);
    void PromptLogin();
    T GetInput<T>() where T : Enum;
    string ReadUsername();
}
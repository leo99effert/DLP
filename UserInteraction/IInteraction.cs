internal interface IInteraction
{
    // --- Interact with menu below ---
    T GetInput<T>() where T : Enum;
    // --- Interact with menu above ---

    // --- None-standard interaction below ---
    void PromptLogin();
    string ReadUsername();
    // --- None-standard interaction above ---

    // --- Scenarios to display below ---
    void Home();
    void Countries(List<Country> countries);
    void Session(Session session);
    void ReadLog(Log log, LogType logType);
    void AlreadyLoggedIn(string username);
    void LoggingIn();
    void NotLoggedIn();
    void LoggingOut();
    void Exit();
    // --- Scenarios to display above ---
}
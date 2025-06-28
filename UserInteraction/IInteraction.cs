internal interface IInteraction
{
    // --- Interact with menu below ---
    T GetInput<T>() where T : Enum;
    // --- Interact with menu above ---

    // --- None-standard interaction below ---
    void PromptLogin();
    string ReadUsername();
    // --- None-standard interaction above ---

    // --- Display scenarios below ---
    void DisplayView(List<string> lines);
    // --- Display scenarios above ---
}
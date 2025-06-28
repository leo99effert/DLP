internal interface IInteraction
{
    // --- Interact with menu below ---
    T GetInput<T>() where T : Enum;
    string GetString();
    // --- Interact with menu above ---

    // --- None-standard interaction below ---
    void PromptLogin();
    // --- None-standard interaction above ---

    // --- Display scenarios below ---
    void DisplayView(List<string> lines);
    // --- Display scenarios above ---
}
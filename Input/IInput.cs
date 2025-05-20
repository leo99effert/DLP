internal interface IInput
{
    public Log Log { get; }
    string Get();
    int GetActionIndex();
}

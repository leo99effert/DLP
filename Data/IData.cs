internal interface IData
{
}

internal interface IData<T> : IData
{
    Task<List<T>> Get();
}
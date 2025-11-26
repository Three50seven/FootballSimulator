namespace Common.Core
{
    public interface IFactory<T> where T : class
    {
        T Create();
    }
}

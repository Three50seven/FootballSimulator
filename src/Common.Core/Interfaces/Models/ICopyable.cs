namespace Common.Core
{
    public interface ICopyable<T> where T : class
    {
        T Copy();
    }
}

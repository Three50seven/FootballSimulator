namespace Common.Core.Domain
{
    public class ContentTemplate : ContentTemplate<object>
    {
        public ContentTemplate(string path, object model)
            : base(path, model)
        {

        }
    }

    public class ContentTemplate<T> : ValueObject<ContentTemplate<T>>
    {
        public ContentTemplate(string path, T model)
        {
            Model = model;
            Path = path;
        }

        public string Path { get; protected set; }

        public T Model { get; protected set; }
    }
}

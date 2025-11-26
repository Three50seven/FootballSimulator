namespace Common.Core.Domain
{
    public class MessageViewContentTemplate<T> : ContentTemplate<T>
    {
        public MessageViewContentTemplate(string templateName, T model, string directory = "MessageTemplates")
            : base(templateName, model)
        {
            Path = $"{directory}/{Path?.Replace(".cshtml", "")}";
        }
    }
}

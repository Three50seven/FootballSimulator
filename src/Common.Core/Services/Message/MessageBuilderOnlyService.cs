using Common.Core.Domain;

namespace Common.Core.Services
{
    /// <summary>
    /// Service to build and process Message entity. Does not commit to repository or database.
    /// </summary>
    public class MessageBuilderOnlyService : MessageServiceBase, IMessageService
    {
        public MessageBuilderOnlyService(IContentRenderer renderer, MessageValidatingInfo validatingInfo) 
            : base(renderer, validatingInfo)
        {
        }
    }
}

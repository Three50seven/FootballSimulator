using Common.Core.Domain;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IMessageSender
    {
        MessageSendResult Send(Message message);
        Task<MessageSendResult> SendAsync(Message message);
    }
}

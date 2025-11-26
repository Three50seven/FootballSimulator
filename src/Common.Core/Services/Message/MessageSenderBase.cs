using Microsoft.Extensions.Logging;
using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Message sending base class that provides exception handling and logging around Send and SendAsync operations
    /// for implementation of <see cref="IMessageSender"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MessageSenderBase<T> : IMessageSender
        where T : MessageSenderBase<T>
    {
        public MessageSenderBase(ILogger<T> logger)
        {
            Logger = logger;
        }

        protected ILogger<T> Logger { get; private set; }

        protected abstract void OnSend(Message message);
        protected abstract Task OnSendAsync(Message message);

        protected virtual void LogInformation(string message)
        {
            Logger?.LogInformation(message);
        }

        protected virtual void LogError(Exception ex)
        {
            Logger?.LogError(ex, ex?.Message);
        }

        public virtual MessageSendResult Send(Message message)
        {
            Guard.IsNotNull(message, nameof(message));

            try
            {
                LogInformation($"Sending message ({message.Guid})");
                OnSend(message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return MessageSendResult.Error(message.Guid, ex);
            }

            return MessageSendResult.Success(message.Guid);
        }

        public virtual async Task<MessageSendResult> SendAsync(Message message)
        {
            Guard.IsNotNull(message, nameof(message));

            try
            {
                LogInformation($"Sending message ({message.Guid})");
                await OnSendAsync(message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return MessageSendResult.Error(message.Guid, ex);
            }

            return MessageSendResult.Success(message.Guid);
        }
    }
}
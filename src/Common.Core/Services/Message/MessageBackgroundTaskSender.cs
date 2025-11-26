using Microsoft.Extensions.Logging;
using Common.Core.Domain;
using Common.Core.Interfaces;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    /// <summary>
    /// Sending class designed for use as a background service for sending a queued Message.
    /// This service is enqueued within <see cref="MessageTaskQueuingService"/> using <see cref="ITaskQueue"/>. 
    /// This setup occurs when <see cref="MessageSettings.QueueType"/> is <see cref="MessageQueueOption.QueueBackgroundTask"/>.
    /// </summary>
    public class MessageBackgroundTaskSender
    {
        private readonly IMessageSender _messageSender;
        private IDomainRepository<Message> _messageRepository;
        private IUnitOfWork _unitOfWork;
        private ILogger<MessageBackgroundTaskSender> _logger;

        public MessageBackgroundTaskSender(
            IMessageSender messageSender,
            IDomainRepository<Message> messageRepository,
            IUnitOfWork unitOfWork,
            ILogger<MessageBackgroundTaskSender> logger)
        {
            _messageSender = messageSender;
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [DisplayName("Sending Message: {0}")]
        public async Task SendAsync(Guid messageGuid, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Looking up Message '{messageGuid}'...");
            var message = await _messageRepository.GetByGuidAsync(messageGuid);
            if (message == null)
                throw new DataObjectNotFoundException(nameof(message), messageGuid);

            _logger.LogInformation($"Message found. Id: {message.Id}");

            _logger.LogInformation(message.ToString());

            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation("Sending message...");
            var result = await _messageSender.SendAsync(message);
            if (result == null)
                throw new NullReferenceException(nameof(MessageSendResult));

            if (!result.Succeeded)
            {
                _logger.LogError(result.Exception, result.ErrorMessage);
                message.Fail(result.ErrorMessage);
            }   
            else
            {
                _logger.LogInformation("Message sent successfully.");
                message.Complete();
            }

            _logger.LogInformation("Updating message record...");

            await _messageRepository.AddOrUpdateAsync(message);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"Message '{message.Guid}' updated.");
        }
    }
}
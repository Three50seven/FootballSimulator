using Common.Core.Domain;
using Common.Core.Validation;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    public class EntityHistoryStore : IEntityHistoryStore
    {
        private readonly ICommandRepository<EntityHistory, long> _repository;
        private readonly IUserId _currentUser;

        public EntityHistoryStore(
            ICommandRepository<EntityHistory, long> repository, 
            IUserId currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public virtual void ProcessCommand(HistoryCommandContext context)
        {
            Guard.IsNotNull(context, nameof(context));

            var history = context.ToEntityHistory(_currentUser.Id);

            context.Entity.OnHistoryUpdate(history);

            if (context.StoreEventRecords)
                _repository.AddOrUpdate(history);
        }

        public virtual async Task ProcessCommandAsync(HistoryCommandContext context)
        {
            Guard.IsNotNull(context, nameof(context));

            var history = context.ToEntityHistory(_currentUser.Id);

            context.Entity.OnHistoryUpdate(history);

            if (context.StoreEventRecords)
                await _repository.AddOrUpdateAsync(history);
        }
    }

    [DisableAutoServiceRegistration]
    public sealed class EntityHistoryStoreNotApplicable : IEntityHistoryStore
    {
        public void ProcessCommand(HistoryCommandContext context)
        {
            return;
        }

        public Task ProcessCommandAsync(HistoryCommandContext context)
        {
            return Task.CompletedTask;
        }
    }
}

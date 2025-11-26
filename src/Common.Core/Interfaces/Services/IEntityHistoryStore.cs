using Common.Core.Domain;
using System.Threading.Tasks;

namespace Common.Core
{
    public interface IEntityHistoryStore
    {
        void ProcessCommand(HistoryCommandContext context);
        Task ProcessCommandAsync(HistoryCommandContext context);
    }
}

using Common.Core.Domain;

namespace Common.EntityFrameworkCore
{
    public interface IContextHistorical
    {
        void AddToHistory(EntityHistory command, bool onlyUnique = true);
    }
}

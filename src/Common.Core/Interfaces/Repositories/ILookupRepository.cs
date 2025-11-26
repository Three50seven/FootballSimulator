using Common.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Core
{
    /// <summary>
    /// Generic repository for standard lookup entity operations.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public interface ILookupRepository<TType> : IQueryRepository<TType> 
        where TType : class
    {
        IEnumerable<SelectItem> GetSelections();
        Task<IEnumerable<SelectItem>> GetSelectionsAsync();
        TType GetByName(string name);
        Task<TType> GetByNameAsync(string name);
    }
}

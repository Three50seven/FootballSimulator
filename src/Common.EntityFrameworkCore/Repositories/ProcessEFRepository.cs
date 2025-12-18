using Microsoft.EntityFrameworkCore;
using Common.Core;
using Common.Core.Domain;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    /// <summary>
    /// Generic repository for standard Process entity <see cref="Process"/>.
    /// </summary>
    /// <typeparam name="TContext">DbContext type of the application where <see cref="Process"/> entity has been configured.</typeparam>
    public class ProcessEFRepository<TContext> : EFDomainRepositoryBase<TContext, Process>
        where TContext : DbContext
    {
        public ProcessEFRepository(TContext context) : base(context)
        {
        }

        protected override IQueryable<Process> GetEntitySet(RepositoryIncludesDefaultOption includes)
        {
            return includes switch
            {
                RepositoryIncludesDefaultOption.All => base.FullEntitySet.IncludeAllRelations(),
                RepositoryIncludesDefaultOption.None => base.FullEntitySet,
                _ => throw new UnsupportedEnumException(nameof(includes)),
            };
        }
    }
}

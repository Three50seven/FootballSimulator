using Microsoft.EntityFrameworkCore;
using Common.Core.Domain;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    public class DocumentDirectoryEFRepository<TContext> : EFLookupRepository<TContext, DocumentDirectory>
        where TContext : DbContext
    {
        public DocumentDirectoryEFRepository(TContext context)
            : base(context)
        {
        }

        protected override IQueryable<DocumentDirectory> EntitySet => base.EntitySet.Include(dd => dd.DocumentDirectoryExtensions)
                                                                                    .ThenInclude(dde => dde.Extension);
    }
}

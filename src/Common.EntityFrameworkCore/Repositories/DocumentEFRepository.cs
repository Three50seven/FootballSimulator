using Microsoft.EntityFrameworkCore;
using Common.Core;
using Common.Core.Domain;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    public class DocumentEFRepository<TContext> : EFDomainRepositoryBase<TContext, Document>
        where TContext : DbContext
    {
        public DocumentEFRepository(TContext context)
            : base(context)
        {
        }

        public DocumentEFRepository(TContext context, IEntityHistoryStore historyStore)
            : base(context, historyStore)
        {
        }

        protected override IQueryable<Document> EntitySet => base.EntitySet.Include(d => d.Directory);

        protected override IQueryable<Document> FullEntitySet => base.FullEntitySet.Include(d => d.Directory)
                                                                                   .ThenInclude(dir => dir.DocumentDirectoryExtensions)
                                                                                   .ThenInclude(dde => dde.Extension);
    }
}

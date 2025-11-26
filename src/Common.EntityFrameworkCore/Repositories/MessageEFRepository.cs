using Microsoft.EntityFrameworkCore;
using Common.Core;
using Common.Core.Domain;
using System.Linq;

namespace Common.EntityFrameworkCore
{
    public class MessageEFRepository<TContext> : EFDomainRepositoryBase<TContext, Message>
        where TContext : DbContext
    {
        public MessageEFRepository(TContext context) 
            : base(context)
        {
        }

        public MessageEFRepository(TContext context, IEntityHistoryStore historyStore) 
            : base(context, historyStore)
        {
        }

        protected override IQueryable<Message> FullEntitySet => base.FullEntitySet.Include(m => m.Recipients)
                                                                                  .Include(m => m.ReplyTos)
                                                                                  .Include(m => m.Category)
                                                                                  .Include(m => m.Attachments)
                                                                                    .ThenInclude(ma => ma.Document)
                                                                                        .ThenInclude(d => d.Directory);
    }
}

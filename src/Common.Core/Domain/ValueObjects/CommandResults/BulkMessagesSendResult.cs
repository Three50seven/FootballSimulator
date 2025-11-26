using System.Collections.Generic;
using System.Linq;

namespace Common.Core.Domain
{
    public class BulkMessagesSendResult : List<MessageSendResult>
    {
        public BulkMessagesSendResult(IEnumerable<MessageSendResult> results)
            : base(results)
        {

        }

        public int SucceededCount => this.Count(r => r.Succeeded);
        public int FailedCount => this.Count(r => !r.Succeeded);
    }
}

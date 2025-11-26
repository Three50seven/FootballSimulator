using System;

namespace Common.Core.Domain
{
    public class MessageCategory : LookupEntity
    {
        public static int DefaultId = 1;

        protected MessageCategory() { }

        public MessageCategory(int id, string name)
            : base (id, name)
        {

        }

        public MessageCategory(Enum @enum)
            : base (@enum)
        {

        }
    }
}

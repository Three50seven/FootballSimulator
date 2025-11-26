using System;

namespace Common.Core.Domain
{
    public class ProcessType : LookupEntity
    {
        private ProcessType() { }
        public ProcessType(Enum @enum)
            : base (@enum)
        {

        }

        public ProcessType(int id, string name)
            : base (id, name)
        {

        }
    }
}

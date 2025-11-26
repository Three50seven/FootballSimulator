namespace Common.Core.Domain
{
    public class ProcessScheduleType : LookupEntity
    {
        private ProcessScheduleType() { }

        public ProcessScheduleType(ProcessScheduleTypeOption type)
            : base (type)
        {

        }
    }
}

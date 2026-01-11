using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class QualificationType : LookupEntity
    {
        private QualificationType() { }
        public QualificationType(QualificationTypeOption name) : base(name)
        {
        }
    }
}
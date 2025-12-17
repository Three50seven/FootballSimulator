using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class Team : FSDataEntity, IArchivable
    {
        private Team() 
        { 
            Name = string.Empty;
        }

        public Team(string name)
        { 
            Name = name;
        }

        public string Name { get; set; }
        public string? Mascot { get; set; }
        public int FoundedYear { get; set; }
        public Guid? RankRandomizer { get; set; }

        public int DivisionId { get; set; }
        public Division? Division { get; set; } = null;
        public int StadiumId { get; set; }
        public Stadium? Stadium { get; set; } = null;

        public bool Archive { get; set; }
        public override string ToString() => Mascot != null ? $"{Name} {Mascot}" : Name;
    }
}

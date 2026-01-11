using Common.Core.Domain;

namespace FootballSimulator.Core.Domain
{
    public class GameType : LookupEntity
    {
        private GameType() { }
        public GameType(GameTypeOption name) : base(name)
        {
        }
    }
}
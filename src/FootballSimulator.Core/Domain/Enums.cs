using System.ComponentModel;

namespace FootballSimulator.Core
{
    public enum EntityTypeOption
    {
        User = 1,
        Country = 2
    }
    public enum RoleOption
    {
        Admin = 1,
        [Description("General User")]
        GeneralUser = 2
    }
    public enum HostEnvironmentOption
    {
        Local = 1,
        Development = 2,
        Staging = 3,
        Test = 4,
        Production = 5
    }
    public enum StadiumQueryIncludeOption
    {
        All = 0,
        None = 1,
        Team = 2,
        Geography = 3
    }
    public enum TeamQueryIncludeOption
    {
        All = 0,
        None = 1,
        Stadium = 2,
        Division = 3
    }
    public enum GameTypeOption
    {
        Intraconference = 1,
        Interconference = 2,
        [Description("17th Game")]
        SeventeenthGame= 3
    }
    public enum SeasonTypeOption
    {
        Preseason = 1,
        [Description("Regular Season")]
        RegularSeason = 2,
        Playoffs = 3
    }
    public enum RankingScopeTypeOption
    {
        Division = 1,
        Conference = 2,
        League = 3
    }
    public enum QualificationTypeOption
    {
        [Description("Division Champion")]
        DivisionChampion = 1,
        WildCard = 2
    }
    public enum StatisticGenerationMethodTypeOption
    {
        Random = 1,
        [Description("Performance-Based")]
        PerformanceBased = 2
    }
}

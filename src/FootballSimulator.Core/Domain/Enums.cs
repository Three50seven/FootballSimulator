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
}

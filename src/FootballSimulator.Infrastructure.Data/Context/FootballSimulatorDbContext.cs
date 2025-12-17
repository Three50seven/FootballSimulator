using Common.Core.Domain;
using Common.EntityFrameworkCore;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public class FootballSimulatorDbContext : IdentityDbContextBase<ApplicationUser, string>
    {
        public FootballSimulatorDbContext(DbContextOptions<FootballSimulatorDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseSnakeCase()
                .PluralizeTableNames()
                .AddEntityHistoryConfigurations<User, EntityTypeOption>()
                .ApplyConfigurationsFromAssembly<FootballSimulatorDbContext>();
        }

        public new DbSet<User>? Users { get; set; }
        public new DbSet<Role>? Roles { get; set; }
        public new DbSet<UserRole>? UserRoles { get; set; }
        public virtual DbSet<UserLoginHistory>? UserLoginHistories { get; set; }

        public virtual DbSet<EntityType>? EntityTypes { get; set; }
        public virtual DbSet<EntityHistory>? EntityHistories { get; set; }
        public virtual DbSet<EntityHistoryChange>? EntityHistoryChanges { get; set; }
        public virtual DbSet<City>? Cities { get; set; }
        public virtual DbSet<Country>? Countries { get; set; }
        public virtual DbSet<State>? States { get; set; }
        public virtual DbSet<ClimateType>? ClimateTypes { get; set; }
        public virtual DbSet<StadiumType>? StadiumTypes { get; set; }
        public virtual DbSet<WeatherType>? WeatherTypes { get; set; }
        public virtual DbSet<Stadium>? Stadiums { get; set; }
        public virtual DbSet<Conference>? Conferences { get; set; }
        public virtual DbSet<Division>? Divisions { get; set; }
        public virtual DbSet<Team>? Teams { get; set; }
    }
}
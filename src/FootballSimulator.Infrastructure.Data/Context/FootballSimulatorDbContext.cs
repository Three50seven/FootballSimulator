using Common.EntityFrameworkCore;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulator.Infrastructure.Data
{
    public class FootballSimulatorDbContext : DbContextBase<FootballSimulatorDbContext>
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


        public virtual DbSet<User>? Users { get; set; }
    }
}

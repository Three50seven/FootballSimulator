using FootballSimulator.Core.Domain;

namespace FootballSimulator.Infrastructure.Data
{
    public static class Seeder
    {
        public static void Seed(FootballSimulatorDbContext context, IServiceProvider serviceProvider)
        {
            SeedSystemUser(context);
            context.SaveChanges();
        }
        private static void SeedSystemUser(FootballSimulatorDbContext context)
        {
            if (context.Users != null && !context.Users!.Any(u => u.UserName == "admin"))
            {
                context.Users.Add(new User("admin", "admin@footballsimulator.com", new Common.Core.Domain.Name("System", "Administrator")));
                context.SaveChanges();
            }
        }
    }
}

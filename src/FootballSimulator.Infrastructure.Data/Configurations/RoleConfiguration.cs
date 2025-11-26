using Common.Core;
using Common.EntityFrameworkCore;
using FootballSimulator.Core;
using FootballSimulator.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSimulator.Infrastructure.Data
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ConfigureLookupEntityProperties<Role, RoleOption>(role =>
            {
                return role switch
                {
                    RoleOption.Admin => new Role(role),
                    RoleOption.GeneralUser => new Role(role),
                    _ => throw new UnsupportedEnumException(role),
                };
            });
        }
    }
}

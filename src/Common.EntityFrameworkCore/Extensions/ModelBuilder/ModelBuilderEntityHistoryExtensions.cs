using Common.Core.Domain;
using Common.Core.Validation;
using Microsoft.EntityFrameworkCore;

namespace Common.EntityFrameworkCore
{
    public static class ModelBuilderEntityHistoryExtensions
    {
        //
        // Summary:
        //     Apply configurations Microsoft.EntityFrameworkCore.IEntityTypeConfiguration`1
        //     for Common.Core.Domain.EntityHistory, Common.Core.Domain.EntityHistoryChange,
        //     and Common.Core.Domain.EntityType. Supply custom user under TUser implementing
        //     Common.Core.Domain.IUser from the calling application to establish foreign
        //     key relationship.
        //
        // Parameters:
        //   modelBuilder:
        //
        // Type parameters:
        //   TUser:
        //     Custom User type from the running application to establish foreign key relationship.
        //
        //
        //   TTypeEnum:
        //     Enum for lookup entity for Common.Core.Domain.EntityType.
        public static ModelBuilder AddEntityHistoryConfigurations<TUser, TTypeEnum>(this ModelBuilder modelBuilder) where TUser : class, IUser where TTypeEnum : Enum
        {
            Guard.IsNotNull(modelBuilder, nameof(modelBuilder));
            return modelBuilder.ApplyConfiguration(new EntityHistoryConfiguration<TUser>())
                               .ApplyConfiguration(new EntityHistoryChangeConfiguration())
                               .ApplyConfiguration(new EntityTypeConfiguration<TTypeEnum>());
        }
    }
}

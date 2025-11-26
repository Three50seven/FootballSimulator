using Common.Core.Domain;
using Common.Core.Validation;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Common.EntityFrameworkCore
{
    public static class MutableModelExtensions
    {
        //
        // Summary:
        //     Get mutable entity types that represent database tables (Common.Core.Domain.IEntity).
        //
        //
        // Parameters:
        //   model:
        public static IEnumerable<IMutableEntityType> GetTableEntityTypes(this IMutableModel model)
        {
            Guard.IsNotNull(model, nameof(model));
            return model.GetEntityTypes().Where(et => typeof(IEntity).IsAssignableFrom(et.ClrType));
        }
    }
}

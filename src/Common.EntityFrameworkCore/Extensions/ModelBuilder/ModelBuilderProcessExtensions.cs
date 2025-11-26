using Microsoft.EntityFrameworkCore;
using Common.Core.Domain;
using Common.Core.Validation;
using System;

namespace Common.EntityFrameworkCore
{
    public static class ModelBuilderProcessExtensions
    {
        /// <summary>
        /// Applies entity configurations for Process entities using <see cref="Process"/>.
        /// Specify the application's <typeparamref name="TUser"/> inheriting <see cref="IUser"/> and 
        /// specify type enum <typeparamref name="TTypeEnum"/> for lookup entity <see cref="ProcessType"/>.
        /// </summary>
        /// <typeparam name="TUser">Application's user entity type.</typeparam>
        /// <typeparam name="TTypeEnum">Enum for lookup entity for <see cref="ProcessType"/>.</typeparam>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder AddProcessConfigurations<TUser, TTypeEnum>(this ModelBuilder modelBuilder)
           where TUser : class, IUser
           where TTypeEnum : Enum
        {
            Guard.IsNotNull(modelBuilder, nameof(modelBuilder));

            return modelBuilder.ApplyConfiguration(new ProcessConfiguration<TUser>())
                               .ApplyConfiguration(new ProcessTypeConfiguration<TTypeEnum>())
                               .ApplyConfiguration(new ProcessParameterConfiguration())
                               .ApplyConfiguration(new ProcessResultConfiguration())
                               .ApplyConfiguration(new ProcessScheduleTypeConfiguration())
                               .ApplyConfiguration(new ProcessRetryConfiguration());
        }
    }
}

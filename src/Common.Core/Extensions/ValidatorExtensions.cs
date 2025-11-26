using Common.Core.Validation;
using System.Linq;

namespace Common.Core
{
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Check if entity is valid. Considered valid if no BrokenRules are returned from validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsValid<T>(this IValidator<T> validator, T entity)
            where T : class
        {
            return !validator.BrokenRules(entity).Any();
        }

        /// <summary>
        /// Throws <see cref="ValidationException"/> if entity is invalid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <param name="entity"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validate<T>(this IValidator<T> validator, T entity)
            where T : class
        {
            var brokenRules = validator.BrokenRules(entity);
            if (brokenRules.Any())
                throw new ValidationException(brokenRules);
        }
    }
}

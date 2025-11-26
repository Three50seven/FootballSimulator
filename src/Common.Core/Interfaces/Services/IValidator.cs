using Common.Core.Validation;

namespace Common.Core
{
    public interface IValidator<T> where T : class
    {
        BrokenRulesList BrokenRules(T entity);
    }
}

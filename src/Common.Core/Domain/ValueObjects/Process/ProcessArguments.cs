using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Core.Domain
{
    /// <summary>
    /// Abstract class to contain list of custom arguments needed for <see cref="Common.Core.Interfaces.IProcessService{T}"/>.
    /// Used to store arguments as well as provide dictionary of argument key/values for abstract storage.
    /// </summary>
    public abstract class ProcessArguments
    {
        /// <summary>
        /// Create instance of arguments needed to execute service <see cref="Common.Core.Interfaces.IProcessService{T}"/>.
        /// </summary>
        /// <param name="userId">UserId representing the user that started a process. Defaults to <see cref="DefaultUser.DefaultId"/>.</param>
        /// <param name="friendlyTaskName">Friendly name of the task executing these arguments. Should be 50 characters or less. Defaults to an attempted humanization of the class name.</param>
        protected ProcessArguments(int? userId = null, string friendlyTaskName = null)
        {
            UserId = userId ?? DefaultUser.DefaultId;

            if (string.IsNullOrEmpty(friendlyTaskName))
            {
                string className = GetType().Name.Replace("Arguments", "").Replace("_", "").Truncate(50);
                FriendlyTaskName = Regex.Replace(className, "([a-z])([A-Z])", "$1 $2");
            }
            else
            {
                FriendlyTaskName = friendlyTaskName;
            }
            
        }

        /// <summary>
        /// UserId representing the user that started a process.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Optional description for the execution of the process.
        /// </summary>
        public virtual string Description { get; }

        /// <summary>
        /// Friendly name of the task executing these arguments. Should be 50 characters or less.
        /// </summary>
        public string FriendlyTaskName { get; private set; }

        protected abstract IEnumerable<KeyValuePair<string, object>> ToArguments();

        /// <summary>
        /// Return key/value pairs of the arguments.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<KeyValuePair<string, string>> ToKeyValues()
        {
            return ToArguments()?.Select(arg => new KeyValuePair<string, string>(arg.Key, arg.Value?.ToString()));
        }

        public override string ToString()
        {
            string name = FriendlyTaskName;
            var values = ToKeyValues();
            
            if (!string.IsNullOrWhiteSpace(Description))
                name = $"{name} - {Description}";

            if (values == null || !values.Any())
                return name;

            return $"{name.Truncate(50, "...")} - {string.Join(" | ", ToKeyValues().Select(kv => $"{kv.Key}: {kv.Value}"))}";
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core.Validation
{
    /// <summary>
    /// List of validation rules that have been broken as part of a process, typically when submitting commands / save actions.
    /// </summary>
    public class BrokenRulesList : List<ValidationRule>
    {
        public const string ModelStateKey = "BrokenRules";

        public static readonly BrokenRulesList Empty = new BrokenRulesList();

        public BrokenRulesList()
        {

        }

        public BrokenRulesList(IEnumerable<ValidationRule> list) 
            : base(list)
        {

        }

        public BrokenRulesList(ValidationRule rule) 
            : base(new List<ValidationRule>() { rule })
        {

        }

        public BrokenRulesList(IEnumerable<string> list) 
            : base(list?.Select(x => new ValidationRule(x)))
        {

        }

        /// <summary>
        /// List out all validation rules based on the non-detailed <see cref="ValidationRule.Message"/>.
        /// </summary>
        public string Message => FormattedResults(detailed: false);

        public void Add(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            Add(new ValidationRule(message));
        }

        public void AddUnique(ValidationRule rule, bool checkSelf = true)
        {
            if (rule == null)
                return;

            if (checkSelf && !this.HasItems())
            {
                Add(rule);
                return;
            }

            if (!this.Any(x => x.Description == rule.Description))
                Add(rule);
        }

        public void AddUniqueRange(IEnumerable<ValidationRule> list)
        {
            if (!list.HasItems())
                return;

            if (!this.HasItems())
            {
                AddRange(list);
                return;
            }

            foreach (var item in list)
            {
                AddUnique(item, checkSelf: false);
            }
        }

        public override string ToString()
        {
            return FormattedResults(detailed: true);
        }

        private string FormattedResults(bool detailed)
        {
            if (this == null || Count == 0)
                return string.Empty;

            if (this.Count == 1)
                return GetRuleMessage(this[0], detailed);

            var sb = new StringBuilder();
            foreach (var item in this)
            {
                sb.AppendLine(GetRuleMessage(item, detailed));
            }
            return sb.ToString();
        }

        public IEnumerable<string> ToMessages()
        {
            return this.Select(x => x.Message);
        }

        private static string GetRuleMessage(ValidationRule rule, bool detailed)
        {
            if (detailed)
                return rule.ToString();
            else
                return rule.Message;
        }

        public virtual IEnumerable<KeyValuePair<string, object>> ToKeyValueList()
        {
            return this.Select(br => new KeyValuePair<string, object>(br.Message, br.Description));
        }
    }
}

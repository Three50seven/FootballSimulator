using System.Text;

namespace Common.Core.Validation
{
    //
    // Summary:
    //     List of validation rules that have been broken as part of a process, typically
    //     when submitting commands / save actions.
    public class BrokenRulesList : List<ValidationRule>
    {
        public const string ModelStateKey = "BrokenRules";

        public static readonly BrokenRulesList Empty = new BrokenRulesList();

        //
        // Summary:
        //     List out all validation rules based on the non-detailed Common.Core.Validation.ValidationRule.Message.
        public string Message => FormattedResults(detailed: false);

        public BrokenRulesList()
        {
        }

        public BrokenRulesList(IEnumerable<ValidationRule> list)
            : base(list)
        {
        }

        public BrokenRulesList(ValidationRule rule)
            : base((IEnumerable<ValidationRule>)new List<ValidationRule> { rule })
        {
        }

        public BrokenRulesList(IEnumerable<string> list)
            : base((list ?? []).Select(x => new ValidationRule(x)))
        {
        }

        public void Add(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Add(new ValidationRule(message));
            }
        }

        public void AddUnique(ValidationRule rule, bool checkSelf = true)
        {
            if (rule is null) return;

            if (checkSelf && !this.HasItems())
            {
                Add(rule);
            }
            else if (!this.Any(x => x.Description == rule.Description))
            {
                Add(rule);
            }
        }

        public void AddUniqueRange(IEnumerable<ValidationRule> list)
        {
            if (!list.HasItems())
            {
                return;
            }

            if (!this.HasItems())
            {
                AddRange(list);
                return;
            }

            foreach (ValidationRule item in list)
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
            if (this == null || base.Count == 0)
            {
                return string.Empty;
            }

            if (base.Count == 1)
            {
                return GetRuleMessage(base[0], detailed);
            }

            StringBuilder stringBuilder = new StringBuilder();
            using (Enumerator enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ValidationRule current = enumerator.Current;
                    stringBuilder.AppendLine(GetRuleMessage(current, detailed));
                }
            }

            return stringBuilder.ToString();
        }

        public IEnumerable<string> ToMessages()
        {
            return this.Select((ValidationRule x) => x.Message);
        }

        private static string GetRuleMessage(ValidationRule rule, bool detailed)
        {
            if (detailed)
            {
                return rule.ToString();
            }

            return rule.Message;
        }

        public virtual IEnumerable<KeyValuePair<string, object>> ToKeyValueList()
        {
            return this.Select((ValidationRule br) => new KeyValuePair<string, object>(br.Message, br.Description));
        }
    }
}

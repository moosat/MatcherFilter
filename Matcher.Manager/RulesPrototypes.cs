using System.Collections.Generic;
using Matcher.Contracts;

namespace Matcher.Manager
{
    public class RulesPrototypes : Dictionary<string, IRule>
    {
        public void AddRuleProptotype(IRule rule)
        {
            Add(rule.TypeName, rule);
        }
    }
}
using System.Collections.Generic;
using Matcher.Contracts;

namespace Matcher.Manager
{
    public class RuleBacket : IRuleBacket
    {
        readonly Dictionary<string, IRule> _rules = new Dictionary<string, IRule>();
        public IRule GetRule(string name)
        {
            return _rules[name];
        }

        public void AddRule(IRule rule)
        {
            _rules[rule.Name] = rule;
        }
    }
}
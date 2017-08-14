using System.Collections.Generic;
using System.Linq;
using Matcher.Contracts;

namespace Matcher.Manager
{
    public static class RuleParser
    {
        public static void AddNewRuleToBacket(string ruleLine, Dictionary<string, IRule> prototypes, IRuleBacket ruleBacket)
        {
            var ruleElements = ruleLine.Split(' ').ToList();
            var ruleName = ruleElements[0].Replace(":", "");
            var ruleType = ruleElements[1];
            ruleElements.RemoveAt(0);
            ruleElements.RemoveAt(0);
            var ruleContent = string.Join(" ", ruleElements);
            if (ruleName == "APPLY")
            {
                ruleContent = ruleType;
                ruleType = ruleName;
            }
            var rulePrototype = prototypes[ruleType];
            var instanseRule = rulePrototype.Clone();
            instanseRule.InitRule(ruleName, ruleContent, ruleBacket);
            ruleBacket.AddRule(instanseRule);
        }
    }
}
using Matcher.BaseRules;
using Matcher.Contracts;
using Matcher.Manager;

namespace Matcher.Tests
{
    public class RuleRunnerTest
    {
        public IRuleBacket RuleBacket = new RuleBacket();
        public RulesPrototypes RulesPrototypes = new RulesPrototypes();
        public RuleRunnerTest()
        {
            IRule rule;
            rule = new BeginWithRule();
            RulesPrototypes.Add(rule.TypeName, rule);            
            rule = new EndWithRule();
            RulesPrototypes.Add(rule.TypeName, rule);            
            rule = new AndRule();
            RulesPrototypes.Add(rule.TypeName, rule);            
        }
    }
}
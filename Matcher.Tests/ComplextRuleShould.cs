using Xunit;

namespace Matcher.Tests
{
    public class ComplextRuleShould
    {
        [Fact]
        public void RunAndRules()
        {
            var ruleRunner = new RuleRunnerTest();
            var beginWith = ruleRunner.RulesPrototypes["BeginWith"].Clone();
            beginWith.InitRule("R1", "begin", ruleRunner.RuleBacket);
            ruleRunner.RuleBacket.AddRule(beginWith);

            var endWith = ruleRunner.RulesPrototypes["EndWith"].Clone();
            endWith.InitRule("R2", "end", ruleRunner.RuleBacket);
            ruleRunner.RuleBacket.AddRule(endWith);

            var and = ruleRunner.RulesPrototypes["AND"].Clone();
            and.InitRule("R3", "R1 R2", ruleRunner.RuleBacket);
            ruleRunner.RuleBacket.AddRule(and);

            var andRule = ruleRunner.RuleBacket.GetRule("R3");
            
            Assert.True(andRule.IsMatch("begin end"));
        }        
    }
}
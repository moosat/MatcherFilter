using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Matcher.BaseRules;
using Xunit;

namespace Matcher.Tests
{
    public class BasicRulesShould
    {
        [Fact]
        public void CheckEndWithRuleIsolated()
        {
            var beginWithRule = new EndWithRule();
            beginWithRule.InitRule("R1", "end", null);
            Assert.True(beginWithRule.IsMatch("begin with begin end"));
            Assert.False(beginWithRule.IsMatch("begin with begin end."));
        }

        [Theory]
        [InlineData("R1","begin", "begin is good", true )]
        [InlineData("R1","begin", "not begin is good", false )]
        public void CheckBeginWithRuleIsolated(string ruleName, string content, string line, bool isMatch)
        {
            var beginWithRule = new BeginWithRule();
            beginWithRule.InitRule(ruleName, content, null);
            Assert.Equal(isMatch, beginWithRule.IsMatch(line));
        }
    }
}

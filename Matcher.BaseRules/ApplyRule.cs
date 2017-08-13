using System.Linq;
using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public class ApplyRule : OperatorRule
    {
        public override string TypeName => "APPLY";
        public override bool IsMatch(string line)
        {
            var applyRule = GetOperands().First();
            return applyRule.IsMatch(line);
        }

        public override IRule Clone()
        {
            return new ApplyRule();
        }
    }
}
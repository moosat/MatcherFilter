using System.Linq;
using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public class NotRule : OperatorRule
    {
        public override string TypeName => "NOT";
        public override bool IsMatch(string line)
        {
            return !GetOperands().First().IsMatch(line);
        }

        public override IRule Clone()
        {
            return new NotRule();
        }
    }
}
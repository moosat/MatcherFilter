using System.Linq;
using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public class OrRule : OperatorRule
    {
        public override string TypeName => "OR";
        public override bool IsMatch(string line)
        {
            return GetOperands().Any(x => x.IsMatch(line));
        }

        public override IRule Clone()
        {
            return new OrRule();
        }
    }
}
using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public class AndRule : OperatorRule
    {
        public override string TypeName => "AND";
        public override bool IsMatch(string line)
        {
            return GetOperands().TrueForAll(x => x.IsMatch(line));
        }

        public override IRule Clone()
        {
            return new AndRule();
        }
    }
}
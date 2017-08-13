using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public class ContainsRule: StringRule
    {
        public override string TypeName => "Contain";

        public override bool IsMatch(string line)
        {
            return line.Contains(Content);
        }

        public override IRule Clone()
        {
            return new ContainsRule();
        }
    }
}
using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public class BeginWithRule : StringRule
    {
        public override string TypeName => "BeginWith";
 
        public override bool IsMatch(string line)
        {
            return line.StartsWith(Content);
        }

        public override IRule Clone()
        {
            return new BeginWithRule();
        }
    }
}
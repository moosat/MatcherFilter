using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public class EndWithRule:BaseRule
    {
        public override string TypeName => "EndWith";
  
        public override bool IsMatch(string line)
        {
            return line.EndsWith(Content);
        }

        public override IRule Clone()
        {
            return new EndWithRule();
        }
    }
}
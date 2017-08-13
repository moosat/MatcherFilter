using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public abstract class StringRule : BaseRule
    {
        public override void InitRule(string name, string content, IRuleBacket ruleBacket)
        {
            base.InitRule(name, content.Trim('"'), ruleBacket);
        }        
    }
}
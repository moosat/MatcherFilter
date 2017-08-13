using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public abstract class BaseRule : IRule
    {
        protected IRuleBacket RuleBacket;

        public abstract string TypeName { get; }
        public string Name { get; private set; }
        public string Content { get; private set; }
        public abstract bool IsMatch(string line);
        public virtual void InitRule(string name, string content, IRuleBacket ruleBacket)
        {
            Name = name;
            Content = content;
            RuleBacket = ruleBacket;
        }

        public abstract IRule Clone();
        
    }
}
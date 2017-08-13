namespace Matcher.Contracts
{
    public interface IRule
    {
        string TypeName { get; }
        string Name { get; }
        string Content { get; }
        bool IsMatch(string line);
        void InitRule(string name, string content, IRuleBacket ruleBacket);
        IRule Clone();
    }
}
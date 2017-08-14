namespace Matcher.Contracts
{
    public interface IRuleBacket
    {
        IRule GetRule(string name);
        void AddRule(IRule rule);
    }
}

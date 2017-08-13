using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matcher.Contracts;

namespace Matcher.BaseRules
{
    public abstract class OperatorRule : BaseRule
    {
        protected List<IRule> GetOperands()
        {
            var ruleNames = Content.Split(' ');

            return ruleNames.Select(ruleName => RuleBacket.GetRule(ruleName)).ToList();
        }
    }
}

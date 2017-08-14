using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matcher.BaseRules;
using Matcher.Contracts;

namespace ExtraRules
{
    public class XorRule: OperatorRule
    {
        public override string TypeName => "XOR";
        public override bool IsMatch(string line)
        {            
            var op1 = GetOperands()[0];
            var op2 = GetOperands()[1];
            return op1.IsMatch(line) ^ op2.IsMatch(line);
        }

        public override IRule Clone()
        {
            return new XorRule();
        }
    }
}

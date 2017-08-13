using System;
using System.Text;
using System.Threading.Tasks;


namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new Runner();
            var rulesDllPath = @"C:\Repos\Mos\MatcherRepo\MatcherFilter\Matcher.BaseRules\bin\Debug\Matcher.BaseRules.dll";
            runner.InitRuleFactories(rulesDllPath);

            var rulePath = @"C:\Repos\Mos\MatcherRepo\MatcherFilter\Rules.txt";

            runner.InstansiateRules(rulePath);

            var textPath = @"C:\Repos\Mos\MatcherRepo\MatcherFilter\Text.txt";

            runner.RunTheApplyRule(textPath);
            Console.ReadKey();
        }
    }
}

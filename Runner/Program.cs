using System;
using System.Text;
using System.Threading.Tasks;


namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var rulesDllPath = @"C:\Repos\Mos\MatcherRepo\MatcherFilter\Matcher.BaseRules\bin\Debug\Matcher.BaseRules.dll";
            var rulePath = @"C:\Repos\Mos\MatcherRepo\MatcherFilter\Rules.txt";
            var textPath = @"C:\Repos\Mos\MatcherRepo\MatcherFilter\Text.txt";

            var runner = new Runner(rulePath, textPath);
            runner.InitRuleFactories(rulesDllPath);
            runner.InstansiateRules(rulePath);
            runner.RunTheApplyRule(textPath);

            Console.ReadKey();
        }
    }
}

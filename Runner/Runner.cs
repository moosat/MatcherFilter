using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Matcher.Contracts;
using SimpleInjector;


namespace Runner
{
    public class Runner
    {
        readonly Container _container;
        private readonly List<IRule> _allRulesPrototypes;

        public Runner()
        {
            _allRulesPrototypes = new List<IRule>();
            _container = new Container();
        }

        public void InitRuleFactories(string path)
        {
            var assembly = Assembly.LoadFile(path);

            _container.RegisterCollection(typeof(IRule), assembly);
            _container.Verify();
            var ruleTemplates  = _container.GetAllInstances<IRule>();
            _allRulesPrototypes.AddRange(ruleTemplates);
        }

        public void InstansiateRules(string rulePath)
        {
            var ruleLines = File.ReadAllLines(rulePath);
            foreach (var ruleLine in ruleLines)
            {
                AddRuleFromLine(ruleLine);
            }
        }
   //     private BlockingCollection<string>
        public void RunTheApplyRule(string textPath)
        {
            var applyRule = _ruleBacket.GetRule("R1");

            //TODO: read line by line
            var lines = File.ReadAllLines(textPath);
            foreach (var line in lines)
            {
                if (applyRule.IsMatch(line))
                    Console.WriteLine(line);
            }
        }

        private readonly RuleBacket _ruleBacket = new RuleBacket();
        private void AddRuleFromLine(string ruleLine)
        {
            var ruleElements = ruleLine.Split(' ').ToList();
            var ruleName = ruleElements[0].Replace(":","");
            var ruleType  = ruleElements[1];
            ruleElements.RemoveAt(0);
            ruleElements.RemoveAt(0);
            var ruleContent = string.Join(" ", ruleElements);
            if (ruleName == "APPLY")
                ruleType = ruleName;
            var rulePrototype = _allRulesPrototypes.Find(x => x.TypeName == ruleType); // change to dictionary
            var instanseRule = rulePrototype.Clone();
            instanseRule.InitRule(ruleName, ruleContent, _ruleBacket);
            _ruleBacket.AddRule(instanseRule);           
        }
    }
}

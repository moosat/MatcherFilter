using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Matcher.Contracts;
using SimpleInjector;
using SimpleInjector.Advanced;


namespace Runner
{
    public class Runner
    {
        private readonly Container _container;
        private readonly List<IRule> _allRulesPrototypes;
        private readonly SortedList<long, string> _sortedOutputLines;
        private readonly RuleBacket _ruleBacket = new RuleBacket();
        private readonly string _resultPath;
        private readonly string _textPath;
        private readonly string _rulesPath;

        public Runner(string rulePath, string textPath, string resultPath)
        {
            _rulesPath = rulePath;
            _textPath = textPath;

            _resultPath = resultPath;

            _allRulesPrototypes = new List<IRule>();
            _container = new Container();
            _outputLines = new BlockingCollection<OutputLine>(100);
            _sortedOutputLines = new SortedList<long, string>();
        }

        public void InitRuleFactories()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Matcher.BaseRules.dll");
            var assembly = Assembly.LoadFile(path);
            var assemblies = new List<Assembly> {assembly};
            InitExtraRule(assemblies);

            _container.RegisterCollection(typeof(IRule), assemblies);

            _container.Verify();
            var ruleTemplates  = _container.GetAllInstances<IRule>();
            _allRulesPrototypes.AddRange(ruleTemplates);
        }

        private void InitExtraRule(List<Assembly> assemblies)
        {
            var assemblyPaths = Directory.GetFiles(Environment.CurrentDirectory, "ContentFilter.*.Rules.dll");
            assemblies.AddRange(assemblyPaths.Select(assemblyPath => Assembly.LoadFile(assemblyPath)));
        }

        public void InstansiateRules()
        {
            var ruleLines = File.ReadAllLines(_rulesPath);
            foreach (var ruleLine in ruleLines)
            {
                AddRuleFromLine(ruleLine);
            }
        }

        private readonly BlockingCollection<OutputLine> _outputLines;
        private long _lastFlushedIndex = -1;

        public void RunTheApplyRule()
        {
            var applyRule = _ruleBacket.GetRule("APPLY");

            var consumerTask = Consume();
            Parallel.ForEach(File.ReadLines(_textPath), (line, state, linenumber) =>
            {
                _outputLines.Add(applyRule.IsMatch(line)
                    ? new OutputLine() {LineNumber = linenumber, Line = linenumber + ":" + line}
                    : new OutputLine() {LineNumber = linenumber, Line = null});
            });
            _outputLines.CompleteAdding();
            consumerTask.Wait();
        }

        private Task Consume()
        {
            return Task.Run(() =>
            {
                while (!_outputLines.IsCompleted)
                {
                    OutputLine line = null;

                    try
                    {
                        line = _outputLines.Take();
                    }
                    catch (InvalidOperationException) { }
                   
                    if (line != null)
                    {
                        _sortedOutputLines.Add(line.LineNumber, line.Line);
                        var firstIndex = _sortedOutputLines.Keys.First();
                        var lastIndex = _sortedOutputLines.Keys.Last();

                        if ( (lastIndex - firstIndex == _sortedOutputLines.Count - 1) &&
                            firstIndex == _lastFlushedIndex+1 )
                        {
                            _lastFlushedIndex = lastIndex;
                            FlushLinesToOutput();
                        }
                    }
                }
            });
        }

        private void FlushLinesToOutput()
        {     
            File.AppendAllLines(_resultPath, _sortedOutputLines.Values.Where(x => x != null));
            _sortedOutputLines.Clear();
        }


        private void AddRuleFromLine(string ruleLine)
        {
            var ruleElements = ruleLine.Split(' ').ToList();
            var ruleName = ruleElements[0].Replace(":","");
            var ruleType  = ruleElements[1];
            ruleElements.RemoveAt(0);
            ruleElements.RemoveAt(0);
            var ruleContent = string.Join(" ", ruleElements);
            if (ruleName == "APPLY")
            {
                ruleContent = ruleType;
                ruleType = ruleName;
            }
            var rulePrototype = _allRulesPrototypes.Find(x => x.TypeName == ruleType); // change to dictionary
            var instanseRule = rulePrototype.Clone();
            instanseRule.InitRule(ruleName, ruleContent, _ruleBacket);
            _ruleBacket.AddRule(instanseRule);           
        }
        private class OutputLine
        {
            public string Line { get; set; }
            public long LineNumber { get; set; }
        }

    }
}

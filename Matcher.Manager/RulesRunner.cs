using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Matcher.Contracts;
using SimpleInjector;

namespace Matcher.Manager
{
    public class RulesRunner
    {
        private readonly Container _container;
        private readonly RulesPrototypes _allRulesPrototypes;
        private readonly SortedList<long, string> _sortedOutputLines;
        private readonly RuleBacket _ruleBacket = new RuleBacket();
        private readonly string _resultPath;
        private readonly string _textPath;
        private readonly string _rulesPath;
        private readonly BlockingCollection<OutputLine> _outputLines;
        private long _lastFlushedIndex = -1;


        public RulesRunner(string rulePath, string textPath, string resultPath)
        {
            _rulesPath = rulePath;
            _textPath = textPath;

            _resultPath = resultPath;

            _allRulesPrototypes = new RulesPrototypes();
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
            foreach (var ruleTemplate in ruleTemplates)
            {
                _allRulesPrototypes.AddRuleProptotype(ruleTemplate);
            }
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
            RuleParser.AddNewRuleToBacket(ruleLine, _allRulesPrototypes, _ruleBacket);
        }
        private class OutputLine
        {
            public string Line { get; set; }
            public long LineNumber { get; set; }
        }
    }
}

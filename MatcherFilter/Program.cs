using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using Matcher.Manager;

namespace MatherFilter.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            var parser = new Parser();
            if (parser.ParseArguments(args, options) && !options.Quit)
            {
                ValidateArgumentAndRun(options);
            }
            if (options.Quit)
                return;

            do
            {
                options = new Options();
                Console.WriteLine(options.GetUsage());
                var interactiveArgs = Console.ReadLine().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                if (parser.ParseArguments(interactiveArgs, options) && !options.Quit)
                {
                    ValidateArgumentAndRun(options);                    
                }
            } while (!options.Quit);

            Console.WriteLine("Thank you for using this app.");
        }

        private static void ValidateArgumentAndRun(Options options)
        {
            var canRun = true;
            if (!File.Exists(options.InputFile))
            {
                Console.WriteLine("Source file is not exists.");
                canRun = false;
            }
            if (!File.Exists(options.RulesFile))
            {
                Console.WriteLine("Rule file is not exists.");
                canRun = false;
            }
            if (File.Exists(options.ResultFile))
            {
                Console.WriteLine("Resultfile already exists.");
                canRun = false;
                Console.WriteLine("I'm deleting it hahahaha");
                File.Delete(options.ResultFile);
                canRun = true;
            }
            if (!canRun)
                return;

            Console.WriteLine("Working...\n");

            var runner = new RulesRunner(options.RulesFile, options.InputFile, options.ResultFile);
            runner.InitRuleFactories();
            runner.InstansiateRules();
            runner.RunTheApplyRule();

            Console.WriteLine("Finished.\n");

        }
    }

    class Options
    {
        [Option('s', "source", Required = true, HelpText = "Source file to filter it's content.")]
        public string InputFile { get; set; }

        [Option('r', "rules", Required = false, HelpText = "Rules text file definition. default Rule.txt", DefaultValue = "Rule.txt")]
        public string RulesFile { get; set; }

        [Option('t', "result", Required = false, HelpText = "Result file. default Result.txt", DefaultValue = "Result.txt")]
        public string ResultFile { get; set; }

        [Option('q', null, HelpText = "Quit the application.")]
        public bool Quit { get; set; }

        private HelpText _helpText;
        [HelpOption]
        public string GetUsage()
        {
            if (_helpText == null)
            {
                _helpText = new HelpText
                {
                    Heading = new HeadingInfo("Mathcer_Filter", "1.0"),
                    Copyright = new CopyrightInfo("Mo.", 2017),
                    AdditionalNewLineAfterOption = true,
                    AddDashesToOption = true
                };
                _helpText.AddPreOptionsLine("You can put in the running directory rules dlls that have the ContentFilter.*.Rules.dll pattern");
                _helpText.AddOptions(this);

            }
            return _helpText;
        }
    }
}




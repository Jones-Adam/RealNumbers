// ReSharper disable UnusedMember.Local
namespace RealNumbers.Benchmarks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;

    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            var config = new RealNumbersConfig();

            if (args.Length > 0)
            {
                RunAll(config, args);
            }
            else
            {
                RunAll(config);
            }
        }

        private static IEnumerable<Summary> RunAll(IConfig config)
        {
            var switcher = new BenchmarkSwitcher(typeof(Program).Assembly);
            var summaries = switcher.Run(new[] { "*" }); // , config);
            return summaries;
        }

        private static IEnumerable<Summary> RunAll(IConfig config, string[] args)
        {
            var switcher = new BenchmarkSwitcher(typeof(Program).Assembly);
            var summaries = switcher.Run(args, config);
            return summaries;
        }

        private static IEnumerable<Summary> RunSingle<T>()
        {
            var summaries = new[] { BenchmarkRunner.Run<T>() };
            return summaries;
        }
    }
}

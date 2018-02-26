namespace RealNumbers.Benchmarks
{
    using System;
    using System.Globalization;
    using BenchmarkDotNet.Attributes;

    [DisassemblyDiagnoser(printAsm: true)]
    public class RealIntegerPythagorasBenchmarks
    {
        [Params(10)]
        public int N;

        private int dataintA;
        private double datadblA;
        private Real64 datarealA;
        private decimal datadecimalA;
        private int dataintB;
        private double datadblB;
        private Real64 datarealB;
        private decimal datadecimalB;

        [GlobalSetup]
        public void GlobalSetup()
        {
            dataintA = 3;
            dataintB = 4;
            datadblA = 3d;
            datadblB = 4d;
            datarealA = Real64.FromInt(3);
            datarealB = Real64.FromInt(3);
            datadecimalA = 3m;
            datadecimalB = 4m;
        }

        [Benchmark(Baseline = true)]
        public int MultiplyInt()
        {
            int res = (int)Math.Sqrt((dataintA * dataintA) + (dataintB * dataintB));

            return res;
        }

        [Benchmark]
        public double MultiplyDouble()
        {
            double res = Math.Sqrt((datadblA * datadblA) + (datadblB * datadblB));

            return res;
        }

        [Benchmark]
        public decimal MultiplyDecimal()
        {
            decimal res = Sqrt((datadecimalA * datadecimalA) + (datadecimalB * datadecimalB));

            return res;
        }

        [Benchmark]
        public Real64 MultiplyReal()
        {
            Real64 res = ((datarealA * datarealA) + (datarealB * datarealB)).Sqrt();

            return res;
        }

        private static decimal Sqrt(decimal x, decimal epsilon = 0.0M)
        {
            if (x < 0) throw new OverflowException("Cannot calculate square root from a negative number");

            decimal current = (decimal)Math.Sqrt((double)x), previous;
            do
            {
                previous = current;
                if (previous == 0.0M) return 0;
                current = (previous + (x / previous)) / 2;
            }
            while (Math.Abs(previous - current) > epsilon);
            return current;
        }
    }
}

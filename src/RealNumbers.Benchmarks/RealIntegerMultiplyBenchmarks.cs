namespace RealNumbers.Benchmarks
{
    using System;
    using System.Globalization;
    using BenchmarkDotNet.Attributes;

    [DisassemblyDiagnoser(printAsm: true)]
    public class RealIntegerMultiplyBenchmarks
    {
        [Params(100)]
        public int N;

        private int dataint;
        private double datadbl;
        private Real64 datareal;
        private decimal datadecimal;

        [GlobalSetup]
        public void GlobalSetup()
        {
            dataint = 3;
            datadbl = 3d;
            datareal = Real64.FromInt(3);
            datadecimal = 3m;
        }

        [Benchmark(Baseline = true)]
        public int MultiplyInt()
        {
            int res = 0;
            for (int i = 0; i < N; i++)
            {
                res = res * dataint;
            }

            return res;
        }

        [Benchmark]
        public double MultiplyDouble()
        {
            double res = 0;
            for (int i = 0; i < N; i++)
            {
                res = res * datadbl;
            }

            return res;
        }

        [Benchmark]
        public decimal MultiplyDecimal()
        {
            decimal res = 0;
            for (int i = 0; i < N; i++)
            {
                res = res * datadecimal;
            }

            return res;
        }

        [Benchmark]
        public Real64 MultiplyReal()
        {
            Real64 res = default;
            for (int i = 0; i < N; i++)
            {
                res = res * datareal;
            }

            return res;
        }
    }
}

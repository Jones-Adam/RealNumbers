namespace RealNumbers.Benchmarks
{
    using System;
    using System.Globalization;
    using System.Numerics;
    using BenchmarkDotNet.Attributes;

    [DisassemblyDiagnoser(printAsm: true)]
    public class RealIntegerAdditionBenchmarks
    {
        [Params(100)]
        public int N;

        private int dataint;
        private double datadbl;
        private Real64 datareal;
        private decimal datadecimal;
        private BigInteger databigint;

        [GlobalSetup]
        public void GlobalSetup()
        {
            dataint = 3;
            datadbl = 3d;
            datareal = Real64.FromInt(3);
            datadecimal = 3m;
            databigint = 3;
        }

        [Benchmark(Baseline = true)]
        public int AdditionInt()
        {
            int res = 0;
            for (int i = 0; i < N; i++)
            {
                res = res + dataint;
            }

            return res;
        }

        [Benchmark]
        public double AdditionDouble()
        {
            double res = 0;
            for (int i = 0; i < N; i++)
            {
                res = res + datadbl;
            }

            return res;
        }

        [Benchmark]
        public decimal AdditionDecimal()
        {
            decimal res = 0;
            for (int i = 0; i < N; i++)
            {
                res = res + datadecimal;
            }

            return res;
        }

        [Benchmark]
        public BigInteger AdditionBigInteger()
        {
            BigInteger res = 0;
            for (int i = 0; i < N; i++)
            {
                res = res + databigint;
            }

            return res;
        }

        [Benchmark]
        public Real64 AdditionReal()
        {
            Real64 res = default;
            for (int i = 0; i < N; i++)
            {
                res = res + datareal;
            }

            return res;
        }
    }
}

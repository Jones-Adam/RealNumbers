namespace RealNumbers.Benchmarks
{
    using System;
    using System.Globalization;
    using BenchmarkDotNet.Attributes;

    public class RealDecimalAdditionBenchmarks
    {
        [Params(100)]
        public int N;

        private float datafloat;
        private double datadbl;
        private Real64 datareal;
        private decimal datadecimal;

        [GlobalSetup]
        public void GlobalSetup()
        {
            datafloat = 3.89f;
            datadbl = 3.89d;
            datareal = Real64.FromDecimal(3.89m);
            datadecimal = 3.89m;
        }

        [Benchmark]
        public double AdditionFloat()
        {
            float res = 0;
            for (int i = 0; i < N; i++)
            {
                res = res + datafloat;
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

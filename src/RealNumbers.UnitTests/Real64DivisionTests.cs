namespace RealNumbers.UnitTests
{
    using System;
    using Xunit;

    public class Real64DivisionTests
    {
        [Theory]
        [InlineData(4, 2, 2)]
        [InlineData(45, -5, -9)]
        public void DivIntInt(int num1, int num2, int expected)
        {
            Real64 r1 = num1;
            Real64 r2 = num2;
            Real64 radd = r1 / r2;
            Assert.Equal(expected, radd.ToInteger());
        }

        [Theory]
        [InlineData(6, 0.2d, 30)]
        [InlineData(30, 1.5d, 20)]
        [InlineData(-5, -2.5d, 2)]
        public void DivIntDec(int num1, double num2, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = (Real64)num2;
            Real64 radd = r1 / r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(0.31d, 0.62d, 0.5d)]
        [InlineData(0.1d, 0.1d, 1)]
        [InlineData(0.1d, 0.001d, 100)]
        [InlineData(0.001d, 0.1d, 0.01d)]
        public void DivDecDec(double num1, double num2, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = (Real64)num2;
            Real64 radd = r1 / r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }
        
    }
}

namespace RealNumbers.UnitTests
{
    using System;
    using Xunit;

    public class Real64SubtractionTests
    {
        [Theory]
        [InlineData(4, 2, 2)]
        [InlineData(45, -5, 50)]
        [InlineData(3, 4, -1)]
        [InlineData(3, -4, 7)]
        [InlineData(-3, 4, -7)]
        [InlineData(-1, 1, -2)]
        [InlineData(-1, -1, 0)]
        [InlineData(1, 0, 1)]
        [InlineData(0, 0, 0)]
        [InlineData(-2147483648, -2147483647, -1)]
        public void SubIntInt(int num1, int num2, int expected)
        {
            Real64 r1 = num1;
            Real64 r2 = num2;
            Real64 radd = r1 - r2;
            Assert.Equal(expected, radd.ToInteger());
        }

        [Theory]
        [InlineData(6, 0.2d, 5.8)]
        [InlineData(-5, -2.5d, -2.5d)]
        public void SubIntDec(int num1, double num2, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = (Real64)num2;
            Real64 radd = r1 - r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(0.31d, 0.62d, -0.31d)]
        [InlineData(0.1d, 0.1d, 0)]
        [InlineData(0.1d, 0.001d, 0.099d)]
        [InlineData(0.001d, 0.1d, -0.099d)]
        public void SubDecDec(double num1, double num2, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = (Real64)num2;
            Real64 radd = r1 - r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }
        
    }
}

namespace RealNumbers.UnitTests
{
    using System;
    using Xunit;

    public class Real64MultiplicationTests
    {
        [Theory]
        [InlineData(4, 2, 8)]
        [InlineData(10, -5, -50)]
        [InlineData(3, 4, 12)]
        [InlineData(3, -4, -12)]
        [InlineData(-3, 4, -12)]
        [InlineData(-1, 1, -1)]
        [InlineData(-1, -1, 1)]
        [InlineData(0, 1, 0)]
        [InlineData(3, 3, 9)]
        [InlineData(0, 0, 0)]
        public void MulIntInt(int num1, int num2, int expected)
        {
            Real64 r1 = num1;
            Real64 r2 = num2;
            Real64 radd = r1 * r2;
            Assert.Equal(expected, radd.ToInteger());
        }

        [Theory]
        [InlineData(6, 0.2d, 1.2)]
        [InlineData(-5, -2.5d, 12.5d)]
        public void MulIntDec(int num1, double num2, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = (Real64)num2;
            Real64 radd = r1 * r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(0.31d, 0.62d, 0.1922d)]
        [InlineData(0.1d, 0.1d, 0.01d)]
        [InlineData(0.1d, 0.001d, 0.0001d)]
        [InlineData(0.001d, 0.1d, 0.0001d)]
        [InlineData(0.5d, 0.5d, 0.25d)]
        [InlineData(0.1d, 0.5d, 0.05d)]
        [InlineData(5d, 0.5d, 2.5d)]
        [InlineData(0.01d, 0.001d, 0.00001d)]
        [InlineData(5.1d, 4.3d, 21.93d)]
        [InlineData(0.9d, 3.9d, 3.51d)]
        [InlineData(20d, 20d, 400d)]
        [InlineData(2.695d, 3.2547d, 8.7714165d)]
        [InlineData(2.695d, 13.2547d, 35.7214165d)]
        [InlineData(52.695d, 73.2547d, 3860.1564165d)]
        [InlineData(5.662222d, 2d, 11.324444d)]
        [InlineData(0.121d, 0.121d, 0.014641d)]
        public void MulDecDec(double num1, double num2, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = (Real64)num2;
            Real64 radd = r1 * r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }
        
    }
}

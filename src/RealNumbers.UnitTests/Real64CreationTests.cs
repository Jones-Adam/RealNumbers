namespace RealNumbers.UnitTests
{
    using System;
    using Xunit;

    public class Real64CreationTests
    {
        [Theory]
        [InlineData(3, 4, 12)]
        [InlineData(3, -4, -12)]
        [InlineData(-3, 4, -12)]
        [InlineData(-1, 1, -1)]
        [InlineData(-1, -1, 1)]
        [InlineData(0, 1, 0)]
        [InlineData(3, 3, 9)]
        [InlineData(0, 0, 0)]
        public void IntegerMultiplication(int num1, int num2, int expected)
        {
            Real64 r1 = Real64.FromInt(num1);
            Real64 r2 = Real64.FromInt(num2);
            Real64 radd = r1 * r2;
            Assert.Equal(expected, radd.ToInteger());
        }

        [Theory]
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
        public void DecimalMultiplication(double num1, double num2, double expected)
        {
            Real64 r1 = Real64.FromDouble(num1);
            Real64 r2 = Real64.FromDouble(num2);
            Real64 radd = r1 * r2;
            Assert.Equal(expected, radd.ToDouble());
        }

        [Theory]
        [InlineData(3, 4, -1)]
        [InlineData(3, -4, 7)]
        [InlineData(-3, 4, -7)]
        [InlineData(-1, 1, -2)]
        [InlineData(-1, -1, 0)]
        [InlineData(1, 0, 1)]
        [InlineData(0, 0, 0)]
        [InlineData(-2147483648, -2147483647, -1)]
        public void IntegerSubtraction(int num1, int num2, int expected)
        {
            Real64 r1 = num1;
            Real64 r2 = num2;
            Real64 rsub = r1 - r2;
            Assert.Equal(expected, rsub.ToInteger());
        }

        [Theory]
        [InlineData(3)]
        [InlineData(-100)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-2147483648)]
        [InlineData(293323)]
        [InlineData(2147483647)]
        public void FromInteger(int num)
        {
            Real64 r = num;
            Assert.True(r.IsInteger);
            Assert.False(r.IsInfinity);
            Assert.False(r.IsNaN);
            Assert.False(r.IsNegativeInfinity);
            Assert.False(r.IsPositiveInfinity);
            Assert.Equal(num, r.ToInteger());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(0.9)]
        [InlineData(0.1)]
        [InlineData(1.4343434)]
        [InlineData(11231232.123123)]
        [InlineData(-131113.12312331)]
        [InlineData(0)]
        public void FromDouble(double num)
        {
            Real64 r = (Real64)num;
            Assert.False(r.IsInfinity);
            Assert.False(r.IsNaN);
            Assert.False(r.IsNegativeInfinity);
            Assert.False(r.IsPositiveInfinity);
            Assert.Equal(num, r.ToDouble());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(0.9)]
        [InlineData(0.1)]
        [InlineData(1.4343434)]
        [InlineData(1231232.787981)]
        [InlineData(-1231113.123)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10097)]
        [InlineData(1.5)]
        [InlineData(50)]
        [InlineData(0.002)]
        [InlineData(0.121)]
        public void FromDecimal(decimal num)
        {
            Real64 r = (Real64)num;
            Assert.True(r.IsDecimal);
            Assert.False(r.IsInfinity);
            Assert.False(r.IsNaN);
            Assert.False(r.IsNegativeInfinity);
            Assert.False(r.IsPositiveInfinity);
            Assert.Equal(num, r.ToDecimal());
        }

        [Theory]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NaN)]
        public void FromDoubleSpecials(double num)
        {
            Real64 r = (Real64)num;
            Assert.Equal(num, r.ToDouble());
        }

        [Theory]
        [InlineData(1, 100000)]
        [InlineData(-4, 10)]
        [InlineData(4, -10)]
        [InlineData(-4, -10)]
        [InlineData(4, 10)]
        public void FromFraction(int numerator, int denominator)
        {
            Real64 r = Real64.FromFraction(numerator, denominator);
            var fraction = r.ToFraction();
            Assert.True(r.IsFraction);
            Assert.Equal(numerator, fraction.numerator);
            Assert.Equal(denominator, fraction.denominator);
        }

        [Fact]
        public void PositiveInfinity()
        {
            Real64 r1 = Real64.PositiveInfinity;
            Assert.True(r1.IsInfinity);
            Assert.True(r1.IsPositiveInfinity);
            Assert.Equal(double.PositiveInfinity, r1.ToDouble());
        }

        [Fact]
        public void NegativeInfinity()
        {
            Real64 r1 = Real64.NegativeInfinity;
            Assert.True(r1.IsInfinity);
            Assert.True(r1.IsNegativeInfinity);
            Assert.Equal(double.NegativeInfinity, r1.ToDouble());
        }

        [Fact]
        public void SpecialPI()
        {
            Real64 r = Real64.PI;
            Assert.Equal(Math.PI, r.ToDouble(), 12);
        }

        [Fact]
        public void SpecialE()
        {
            Real64 r = Real64.E;
            Assert.Equal(Math.E, r.ToDouble(), 12);
        }

        [Fact]
        public void FracSEPI()
        {
            Real64 r1 = Real64.FromFraction(Real64.E, Real64.PI);
            Assert.True(r1.IsFraction);
            Assert.Equal(Math.E / Math.PI, r1.ToDouble(), 12);
        }
    }
}

namespace RealNumbers.UnitTests
{
    using System;
    using Xunit;

    public class Real64FractionTests
    { 
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

        [Theory]
        [InlineData(1, 2, 1, 2, 1d)]
        public void FractionAddition(int n1, int d1, int n2, int d2, double result)
        {
            Real64 r1 = Real64.FromFraction(n1, d1);
            Real64 r2 = Real64.FromFraction(n2, d2);
            Real64 res = r1 + r2;
            Assert.Equal(new decimal(result), res.ToDecimal());
        }

        [Theory]
        [InlineData(3, 2, 1, 2, 1d)]
        public void FractionSubtraction(int n1, int d1, int n2, int d2, double result)
        {
            Real64 r1 = Real64.FromFraction(n1, d1);
            Real64 r2 = Real64.FromFraction(n2, d2);
            Real64 res = r1 - r2;
            Assert.Equal(new decimal(result), res.ToDecimal());
        }

        [Theory]
        [InlineData(1, 2, 1, 2, 0.25d)]
        public void FractionMultiplication(int n1, int d1, int n2, int d2, double result)
        {
            Real64 r1 = Real64.FromFraction(n1, d1);
            Real64 r2 = Real64.FromFraction(n2, d2);
            Real64 res = r1 * r2;
            Assert.Equal(new decimal(result), res.ToDecimal());
        }
    }
}

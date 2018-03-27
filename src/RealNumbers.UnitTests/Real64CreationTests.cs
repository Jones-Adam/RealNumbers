namespace RealNumbers.UnitTests
{
    using System;
    using Xunit;
    
    public class Real64CreationTests
    {
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
            // Assert.True(r.IsInteger);
            // Assert.False(r.IsMaxValue);
            // Assert.False(r.IsPositiveMaxValue);
            // Assert.False(r.IsNegativeMaxValue);
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
            //Assert.False(r.IsMaxValue);
            //Assert.False(r.IsPositiveMaxValue);
            //Assert.False(r.IsNegativeMaxValue);
            Assert.Equal(num, r.ToDouble());
        }

        [Theory]
        [InlineData(0.9)]
        [InlineData(0.1)]
        [InlineData(1.4343434)]
        [InlineData(1231232.787981)]
        [InlineData(-1231113.123)]
        [InlineData(1.5)]
        [InlineData(0.002)]
        [InlineData(0.121)]
        public void DecimalFromDecimal(decimal num)
        {
            Real64 r = (Real64)num;
            Assert.True(r.IsDecimal);
            // Assert.False(r.IsMaxValue);
            // Assert.False(r.IsPositiveMaxValue);
            // Assert.False(r.IsNegativeMaxValue);
            Assert.Equal(num, r.ToDecimal());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10097)]
        [InlineData(50)]
        public void IntegerFromDecimal(decimal num)
        {
            Real64 r = (Real64)num;
            Assert.True(r.IsFraction);
            // Assert.False(r.IsMaxValue);
            // Assert.False(r.IsPositiveMaxValue);
            // Assert.False(r.IsNegativeMaxValue);
            Assert.Equal(num, r.ToDecimal());
        }

        /*
        [Theory]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NaN)]
        public void FromDoubleSpecials(double num)
        {
            Real64 r = (Real64)num;
            Assert.Equal(num, r.ToDouble());
        }
        */

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
            var sign = Math.Sign(numerator * denominator);
            Assert.True(r.IsFraction);
            Assert.Equal(Math.Abs(numerator) * sign, fraction.numerator);
            Assert.Equal(Math.Abs(denominator), fraction.denominator);
        }

        [Theory]
        [InlineData(4)]
        public void FromFractionSpecialDenominator(int numerator)
        {
            Real64 r1 = Real64.FromFraction(numerator, Real64.PI);
            var fraction = r1.ToFraction();
            Assert.True(r1.IsFraction);
            Assert.Equal(numerator, fraction.numerator);
            var d = r1.ToDouble();
            Assert.Equal(numerator / Math.PI, d, 12);
        }

        [Theory]
        [InlineData(4)]
        public void FromFractionSpecialNumerator(int denominator)
        {
            Real64 r1 = Real64.FromFraction(Real64.PI, denominator);
            var fraction = r1.ToFraction();
            Assert.True(r1.IsFraction);
            Assert.Equal(denominator, fraction.denominator);
            var d = r1.ToDouble();
            Assert.Equal(Math.PI / denominator, d, 12);
        }
     
        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(0)]
        public void FromSqrt(int radical)
        {
            Real64 r1 = Real64.FromRadical(2, radical);
            Assert.False(r1.IsDecimal);
            var d = r1.ToDouble();
            Assert.Equal(Math.Sqrt(radical), d, 12);
        }

        /*

        [Fact]
        public void PositiveInfinity()
        {
            Real64 r1 = Real64.PositiveMaxValue;
            Assert.True(r1.IsMaxValue);
            Assert.True(r1.IsPositiveMaxValue);
            Assert.Equal(double.PositiveInfinity, r1.ToDouble());
        }

        [Fact]
        public void NegativeInfinity()
        {
            Real64 r1 = Real64.NegativeMaxValue;
            Assert.True(r1.IsMaxValue);
            Assert.True(r1.IsNegativeMaxValue);
            Assert.Equal(double.NegativeInfinity, r1.ToDouble());
        }
        */

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
        public void SpecialESqrt()
        {
            Real64 r = Real64.E.Sqrt();
            Assert.Equal(Math.Sqrt(Math.E), r.ToDouble(), 12);
        }

        [Fact]
        public void SpecialPISqrt()
        {
            Real64 r = Real64.PI.Sqrt();
            Assert.Equal(Math.Sqrt(Math.PI), r.ToDouble(), 12);
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

namespace RealNumbers.UnitTests
{
    using System;
    using Xunit;
    
    public class Real64StringTests
    {
        [Theory]
        [InlineData(4, "4")]
        [InlineData(17000, "17000")]
        [InlineData(0, "0")]
        public void IntegerToString(int number, string expected)
        {
            Real64 r1 = number;
            Assert.Equal(expected, r1.ToString());
        }

        [Theory]
        [InlineData(0d, "0")]
        [InlineData(0.3d, "0.3")]
        [InlineData(-0.3d, "-0.3")]
        [InlineData(0.003d, "0.003")]
        [InlineData(-3d, "-3")]
        [InlineData(1.234d, "1.234")]
        [InlineData(-1.234d, "-1.234")]
        [InlineData(122211.827234d, "122211.827234")]
        [InlineData(50000.000001d, "50000.000001")]
        [InlineData(-50000d, "-50000")]
        [InlineData(50000d, "50000")]
        public void DoubleToString(double number, string expected)
        {
            Real64 r1 = (Real64)number;
            Assert.Equal(expected, r1.ToString());
        }

        [Theory]
        [InlineData(4,4, "4∕4")]
        [InlineData(1,2, "1∕2")]
        [InlineData(35,69, "35" + "\u2215" + "69")]
        public void FractionToString(int number, int denominator, string expected)
        {
            Real64 r1 = Real64.FromFraction(number, denominator);
            Assert.Equal(expected, r1.ToString());
        }

    }
}

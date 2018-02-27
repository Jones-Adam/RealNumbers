namespace RealNumbers.UnitTests
{
    using System;
    using Xunit;

    public class Real64AdditionTests
    {
        [Theory]
        [InlineData(3, 4, 7)]
        [InlineData(3, -4, -1)]
        [InlineData(-3, 4, 1)]
        [InlineData(-1, 1, 0)]
        [InlineData(-1, -1, -2)]
        [InlineData(0, 1, 1)]
        [InlineData(3, 3, 6)]
        [InlineData(0, 0, 0)]
        [InlineData(-2147483648, 2147483647, -1)]
        public void AddIntInt(int num1, int num2, int expected)
        {
            Real64 r1 = num1;
            Real64 r2 = num2;
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToInteger());
        }

        [Theory]
        [InlineData(0.2d, 0.2d, 0.4d)]
        [InlineData(0.1d, 0.01d, 0.11d)]
        [InlineData(0.01d, 0.1d, 0.11d)]
        [InlineData(3.1d, 0.1d, 3.2d)]
        [InlineData(3.9d, 0.121d, 4.021d)]
        public void AddDecDec(double num1, double num2, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = (Real64)num2;
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(0.2d, Math.PI + 0.2)]
        public void AddDecDecSPI(double num1, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = Real64.PI;
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(0.2d, Math.E + 0.2)]
        public void AddDecDecSE(double num1, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = Real64.E;
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact]
        public void AddDecSDecS()
        {
            Real64 r1 = Real64.PI;
            Real64 r2 = Real64.PI;
            Real64 radd = r1 + r2;
            Assert.Equal(Math.PI * 2, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(0.2d, 2, 4)]
        public void AddDecRoot(double num1, int root, int num2)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = default; // from root num2;
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Pow(num2, -root) + num1, radd.ToDouble(), 12);
        }

        [Fact(Skip = "Not Implemented")]
        public void AddDecSRoot()
        {
            Real64 r1 = Real64.PI;
            Real64 r2 = default; // root
            Real64 radd = r1 + r2;
            Assert.Equal(Math.PI + Math.Sqrt(2), radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(9, 4, 5)]
        public void AddRootRoot(int num1, int num2, double expected)
        {
            Real64 r1 = default; // from root
            Real64 r2 = default; // from root
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(0.2d)]
        public void AddDecRootS(double num1)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = Real64.PI.Sqrt();
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Sqrt(Math.PI) + num1, radd.ToDouble(), 12);
        }

        [Fact(Skip = "Not Implemented")]
        public void AddDecSRootS()
        {
            Real64 r1 = Real64.PI;
            Real64 r2 = Real64.PI.Sqrt();
            Real64 radd = r1 + r2;
            Assert.Equal(Math.PI + Math.Sqrt(Math.PI), radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(0.2d, 0.2d)]
        public void AddRootRootS(int root, int num1)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = Real64.E.Sqrt();
            Real64 radd = r1 + r2;
            double expected = Math.Pow(num1, -root) + Math.Sqrt(Math.E);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact(Skip = "Not Implemented")]
        public void AddRootSRootS()
        {
            Real64 r1 = Real64.PI.Sqrt();
            Real64 r2 = Real64.E.Sqrt();
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Sqrt(Math.PI) + Math.Sqrt(Math.E), radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(0.2d, 1, 5, 0.4d)]
        public void AddDecFrac(double d, int num, int denom, double expected)
        {
            Real64 r1 = (Real64)d;
            Real64 r2 = Real64.FromFraction(num, denom);
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(0.2d, 0.2d, 0.4d)]
        public void AddDecSFrac(int num, int denom, double expected)
        {
            Real64 r1 = Real64.PI;
            Real64 r2 = Real64.FromFraction(num, denom);
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(2, 4, 4, 2, 4)]
        public void AddRootFrac(int root, int root2, int num, int denom, double expected)
        {
            Real64 r1 = default; //from root
            Real64 r2 = Real64.FromFraction(num, denom);
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(5, 4)]
        public void AddRootSFrac(int num, int denom)
        {
            Real64 r1 = Real64.PI.Sqrt();
            Real64 r2 = Real64.FromFraction(num, denom);
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Sqrt(Math.PI) + num / denom, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(1, 2, 1, 2, 1d)]
        [InlineData(1, 5, 3, 5, 0.8d)]
        public void AddFracFrac(int n1, int d1, int n2, int d2, double result)
        {
            Real64 r1 = Real64.FromFraction(n1, d1);
            Real64 r2 = Real64.FromFraction(n2, d2);
            Real64 res = r1 + r2;
            Assert.Equal(new decimal(result), res.ToDecimal(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(0.2d, 0.2d)]
        public void AddDecFracS(double num1, int denom)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = default; // pi / denom
            Real64 radd = r1 + r2;
            double expected = num1 + Math.PI / denom;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact(Skip = "Not Implemented")]
        public void AddDecSFracS()
        {
            Real64 r1 = Real64.PI;
            Real64 r2 = default; // pi / 2
            Real64 radd = r1 + r2;
            Assert.Equal(Math.PI + Math.PI / 2, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(2, 4, 2)]
        public void AddRootFracS(int root, int root2, int denom)
        {
            Real64 r1 = default; // root
            Real64 r2 = default; // pi / denom
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Pow(root2, 1/root) + Math.PI / denom, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(2)]
        public void AddRootSFracS(int num)
        {
            Real64 r1 = Real64.PI.Sqrt();
            Real64 r2 = default;
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Sqrt(Math.PI) + Math.PI / num, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(1, 2, 2)]
        public void AddFracFracS(int n1, int d1, int d2)
        {
            Real64 r1 = Real64.FromFraction(n1, d1);
            Real64 r2 = default; // pi / d2
            Real64 radd = r1 + r2;
            double expected = (n1 / d1) + (Math.PI / d2);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(2, 2)]
        public void AddFracSFracS(int d1, int d2)
        {
            Real64 r1 = default; //  (Real64)num1;
            Real64 r2 = default; // (Real64)num2;
            Real64 radd = r1 + r2;
            double expected = (Math.PI / d1) + (Math.PI / d2);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(0.2d, 4)]
        public void AddDecFracNS(double num1, int n1)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = default; // n1 / pi
            Real64 radd = r1 + r2;
            double expected = num1 + (n1 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(4)]
        public void AddDecSFracNS(int n1)
        {
            Real64 r1 = Real64.E;
            Real64 r2 = default; // n1 / pi
            Real64 radd = r1 + r2;
            double expected = Math.E + (n1 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(2, 4, 4)]
        public void AddRootFracNS(int root, int root2, int n1)
        {
            Real64 r1 = default; // from root
            Real64 r2 = default; // n1 / pi
            Real64 radd = r1 + r2;
            double expected = Math.Pow(root2, 1 / root) + (n1 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(4)]
        public void AddRootSFracNS(int n1)
        {
            Real64 r1 = default; // from root e
            Real64 r2 = default; // n1 / pi
            Real64 radd = r1 + r2;
            double expected = Math.Sqrt(Math.E) + (n1 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(1, 2, 2)]
        public void AddFracFracNS(int n1, int d1, int n2)
        {
            Real64 r1 = Real64.FromFraction(n1, d1);
            Real64 r2 = default; // (Real64)num2;
            Real64 radd = r1 + r2;
            double expected = (n1 / d1) + (n2 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact(Skip = "Not Implemented")]
        public void AddFracSFracNS()
        {
            Real64 r1 = default;
            Real64 r2 = default;
            Real64 radd = r1 + r2;
            double expected = (Math.PI / 2) + (2 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(1, 2)]
        public void AddFracNSFracNS(int num1, int num2)
        {
            Real64 r1 = default; // Real64.FromFraction(num1, pi)
            Real64 r2 = default; // Real64.FromFraction(num2, pi)
            Real64 radd = r1 + r2;
            double expected = (num1 / Math.PI) + (num2 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(0.2d)]
        public void AddDecFracSS(double num1)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = default; // 
            Real64 radd = r1 + r2;
            double expected = num1 + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact(Skip = "Not Implemented")]
        public void AddDecSFracSS()
        {
            Real64 r1 = default;
            Real64 r2 = default;
            Real64 radd = r1 + r2;
            double expected = Math.E + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(2, 4)]
        public void AddRootFracSS(int root, int root2)
        {
            Real64 r1 = default;
            Real64 r2 = default;
            Real64 radd = r1 + r2;
            double expected = Math.Pow(root2, 1 / root) + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact(Skip = "Not Implemented")]
        public void AddRootSFracSS()
        {
            Real64 r1 = default;
            Real64 r2 = default;
            Real64 radd = r1 + r2;
            double expected = Math.Sqrt(Math.PI) + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(1, 2)]
        public void AddFracFracSS(int num, int demon)
        {
            Real64 r1 = Real64.FromFraction(num, demon);
            Real64 r2 = default;
            Real64 radd = r1 + r2;
            double expected = (num/demon) + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(4)]
        public void AddFracSFracSS(int num1)
        {
            Real64 r1 = default;
            Real64 r2 = default;
            Real64 radd = r1 + r2;
            double expected = Math.PI / num1 + Math.PI / Math.PI;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory(Skip = "Not Implemented")]
        [InlineData(4)]
        public void AddFracNSFracSS(int num1)
        {
            Real64 r1 = default;
            Real64 r2 = default;
            Real64 radd = r1 + r2;
            double expected = num1 / Math.PI + Math.PI / Math.PI;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact(Skip = "Not Implemented")]
        public void AddFracSSFracSS()
        {
            Real64 r1 = default;
            Real64 r2 = default;
            Real64 radd = r1 + r2;
            double expected = Math.E / Math.PI + Math.PI / Math.PI;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }
    }
}

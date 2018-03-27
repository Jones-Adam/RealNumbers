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
        [InlineData(6, 0.2d, 6.2d)]
        [InlineData(10, 0.01d, 10.01d)]
        [InlineData(4, 3.1d, 7.1d)]
        [InlineData(16, 0.121d, 16.121d)]
        public void AddIntDec(int num1, double num2, double expected)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = (Real64)num2;
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
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


        [Theory]
        [InlineData(0.2d, 2, 4)]
        public void AddDecRoot(double num1, int root, int num2)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 =  Real64.FromRadical(root, num2);
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Pow(num2, 1.0/root) + num1, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(2)]
        public void AddDecSRoot(int num)
        {
            Real64 r1 = Real64.PI;
            Real64 r2 = Real64.FromRadical(2, num);
            Real64 radd = r1 + r2;
            Assert.Equal(Math.PI + Math.Sqrt(num), radd.ToDouble(), 12);
        }


        [Theory]
        [InlineData(9, 4, 2, 2, 5)]
        public void AddRootRoot(int num1, int num2, int root1, int root2, double expected)
        {
            Real64 r1 = ((Real64)num1).NthRoot(root1);
            Real64 r2 = ((Real64)num2).NthRoot(root2);
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(0.2d, 2)]
        public void AddDecRootS(double num1, int root)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = Real64.PI.NthRoot(root);
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Pow(Math.PI, 1.0 / root) + num1, radd.ToDouble(), 12);
        }


        [Fact]
        public void AddDecSRootS()
        {
            Real64 r1 = Real64.PI;
            Real64 r2 = Real64.PI.Sqrt();
            Real64 radd = r1 + r2;
            Assert.Equal(Math.PI + Math.Sqrt(Math.PI), radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(2, 4)]
        public void AddRootRootS(int root, int num1)
        {
            Real64 r1 = ((Real64)num1).NthRoot(root);
            Real64 r2 = Real64.E.Sqrt();
            Real64 radd = r1 + r2;
            double expected = Math.Pow(num1, 1.0 / root) + Math.Sqrt(Math.E);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }


        [Fact]
        public void AddRootSRootS()
        {
            Real64 r1 = Real64.PI.Sqrt();
            decimal check1 = r1.ToDecimal();
            Real64 r2 = Real64.E.Sqrt();
            decimal check2 = r2.ToDecimal();
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Sqrt(Math.PI) + Math.Sqrt(Math.E), radd.ToDouble(), 12);
        }

       
        [Theory]
        [InlineData(0.2d, 1, 5, 0.4d)]
        public void AddDecFrac(double d, int num, int denom, double expected)
        {
            Real64 r1 = (Real64)d;
            Real64 r2 = Real64.FromFraction(num, denom);
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(2, 4, 3.64159265359d)]
        public void AddDecSFrac(int num, int denom, double expected)
        {
            Real64 r1 = Real64.PI;
            Real64 r2 = Real64.FromFraction(num, denom);
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }
         
        [Theory]
        [InlineData(2, 4, 4, 2, 4)]
        public void AddRootFrac(int root, int root2, int num, int denom, double expected)
        {
            Real64 r1 = ((Real64)root2).NthRoot(root);
            Real64 r2 = Real64.FromFraction(num, denom);
            Real64 radd = r1 + r2;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(5, 4)]
        public void AddRootSFrac(int num, int denom)
        {
            Real64 r1 = Real64.PI.Sqrt();
            Real64 r2 = Real64.FromFraction(num, denom);
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Sqrt(Math.PI) + (double)num / (double)denom, radd.ToDouble(), 12);
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

        [Theory]
        [InlineData(0.2d, 2)]
        public void AddDecFracN(double num1, int denom)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = Real64.FromFraction(Real64.PI, denom);
            Real64 radd = r1 + r2;
            double expected = num1 + (Math.PI / denom);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact]
        public void AddDecSFracN()
        {
            Real64 r1 = Real64.PI;
            Real64 r2 = Real64.FromFraction(Real64.PI, 2);
            Real64 radd = r1 + r2;
            Assert.Equal(Math.PI + Math.PI / 2, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(2, 4, 2)]
        public void AddRootFracN(int root, int root2, int denom)
        {
            Real64 r1 = ((Real64)root2).NthRoot(root);
            Real64 r2 = Real64.FromFraction(Real64.PI, denom);
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Pow(root2, 1.0/root) + (Math.PI / denom), radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(2)]
        public void AddRootSFracN(int num)
        {
            Real64 r1 = Real64.PI.Sqrt();
            Real64 r2 = Real64.FromFraction(Real64.PI, num);
            Real64 radd = r1 + r2;
            Assert.Equal(Math.Sqrt(Math.PI) + Math.PI / num, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(1, 2, 2)]
        public void AddFracFracN(int n1, int d1, int d2)
        {
            Real64 r1 = Real64.FromFraction(n1, d1);
            Real64 r2 = Real64.FromFraction(Real64.PI, d2);
            Real64 radd = r1 + r2;
            double expected = ((double)n1 / (double)d1) + (Math.PI / d2);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(2, 2)]
        public void AddFracNFracN(int d1, int d2)
        {
            Real64 r1 = Real64.FromFraction(Real64.PI, d1);
            Real64 r2 = Real64.FromFraction(Real64.PI, d2);
            Real64 radd = r1 + r2;
            double expected = (Math.PI / d1) + (Math.PI / d2);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(0.2d, 4)]
        public void AddDecFracD(double num1, int n1)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = Real64.FromFraction(n1, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = num1 + (n1 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(4)]
        public void AddDecSFracD(int n1)
        {
            Real64 r1 = Real64.E;
            Real64 r2 = Real64.FromFraction(n1, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = Math.E + (n1 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(2, 4, 4)]
        public void AddRootFracD(int root, int root2, int n1)
        {
            Real64 r1 = ((Real64)root2).NthRoot(root);
            Real64 r2 = Real64.FromFraction(n1, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = Math.Pow(root2, 1.0 / root) + (n1 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(4)]
        public void AddRootSFracD(int n1)
        {
            Real64 r1 = Real64.E.Sqrt();
            Real64 r2 = Real64.FromFraction(n1, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = Math.Sqrt(Math.E) + (n1 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(1, 2, 2)]
        public void AddFracFracD(int n1, int d1, int n2)
        {
            Real64 r1 = Real64.FromFraction(n1, d1);
            Real64 r2 = Real64.FromFraction(n2, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = ((double)n1 / (double)d1) + (n2 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(1, 2)]
        public void AddFracNFracD(int n1, int d1)
        {
            Real64 r1 = Real64.FromFraction(Real64.PI, d1);
            Real64 r2 = Real64.FromFraction(n1, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = (Math.PI / d1) + (n1 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(1, 2)]
        public void AddFracDFracD(int num1, int num2)
        {
            Real64 r1 = Real64.FromFraction(num1, Real64.PI);
            Real64 r2 = Real64.FromFraction(num2, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = (num1 / Math.PI) + (num2 / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(0.2d)]
        public void AddDecFracS(double num1)
        {
            Real64 r1 = (Real64)num1;
            Real64 r2 = Real64.FromFraction(Real64.PI, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = num1 + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact]
        public void AddDecSFracS()
        {
            Real64 r1 = Real64.E;
            Real64 r2 = Real64.FromFraction(Real64.PI, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = Math.E + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(2, 4)]
        public void AddRootFracS(int root, int root2)
        {
            Real64 r1 = ((Real64)root2).NthRoot(root);
            Real64 r2 = Real64.FromFraction(Real64.PI, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = Math.Pow(root2, 1.0 / root) + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact]
        public void AddRootSFracS()
        {
            Real64 r1 = Real64.PI.Sqrt();
            Real64 r2 = Real64.FromFraction(Real64.PI, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = Math.Sqrt(Math.PI) + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(1, 2)]
        public void AddFracFracS(int num, int demon)
        {
            Real64 r1 = Real64.FromFraction(num, demon);
            Real64 r2 = Real64.FromFraction(Real64.PI, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = ((double)num/(double)demon) + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(4)]
        public void AddFracNFracS(int num1)
        {
            Real64 r1 = Real64.FromFraction(Real64.PI, num1);
            Real64 r2 = Real64.FromFraction(Real64.PI, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = Math.PI / num1 + Math.PI / Math.PI;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Theory]
        [InlineData(4)]
        public void AddFracDFracS(int num1)
        {
            Real64 r1 = Real64.FromFraction(num1, Real64.PI);
            Real64 r2 = Real64.FromFraction(Real64.PI, Real64.PI);
            Assert.Equal(1d, r2.ToDouble(), 12);
            Real64 radd = r1 + r2;
            double expected = (num1 / Math.PI) + (Math.PI / Math.PI);
            Assert.Equal(expected, radd.ToDouble(), 12);
        }

        [Fact]
        public void AddFracSFracS()
        {
            Real64 r1 = Real64.FromFraction(Real64.E, Real64.PI);
            Real64 r2 = Real64.FromFraction(Real64.PI, Real64.PI);
            Real64 radd = r1 + r2;
            double expected = Math.E / Math.PI + Math.PI / Math.PI;
            Assert.Equal(expected, radd.ToDouble(), 12);
        }
    }
}

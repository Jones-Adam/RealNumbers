﻿namespace System
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public readonly partial struct Real64
    {
        #region Addition Operations

        private static RealBinaryOperator[] AddOperators = new RealBinaryOperator[] {
            AddDecDec,
            AddDecDecS,
            AddDecSDecS,
            AddDecRoot,
            AddDecSRoot,
            AddRootRoot,
            AddDecRootS,
            AddDecSRootS,
            AddRootRootS,
            AddRootSRootS,
            AddDecFrac,
            AddDecSFrac,
            AddRootFrac,
            AddRootSFrac,
            AddFracFrac,
            AddDecFracN,
            AddDecSFracN,
            AddRootFracN,
            AddRootSFracN,
            AddFracFracN,
            AddFracNFracN,
            AddDecFracD,
            AddDecSFracD,
            AddRootFracD,
            AddRootSFracD,
            AddFracFracD,
            AddFracNFracD,
            AddFracDFracD,
            AddDecFracS,
            AddDecSFracS,
            AddRootFracS,
            AddRootSFracS,
            AddFracFracS,
            AddFracNFracS,
            AddFracDFracS,
            AddFracSFracS
        };

        private static Real64 AddDecDec(Real64 r1, Real64 r2)
        {
            long result;
            long expresult;
            int resultOffset = 1;
            (long d1mantissa, long d1exp) = r1.GetSegments();
            (long d2mantissa, long d2exp) = r2.GetSegments();
            if (d1exp == d2exp)
            {
                result = d1mantissa + d2mantissa;
                expresult = d1exp;
                resultOffset += (expresult < 0) ? CountMinimumByteSegments((ulong)~expresult) : CountMinimumByteSegments((ulong)expresult);
            }
            else
            {
                if (d2exp < d1exp)
                {
                    long difference = d1exp - d2exp;
                    long g = d1mantissa * (long)Math.Pow(10, difference);
                    result = g + d2mantissa;
                    expresult = d1exp + difference;
                }
                else
                {
                    long difference = d2exp - d1exp;
                    long g = d2mantissa * (long)Math.Pow(10, difference);
                    result = g + d1mantissa;
                    expresult = d2exp - difference;
                }
                resultOffset += (expresult < 0) ? CountMinimumByteSegments((ulong)~expresult) : CountMinimumByteSegments((ulong)expresult);
            }
            ulong final = ((ulong)result << (resultOffset * 8)) | ((uint)resultOffset & 0xF);

            if (expresult != 0)
            {
                final |= ((ulong)expresult & offsetMasks[resultOffset]) << 8;
            }
            return new Real64(final);
        }

        private static Real64 AddDecDecS(Real64 r1, Real64 r2)
        {
            if (r2.IsNaN)
            {
                return Real64.NaN;
            }

            if (r2.IsInfinity)
            {
                return r2;
            }

            return AddDecDec(r1, ConvertSpecialToDecimal(r2));
        }

        private static Real64 AddDecRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();

        private static Real64 AddDecSDecS(Real64 r1, Real64 r2)
        {
            if (r1.IsPositiveInfinity)
            {
                return (r2.IsNegativeInfinity) ? new Real64(0ul) : r1;
            }
            if (r1.IsNegativeInfinity)
            {
                return (r2.IsPositiveInfinity) ? new Real64(0ul) : r1;
            }
            if (r2.IsPositiveInfinity)
            {
                return r2;
            }
            if (r2.IsNegativeInfinity)
            {
                return r2;
            }
            if (r1.IsNaN || r2.IsNaN)
            {
                return Real64.NaN;
            }

            // TODO: if sign is different and same special return 0;

            return AddDecDec(ConvertSpecialToDecimal(r1), ConvertSpecialToDecimal(r2));
        }

        private static Real64 AddDecSRoot(Real64 r1, Real64 r2)
        {
            if (r1.IsNaN)
            {
                return Real64.NaN;
            }

            if (r1.IsInfinity)
            {
                return r1;
            }

            return AddDecRoot(ConvertSpecialToDecimal(r1), r2);
        }
        private static Real64 AddDecSRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecSFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecSFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecSFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();

        private static Real64 AddFracFrac(Real64 r1, Real64 r2)
        {
            (long demoniator1, long numerator1) = r1.GetSegments();
            (long demoniator2, long numerator2) = r2.GetSegments();
            if (demoniator1 == demoniator2)
            {
                long finalnum = numerator1 + numerator2;
                int offset = r1.GetOffset();
                ulong offsetmask = offsetMasks[offset] << 8;
                ulong h = (r1.unum & ~offsetmask) | (((ulong)finalnum << 8) & offsetmask);
                return new Real64(h);
            }
            throw new NotImplementedException();
        }

        private static Real64 AddFracFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 AddFracFracD(Real64 r1, Real64 r2)
        {
            throw new NotImplementedException();
        }

        private static Real64 AddFracFracS(Real64 r1, Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }

        private static Real64 AddFracNFracN(Real64 r1, Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }

        private static Real64 AddFracNFracD(Real64 r1, Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }

        private static Real64 AddFracNFracS(Real64 r1, Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }

        private static Real64 AddFracDFracD(Real64 r1, Real64 r2)
        {
            (long demoniator1, long numerator1) = r1.GetSegments();
            (long demoniator2, long numerator2) = r2.GetSegments();
            if (demoniator1 == demoniator2)
            {
                long finalnum = numerator1 + numerator2;
                int offset = r1.GetOffset();
                ulong offsetmask = offsetMasks[offset] << 8;
                ulong h = (r1.unum & ~offsetmask) | (((ulong)finalnum << 8) & offsetmask);
                return new Real64(h);
            }
            else
            {
                return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
            }
        }

        private static Real64 AddFracDFracS(Real64 r1, Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }
        private static Real64 AddFracSFracS(Real64 r1, Real64 r2)
        {
            // currently fraction elements must be integers so only way to handle this is to decimalise them
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }


        #endregion

        #region Subtraction Operations

        private static RealBinaryOperator[] SubOperators = new RealBinaryOperator[] {
            SubDecDec,
            SubDecDecS,
            SubDecSDecS,
            SubDecRoot,
            SubDecSRoot,
            SubRootRoot,
            SubDecRootS,
            SubDecSRootS,
            SubRootRootS,
            SubRootSRootS,
            SubDecFrac,
            SubDecSFrac,
            SubRootFrac,
            SubRootSFrac,
            SubFracFrac,
            SubDecFracN,
            SubDecSFracN,
            SubRootFracN,
            SubRootSFracN,
            SubFracFracN,
            SubFracNFracN,
            SubDecFracD,
            SubDecSFracD,
            SubRootFracD,
            SubRootSFracD,
            SubFracFracD,
            SubFracNFracD,
            SubFracDFracD,
            SubDecFracS,
            SubDecSFracS,
            SubRootFracS,
            SubRootSFracS,
            SubFracFracS,
            SubFracNFracS,
            SubFracDFracS,
            SubFracSFracS
        };

        private static Real64 SubDecDec(Real64 r1, Real64 r2)
        {
            long result;
            long expresult;
            int resultOffset;
            (long d1mantissa, long d1exp) = r1.GetSegments();
            (long d2mantissa, long d2exp) = r2.GetSegments();
            if (d1exp == d2exp)
            {
                result = d1mantissa - d2mantissa;
                expresult = d1exp;
                int sigbit2 = (result < 0) ? CountMinimumByteSegments((ulong)~result) : CountMinimumByteSegments((ulong)result);
                resultOffset = Math.Max(sigbit2, 1);
            }
            else
            {
                result = 0;
                expresult = 0;
                resultOffset = 1;
            }
            ulong final = ((ulong)result << (resultOffset * 8)) | (uint)resultOffset;

            if (expresult != 0)
            {
                final |= (ulong)expresult;
            }
            return new Real64(final);
        }

        private static Real64 SubDecDecS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSDecS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracFrac(Real64 r1, Real64 r2)
        {
            (long demoniator1, long numerator1) = r1.GetSegments();
            (long demoniator2, long numerator2) = r2.GetSegments();
            if (demoniator1 == demoniator2)
            {
                long finalnum = numerator1 - numerator2;
                int offset = r1.GetOffset();
                ulong offsetmask = offsetMasks[offset] << 8;
                ulong h = (r1.unum & ~offsetmask) | (((ulong)finalnum << 8) & offsetmask);
                return new Real64(h);
            }
            throw new NotImplementedException();
        }

        private static Real64 SubFracFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracNFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracNFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracNFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracDFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracDFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();

        #endregion

        #region Multiply Operations

        private static RealBinaryOperator[] MulOperators = new RealBinaryOperator[] {
            MulDecDec,
            MulDecDecS,
            MulDecSDecS,
            MulDecRoot,
            MulDecSRoot,
            MulRootRoot,
            MulDecRootS,
            MulDecSRootS,
            MulRootRootS,
            MulRootSRootS,
            MulDecFrac,
            MulDecSFrac,
            MulRootFrac,
            MulRootSFrac,
            MulFracFrac,
            MulDecFracN,
            MulDecSFracN,
            MulRootFracN,
            MulRootSFracN,
            MulFracFracN,
            MulFracNFracN,
            MulDecFracD,
            MulDecSFracD,
            MulRootFracD,
            MulRootSFracD,
            MulFracFracD,
            MulFracNFracD,
            MulFracDFracD,
            MulDecFracS,
            MulDecSFracS,
            MulRootFracS,
            MulRootSFracS,
            MulFracFracS,
            MulFracNFracS,
            MulFracDFracS,
            MulFracSFracS
        };

        private static Real64 MulDecDec(Real64 r1, Real64 r2)
        {
            long result;
            long expresult;
            int resultOffset = 1;
            (long d1mantissa, long d1exp) = r1.GetSegments();
            (long d2mantissa, long d2exp) = r2.GetSegments();

            result = d1mantissa * d2mantissa;

            int expected = CountDigits(d1mantissa) + CountDigits(d2mantissa);
            int adjust = CountDigits(result) - expected;
            expresult = d1exp + d2exp + adjust;

            resultOffset += (expresult < 0) ? CountMinimumByteSegments((ulong)~expresult) : CountMinimumByteSegments((ulong)expresult);
            ulong final = ((ulong)result << (resultOffset * 8)) | (uint)resultOffset;

            if (expresult != 0)
            {
                ulong temp = ((ulong)expresult & offsetMasks[resultOffset]) << 8;
                final |= ((ulong)expresult & offsetMasks[resultOffset]) << 8;
            }
            return new Real64(final);
        }

        private static Real64 MulDecDecS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSDecS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracFrac(Real64 r1, Real64 r2)
        {
            (long demoniator1, long numerator1) = r1.GetSegments();
            (long demoniator2, long numerator2) = r2.GetSegments();
            long finaldenominator = demoniator1 * demoniator2;
            long finalnumerator = numerator1 * numerator2;
            int offset = 1 + ((finalnumerator < 0) ? CountMinimumByteSegments((ulong)~finalnumerator) : CountMinimumByteSegments((ulong)finalnumerator));
            ulong h = ((ulong)finaldenominator << (8 * offset)) | ((ulong)finalnumerator << 8) | ((ulong)offset ^ flagFraction);
            return new Real64(h);
        }

        private static Real64 MulFracFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracNFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracNFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracNFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracDFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracDFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();

        #endregion

        #region Divide Operations

        private static RealBinaryOperator[] DivOperators = new RealBinaryOperator[] {
            DivDecDec,
            DivDecDecS,
            DivDecSDecS,
            DivDecRoot,
            DivDecSRoot,
            DivRootRoot,
            DivDecRootS,
            DivDecSRootS,
            DivRootRootS,
            DivRootSRootS,
            DivDecFrac,
            DivDecSFrac,
            DivRootFrac,
            DivRootSFrac,
            DivFracFrac,
            DivDecFracN,
            DivDecSFracN,
            DivRootFracN,
            DivRootSFracN,
            DivFracFracN,
            DivFracNFracN,
            DivDecFracD,
            DivDecSFracD,
            DivRootFracD,
            DivRootSFracD,
            DivFracFracD,
            DivFracNFracD,
            DivFracDFracD,
            DivDecFracS,
            DivDecSFracS,
            DivRootFracS,
            DivRootSFracS,
            DivFracFracS,
            DivFracNFracS,
            DivFracDFracS,
            DivFracSFracS
        };

        private static Real64 DivDecDec(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecDecS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSDecS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootRoot(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSRootS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracFrac(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracNFracN(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracNFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracNFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracDFracD(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracDFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracSFracS(Real64 r1, Real64 r2) => throw new NotImplementedException();

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Real64 IntegerAdd(in Real64 r1, in Real64 r2)
        {
            long result = (long)r1.unum + (long)r2.unum;
            result = result ^ 3u;
            return new Real64((ulong)result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Real64 IntegerSub(in Real64 r1, in Real64 r2)
        {
            long result = ((long)r1.unum >> 8) - ((long)r2.unum >> 8);
            result = (result << 8) ^ 1u;
            return new Real64((ulong)result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Real64 IntegerMul(in Real64 r1, in Real64 r2)
        {
            long result = ((long)r1.unum >> 8) * ((long)r2.unum >> 8);
            result = (result << 8) ^ 1u;
            return new Real64((ulong)result);
        }

        public static Real64 operator +(Real64 r1, Real64 r2)
        {
            if (r1.IsInteger && r2.IsInteger)
            {
                return IntegerAdd(r1, r2);
            }

            int r1header = r1.header >> 4;
            int r2header = r2.header >> 4;
            if (r1header > r2header)
            {
                RealBinaryOperator op = AddOperators[Pairing(r1header, r2header)];
                return op(r2, r1);
            }
            else
            {
                RealBinaryOperator op = AddOperators[Pairing(r2header, r1header)];
                return op(r1, r2);
            }
        }

        public static Real64 operator -(Real64 r1, Real64 r2)
        {
            if (r1.IsInteger && r2.IsInteger)
            {
                return IntegerSub(r1, r2);
            }

            int r1header = r1.header >> 4;
            int r2header = r2.header >> 4;
            if (r1header > r2header)
            {
                RealBinaryOperator op = SubOperators[Pairing(r1header, r2header)];
                return op(r2, r1);
            }
            else
            {
                RealBinaryOperator op = SubOperators[Pairing(r2header, r1header)];
                return op(r1, r2);
            }
        }

        public static Real64 operator *(Real64 r1, Real64 r2)
        {
            if (r1.IsInteger && r2.IsInteger)
            {
                return IntegerMul(r1, r2);
            }

            int r1header = r1.header >> 4;
            int r2header = r2.header >> 4;
            if (r1header > r2header)
            {
                RealBinaryOperator op = MulOperators[Pairing(r1header, r2header)];
                return op(r2, r1);
            }
            else
            {
                RealBinaryOperator op = MulOperators[Pairing(r2header, r1header)];
                return op(r1, r2);
            }
        }

        public static Real64 operator /(Real64 r1, Real64 r2)
        {
            if (r1.IsInteger && r2.IsInteger)
            {
               // return IntegerDiv(r1, r2);
            }

            int r1header = r1.header >> 4;
            int r2header = r2.header >> 4;
            if (r1header > r2header)
            {
                RealBinaryOperator op = DivOperators[Pairing(r1header, r2header)];
                return op(r2, r1);
            }
            else
            {
                RealBinaryOperator op = DivOperators[Pairing(r2header, r1header)];
                return op(r1, r2);
            }
        }

        public Real64 NthRoot(int n)
        {
            ulong h = this.unum | flagRootNumbers;
            return new Real64(h);
        }

        public Real64 Sqrt()
        {
            ulong h = this.unum | flagRootNumbers;
            return new Real64(h);
        }
    }
}
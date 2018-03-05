namespace System
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

        private static Real64 AddDecDec(in Real64 r1, in Real64 r2)
        {
            long result;
            long expresult;
            int resultOffset = 1;
            if (r1.IsInteger && r2.IsInteger)
            {
                return IntegerAdd((long)r1.unum, (long)r2.unum);
            }

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
                if (d1exp < d2exp)
                {
                    long diff = d2exp - d1exp;
                    long shiftedInput = d2mantissa * (long)PowersOf10[diff];
                    result = d1mantissa + shiftedInput;
                    expresult = d1exp;
                }
                else
                {
                    long diff = d1exp - d2exp;
                    long shiftedInput = d1mantissa * (long)PowersOf10[diff];
                    result = d2mantissa + shiftedInput;
                    expresult = d2exp;
                }
                /*
                long rebase = (d1exp < d2exp) ? d1exp : d2exp;
                long diff1 = base1 - rebase;
                long diff2 = Math.Max(base2 - rebase, 0);
                long pairdiff = base2 - base1;
                result = d1mantissa * (long)PowersOf10[diff1] + d2mantissa * (long)PowersOf10[diff2];
                expresult = rebase - pairdiff + diff2;
                */
                resultOffset += (expresult < 0) ? CountMinimumByteSegments((ulong)~expresult) : CountMinimumByteSegments((ulong)expresult);
            }
            ulong final = ((ulong)result << (resultOffset * 8)) | ((uint)resultOffset & 0xF);

            if (expresult != 0)
            {
                final |= ((ulong)expresult & offsetMasks[resultOffset]) << 8;
            }
            return new Real64(final);
        }

        private static Real64 AddDecDecS(in Real64 r1, in Real64 r2)
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

        private static Real64 AddDecRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();

        private static Real64 AddDecSDecS(in Real64 r1, in Real64 r2)
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

        private static Real64 AddDecSRoot(in Real64 r1, in Real64 r2)
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
        private static Real64 AddDecSRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddDecSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddRootSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();

        private static Real64 AddFracFrac(in Real64 r1, in Real64 r2)
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

        private static Real64 AddFracFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 AddFracFracD(in Real64 r1, in Real64 r2)
        {
            throw new NotImplementedException();
        }

        private static Real64 AddFracFracS(in Real64 r1, in Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }

        private static Real64 AddFracNFracN(in Real64 r1, in Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }

        private static Real64 AddFracNFracD(in Real64 r1, in Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }

        private static Real64 AddFracNFracS(in Real64 r1, in Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }

        private static Real64 AddFracDFracD(in Real64 r1, in Real64 r2)
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

        private static Real64 AddFracDFracS(in Real64 r1, in Real64 r2)
        {
            return AddDecDec((Real64)r1.ToDecimal(), (Real64)r2.ToDecimal());
        }
        private static Real64 AddFracSFracS(in Real64 r1, in Real64 r2)
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

        private static Real64 SubDecDec(in Real64 r1, in Real64 r2)
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

        private static Real64 SubDecDecS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSDecS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubDecSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubRootSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracFrac(in Real64 r1, in Real64 r2)
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

        private static Real64 SubFracFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracNFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracNFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracNFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracDFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracDFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 SubFracSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();

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

        private static Real64 MulDecDec(in Real64 r1, in Real64 r2)
        {
            long result;
            int resultOffset = 1;
            (long d1mantissa, long d1exp) = r1.GetSegments();
            (long d2mantissa, long d2exp) = r2.GetSegments();

            result = d1mantissa * d2mantissa;
            long expresult = (d1exp + d2exp - 1);
            //int expected = CountDigits(d1mantissa) + CountDigits(d2mantissa);
            //int adjust = CountDigits(result) - expected;
            //expresult = d1exp + d2exp + adjust;
            resultOffset += (expresult < 0) ? CountMinimumByteSegments((ulong)~expresult) : CountMinimumByteSegments((ulong)expresult);
            ulong final = ((ulong)result << (resultOffset * 8)) | (uint)resultOffset;

            if (expresult != 0)
            {
                ulong temp = ((ulong)expresult & offsetMasks[resultOffset]) << 8;
                final |= ((ulong)expresult & offsetMasks[resultOffset]) << 8;
            }
            return new Real64(final);
        }

        private static Real64 MulDecDecS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSDecS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulDecSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulRootSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracFrac(in Real64 r1, in Real64 r2)
        {
            (long demoniator1, long numerator1) = r1.GetSegments();
            (long demoniator2, long numerator2) = r2.GetSegments();
            long finaldenominator = demoniator1 * demoniator2;
            long finalnumerator = numerator1 * numerator2;
            int offset = 1 + ((finalnumerator < 0) ? CountMinimumByteSegments((ulong)~finalnumerator) : CountMinimumByteSegments((ulong)finalnumerator));
            ulong h = ((ulong)finaldenominator << (8 * offset)) | ((ulong)finalnumerator << 8) | ((ulong)offset ^ flagFraction);
            return new Real64(h);
        }

        private static Real64 MulFracFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracNFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracNFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracNFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracDFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracDFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 MulFracSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();

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

        private static Real64 DivDecDec(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecDecS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSDecS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivDecSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootRoot(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSRootS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivRootSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracNFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracNFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracNFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracDFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracDFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
        private static Real64 DivFracSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Real64 IntegerAdd(in long r1, in long r2)
        {
            long result = (r1 + r2) ^ 3u;
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

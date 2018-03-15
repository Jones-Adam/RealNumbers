namespace System
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public readonly partial struct Real64
    {

        //#region Addition Operations

        private static readonly ulong[] offsetMasks = new ulong[8] {
            0x00_00_00_00_00_00_00_00uL,  // offset 0
            0x00_00_00_00_00_00_00_FFuL,  // offset 1
            0x00_00_00_00_00_00_FF_FFuL,  // offset 2
            0x00_00_00_00_00_FF_FF_FFuL,  // offset 3
            0x00_00_00_00_FF_FF_FF_FFuL,  // offset 4
            0x00_00_00_FF_FF_FF_FF_FFuL,  // offset 5
            0x00_00_FF_FF_FF_FF_FF_FFuL,  // offset 6
            0x00_FF_FF_FF_FF_FF_FF_FFuL,  // offset 7
        };

        private static (long mantissa, long exp) AddDecDec(long mantissa1, long exp1, long mantissa2, long exp2)
        {
            if (exp1 == exp2)
            {
                long mantissa = mantissa1 + mantissa2;
                return (mantissa1 + mantissa2, exp1);
            }
            else
            {
                long result;
                if (exp1 < exp2)
                {
                    long diff = exp2 - exp1;
                    long shiftedInput = mantissa2 * (long)PowersOf10[diff];
                    result = mantissa1 + shiftedInput;
                    return (result, exp1);
                }
                else
                {
                    long diff = exp1 - exp2;
                    long shiftedInput = mantissa1 * (long)PowersOf10[diff];
                    result = mantissa2 + shiftedInput;
                    return (result, exp2);
                }
            }
        }

        /*
private static Real64 AddDecDec(in Real64 r1, in Real64 r2)
{
    int offsetr1 = r1.GetOffset();
    int offsetr2 = r2.GetOffset();

    long d1exp = (offsetr1 == 1) ? 1 : (long)r1.unum << sizeMantissa[offsetr1] >> sizeMantissa[offsetr1] + 8;
    long d2exp = (offsetr2 == 1) ? 1 : (long)r2.unum << sizeMantissa[offsetr2] >> sizeMantissa[offsetr2] + 8;

    if (d1exp == d2exp)
    {
        long d1mantissa = (long)r1.unum >> (offsetr1 << 3);
        long d2mantissa = (long)r2.unum >> (offsetr2 << 3);
        long result = d1mantissa + d2mantissa;
        ulong final = ((ulong)result << (offsetr1 * 8)) | (r1.unum & offsetMasks[offsetr1 + 1]);
        return new Real64(final);
    }
    else
    {
        long result;
        long expresult;
        long d1mantissa = (long)r1.unum >> (offsetr1 << 3);
        long d2mantissa = (long)r2.unum >> (offsetr2 << 3);
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
        int resultOffset = Math.Max(offsetr1, offsetr2);
        //resultOffset += (expresult < 0) ? CountMinimumByteSegments((ulong)~expresult) : CountMinimumByteSegments((ulong)expresult);

        ulong final = ((ulong)result << (resultOffset * 8)) | (((ulong)expresult & offsetMasks[resultOffset]) << 8) | ((uint)resultOffset & 0xF);
        return new Real64(final);
    }
}

private static Real64 AddDecDecS(in Real64 r1, in Real64 r2)
{
    if (r2.IsMaxValue)
    {
        return r2;
    }

    return AddDecDec(r1, ConvertSpecialToDecimal(r2));
}

private static Real64 AddDecFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 AddDecFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 AddDecFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 AddDecFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();

private static Real64 AddDecSDecS(in Real64 r1, in Real64 r2)
{
    if (r1.IsPositiveMaxValue)
    {
        return (r2.IsNegativeMaxValue) ? new Real64(0ul) : r1;
    }
    if (r1.IsNegativeMaxValue)
    {
        return (r2.IsPositiveMaxValue) ? new Real64(0ul) : r1;
    }
    if (r2.IsMaxValue)
    {
        return r2;
    }

    // TODO: if sign is different and same special return 0;

    return AddDecDec(ConvertSpecialToDecimal(r1), ConvertSpecialToDecimal(r2));
}
private static Real64 AddDecSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 AddDecSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 AddDecSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 AddDecSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();

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
    SubDecFrac,
    SubDecSFrac,
    SubFracFrac,
    SubDecFracN,
    SubDecSFracN,
    SubFracFracN,
    SubFracNFracN,
    SubDecFracD,
    SubDecSFracD,
    SubFracFracD,
    SubFracNFracD,
    SubFracDFracD,
    SubDecFracS,
    SubDecSFracS,
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
private static Real64 SubDecFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 SubDecFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 SubDecFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 SubDecFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 SubDecSDecS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 SubDecSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 SubDecSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 SubDecSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 SubDecSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();

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
    MulDecFrac,
    MulDecSFrac,
    MulFracFrac,
    MulDecFracN,
    MulDecSFracN,
    MulFracFracN,
    MulFracNFracN,
    MulDecFracD,
    MulDecSFracD,
    MulFracFracD,
    MulFracNFracD,
    MulFracDFracD,
    MulDecFracS,
    MulDecSFracS,
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
    DivDecFrac,
    DivDecSFrac,
    DivFracFrac,
    DivDecFracN,
    DivDecSFracN,
    DivFracFracN,
    DivFracNFracN,
    DivDecFracD,
    DivDecSFracD,
    DivFracFracD,
    DivFracNFracD,
    DivFracDFracD,
    DivDecFracS,
    DivDecSFracS,
    DivFracFracS,
    DivFracNFracS,
    DivFracDFracS,
    DivFracSFracS
};

private static Real64 DivDecDec(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecDecS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecSDecS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecSFrac(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecSFracN(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecSFracD(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
private static Real64 DivDecSFracS(in Real64 r1, in Real64 r2) => throw new NotImplementedException();
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
*/
        private static (long mantissa, long exp) SubDecDec(long mantissa1, long exp1, long mantissa2, long exp2)
        {
            if (exp1 == exp2)
            {
                long mantissa = mantissa1 - mantissa2;
                return (mantissa1 - mantissa2, exp1);
            }
            else
            {
                long result;
                if (exp1 < exp2)
                {
                    long diff = exp2 - exp1;
                    long shiftedInput = mantissa2 * (long)PowersOf10[diff];
                    result = mantissa1 - shiftedInput;
                    return (result, exp1);
                }
                else
                {
                    long diff = exp1 - exp2;
                    long shiftedInput = mantissa1 * (long)PowersOf10[diff];
                    result = shiftedInput - mantissa2;
                    return (result, exp2);
                }
            }
        }

        private static (long mantissa, long exp) MulDecDec(long mantissa1, long exp1, long mantissa2, long exp2)
        {
            decimal result = new decimal(mantissa1) * new decimal(mantissa2);
            (long mantissa, long exp) = ExtractDecimal(result);
            //long mantissa = mantissa1 / mantissa2;
            return (mantissa, (exp1 + exp2 - exp));
        }

        private static (long mantissa, long exp) DivDecDec(long mantissa1, long exp1, long mantissa2, long exp2)
        {
            decimal result = new decimal(mantissa1) / new decimal(mantissa2);
            (long mantissa, long exp) = ExtractDecimal(result);
            //long mantissa = mantissa1 / mantissa2;
            return (mantissa, (exp1 - exp2 + exp));
        }

        public static Real64 operator +(Real64 r1, Real64 r2)
        {
            if (r1.IsDecimal)
            {
                if (r2.IsDecimal)
                {
                    int offsetr1 = r1.GetOffset();
                    int offsetr2 = r2.GetOffset();

                    long d1mantissa = (long)r1.unum >> 4 + (offsetr1 << 3);
                    long d2mantissa = (long)r2.unum >> 4 + (offsetr2 << 3);

                    long d1exp = (long)r1.unum << sizeMantissa[offsetr1] >> sizeMantissa[offsetr1] + 4;
                    long d2exp = (long)r2.unum << sizeMantissa[offsetr2] >> sizeMantissa[offsetr2] + 4;

                    (long result, long expresult) = AddDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                    int resultOffset = Math.Max(offsetr1, offsetr2);

                    ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                    return new Real64(final);
                }
                else
                {
                    int offsetr1 = r1.GetOffset();
                    long d1mantissa = (long)r1.unum >> 4 + (offsetr1 << 3);
                    long d1exp = (long)r1.unum << sizeMantissa[offsetr1] >> sizeMantissa[offsetr1] + 4;

                    // convert r2 to decimal
                    (long d2mantissa, long d2exp) = r2.ConvertToDecimalSegments();

                    (long result, long expresult) = AddDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                    int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                    ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                    return new Real64(final);
                }
            }
            else if (r2.IsDecimal)
            {
                // convert r1 to decimal
                int offsetr2 = r2.GetOffset();
                long d2mantissa = (long)r2.unum >> 4 + (offsetr2 << 3);
                long d2exp = (long)r2.unum << sizeMantissa[offsetr2] >> sizeMantissa[offsetr2] + 4;

                // convert r2 to decimal
                (long d1mantissa, long d1exp) = r1.ConvertToDecimalSegments();

                (long result, long expresult) = AddDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                return new Real64(final);
            }
            else
            {
                // fraction add
                int offsetr1 = r1.GetOffset();
                int offsetr2 = r2.GetOffset();

                ulong r1segment1flag = (r1.unum >> (4 + (offsetr1 << 3))) & segmentFlags;
                ulong r1segment2 = (offsetr1 == 0) ? 0 : (r1.unum >> 4) & offsetMasks[offsetr1];
                ulong r2segment1flag = (r2.unum >> (4 + (offsetr2 << 3))) & segmentFlags;
                ulong r2segment2 = (offsetr2 == 0) ? 0 : (r2.unum >> 4) & offsetMasks[offsetr2];
                //get flag bits

                if (r1segment2 == r2segment2)
                {
                    //denominators are the same
                    if ((r1segment1flag & r2segment1flag) == flagInteger)
                    {
                        // addable
                        long numerator1 = (long)r1.unum >> 8 + (offsetr1 << 3);
                        long numerator2 = (long)r2.unum >> 8 + (offsetr2 << 3);
                        long result = numerator1 + numerator2;
                        ulong final;
                        if (r1segment2 == 0)
                        {
                            final = ((ulong)result << 8) | flagFraction;
                        }
                        else
                        {
                            final = ((ulong)result << (8 + (offsetr1 * 8))) | ((r1segment2 & offsetMasks[offsetr1]) << 4) | ((uint)offsetr1 & 0xF | flagFraction);
                        }
                        return new Real64(final);
                    }
                    else
                    {
                        (long d1mantissa, long d1exp) = r1.ConvertToDecimalSegments();
                        (long d2mantissa, long d2exp) = r2.ConvertToDecimalSegments();
                        (long result, long expresult) = AddDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                        int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                        ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                        return new Real64(final);
                    }
                }
                else
                {
                    // denomiators different

                    //TODO: optimisation when lcd is computable

                    (long d1mantissa, long d1exp) = r1.ConvertToDecimalSegments();
                    (long d2mantissa, long d2exp) = r2.ConvertToDecimalSegments();
                    (long result, long expresult) = AddDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                    int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                    ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                    return new Real64(final);

                }

                throw new NotImplementedException();
            }
        }

        public static Real64 operator -(Real64 r1, Real64 r2)
        {
            if (r1.IsDecimal)
            {
                if (r2.IsDecimal)
                {
                    int offsetr1 = r1.GetOffset();
                    int offsetr2 = r2.GetOffset();

                    long d1mantissa = (long)r1.unum >> 4 + (offsetr1 << 3);
                    long d2mantissa = (long)r2.unum >> 4 + (offsetr2 << 3);

                    long d1exp = (long)r1.unum << sizeMantissa[offsetr1] >> sizeMantissa[offsetr1] + 4;
                    long d2exp = (long)r2.unum << sizeMantissa[offsetr2] >> sizeMantissa[offsetr2] + 4;

                    (long result, long expresult) = SubDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                    int resultOffset = Math.Max(offsetr1, offsetr2);

                    ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                    return new Real64(final);
                }
                else
                {
                    int offsetr1 = r1.GetOffset();
                    long d1mantissa = (long)r1.unum >> 4 + (offsetr1 << 3);
                    long d1exp = (long)r1.unum << sizeMantissa[offsetr1] >> sizeMantissa[offsetr1] + 4;

                    // convert r2 to decimal
                    (long d2mantissa, long d2exp) = r2.ConvertToDecimalSegments();

                    (long result, long expresult) = SubDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                    int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                    ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                    return new Real64(final);
                }
            }
            else if (r2.IsDecimal)
            {
                // convert r1 to decimal
                int offsetr2 = r2.GetOffset();
                long d2mantissa = (long)r2.unum >> 4 + (offsetr2 << 3);
                long d2exp = (long)r2.unum << sizeMantissa[offsetr2] >> sizeMantissa[offsetr2] + 4;

                // convert r2 to decimal
                (long d1mantissa, long d1exp) = r1.ConvertToDecimalSegments();

                (long result, long expresult) = SubDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                return new Real64(final);
            }
            else
            {
                // convert to decimal
                (long d1mantissa, long d1exp) = r1.ConvertToDecimalSegments();
                (long d2mantissa, long d2exp) = r2.ConvertToDecimalSegments();

                (long result, long expresult) = SubDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                return new Real64(final);
            }
        }

        public static Real64 operator *(Real64 r1, Real64 r2)
        {
            if (r1.IsDecimal)
            {
                if (r2.IsDecimal)
                {
                    int offsetr1 = r1.GetOffset();
                    int offsetr2 = r2.GetOffset();

                    long d1mantissa = (long)r1.unum >> 4 + (offsetr1 << 3);
                    long d2mantissa = (long)r2.unum >> 4 + (offsetr2 << 3);

                    long d1exp = (long)r1.unum << sizeMantissa[offsetr1] >> sizeMantissa[offsetr1] + 4;
                    long d2exp = (long)r2.unum << sizeMantissa[offsetr2] >> sizeMantissa[offsetr2] + 4;

                    (long result, long expresult) = MulDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                    int resultOffset = Math.Max(offsetr1, offsetr2);

                    ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                    return new Real64(final);
                }
                else
                {
                    int offsetr1 = r1.GetOffset();
                    long d1mantissa = (long)r1.unum >> 4 + (offsetr1 << 3);
                    long d1exp = (long)r1.unum << sizeMantissa[offsetr1] >> sizeMantissa[offsetr1] + 4;

                    // convert r2 to decimal
                    (long d2mantissa, long d2exp) = r2.ConvertToDecimalSegments();

                    (long result, long expresult) = MulDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                    int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                    ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                    return new Real64(final);
                }
            }
            else if (r2.IsDecimal)
            {
                // convert r1 to decimal
                int offsetr2 = r2.GetOffset();
                long d2mantissa = (long)r2.unum >> 4 + (offsetr2 << 3);
                long d2exp = (long)r2.unum << sizeMantissa[offsetr2] >> sizeMantissa[offsetr2] + 4;

                // convert r2 to decimal
                (long d1mantissa, long d1exp) = r1.ConvertToDecimalSegments();

                (long result, long expresult) = MulDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                return new Real64(final);
            }
            else
            {
                // convert to decimal
                (long d1mantissa, long d1exp) = r1.ConvertToDecimalSegments();
                (long d2mantissa, long d2exp) = r2.ConvertToDecimalSegments();

                (long result, long expresult) = MulDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                return new Real64(final);
            }
        }

        public static Real64 operator /(Real64 r1, Real64 r2)
        {
            if (r1.IsDecimal)
            {
                if (r2.IsDecimal)
                {
                    int offsetr1 = r1.GetOffset();
                    int offsetr2 = r2.GetOffset();

                    long d1mantissa = (long)r1.unum >> 4 + (offsetr1 << 3);
                    long d2mantissa = (long)r2.unum >> 4 + (offsetr2 << 3);

                    long d1exp = (long)r1.unum << sizeMantissa[offsetr1] >> sizeMantissa[offsetr1] + 4;
                    long d2exp = (long)r2.unum << sizeMantissa[offsetr2] >> sizeMantissa[offsetr2] + 4;

                    (long result, long expresult) = DivDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                    int resultOffset = Math.Max(offsetr1, offsetr2);

                    ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                    return new Real64(final);
                }
                else
                {
                    int offsetr1 = r1.GetOffset();
                    long d1mantissa = (long)r1.unum >> 4 + (offsetr1 << 3);
                    long d1exp = (long)r1.unum << sizeMantissa[offsetr1] >> sizeMantissa[offsetr1] + 4;

                    // convert r2 to decimal
                    (long d2mantissa, long d2exp) = r2.ConvertToDecimalSegments();

                    (long result, long expresult) = DivDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                    int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                    ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                    return new Real64(final);
                }
            }
            else if (r2.IsDecimal)
            {
                // convert r1 to decimal
                int offsetr2 = r2.GetOffset();
                long d2mantissa = (long)r2.unum >> 4 + (offsetr2 << 3);
                long d2exp = (long)r2.unum << sizeMantissa[offsetr2] >> sizeMantissa[offsetr2] + 4;

                // convert r2 to decimal
                (long d1mantissa, long d1exp) = r1.ConvertToDecimalSegments();

                (long result, long expresult) = DivDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                return new Real64(final);
            }
            else
            {
                // convert to decimal
                (long d1mantissa, long d1exp) = r1.ConvertToDecimalSegments();
                (long d2mantissa, long d2exp) = r2.ConvertToDecimalSegments();

                (long result, long expresult) = DivDecDec(d1mantissa, d1exp, d2mantissa, d2exp);

                int resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)-expresult) : CountMinimumByteSegments((ulong)expresult);

                ulong final = ((ulong)result << (4 + (resultOffset * 8))) | (((ulong)expresult & offsetMasks[resultOffset]) << 4) | ((uint)resultOffset & 0xF);
                return new Real64(final);
            }
        }
    }
}

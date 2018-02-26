﻿namespace System
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    struct DoubleConvert
    {
        [FieldOffset(0)]
        public ulong Bits;
        [FieldOffset(0)]
        public double Double;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public readonly struct Real64 : IEquatable<Real64>, IComparable<Real64>, IRealNumber
    {
        // Flags
        private const byte flagSpecial =        0b11000;
        private const byte flagPositiveLimit =  0b11010;
        private const byte flagNegativeLimit =  0b11011;
        private const byte flagFraction =       0b_0100_0000;
        private const byte flagInteger =        0b_1111_1110;
        private const byte flagDecimal =        0b_1110_0000;
        private const byte flagRootNumbers =    0b_0010_0000;
        private const byte flagOffset =         0b00111;
        private const byte flagFullOffset =     0b00111000;


        private const ulong specialPositiveInfinity = 0x00_00_00_00_00_00_00_58uL;
        private const ulong specialNegativeInfinity = 0x00_00_00_00_00_00_00_78uL;
        private const ulong specialNaN =              0x00_00_00_00_00_00_00_D8uL;
        private const ulong specialPI =               0xC0_00_00_00_00_00_01_18uL;
        private const ulong speciale =                0xC0_00_00_00_00_00_02_18uL;

        private const ulong OffsetMask = 0x00_00_00_00_00_00_00_07uL;

        private static readonly ulong[] offsetMasks = new ulong[8] {
            0x00_00_00_00_00_00_00_00uL,  // offset 0
            0x00_00_00_00_00_00_00_00uL,  // offset 1
            0x00_00_00_00_00_00_00_FFuL,  // offset 2
            0x00_00_00_00_00_00_FF_FFuL,  // offset 3
            0x00_00_00_00_00_FF_FF_FFuL,  // offset 4
            0x00_00_00_00_FF_FF_FF_FFuL,  // offset 5
            0x00_00_00_FF_FF_FF_FF_FFuL,  // offset 6
            0x00_00_FF_FF_FF_FF_FF_FFuL,  // offset 7
        };

        private static readonly ulong[] signMasks = new ulong[8] {
            0x00_00_00_00_00_00_00_00uL,  // offset 0
            0x00_00_00_00_00_00_00_00uL,  // offset 1
            0x00_00_00_00_00_00_00_80uL,  // offset 2
            0x00_00_00_00_00_00_80_00uL,  // offset 3
            0x00_00_00_00_00_80_00_00uL,  // offset 4
            0x00_00_00_00_80_00_00_00uL,  // offset 5
            0x00_00_00_80_00_00_00_00uL,  // offset 6
            0x00_00_80_00_00_00_00_00uL,  // offset 7
        };

        private const ulong maxlong = 0x7FFFFFFFFFFFFFF;

        [FieldOffset(0)] readonly ulong unum;
        [FieldOffset(0)] readonly byte header;

        public static readonly Real64 PositiveInfinity = new Real64(specialPositiveInfinity);

        public static readonly Real64 NegativeInfinity = new Real64(specialNegativeInfinity);

        public static readonly Real64 NaN = new Real64(specialNaN);

        public bool IsInfinity => (IsPositiveInfinity || IsNegativeInfinity);

        public bool IsPositiveInfinity => unum == specialPositiveInfinity;

        public bool IsNegativeInfinity => unum == specialNegativeInfinity;

        public bool IsNaN => (unum & specialNaN) == specialNaN;

        public bool IsDecimal => (header & flagDecimal) == 0;

        public bool IsInteger => (header & flagInteger) == 0;

        public bool IsFraction => (header & flagFraction) == flagFraction;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetOffset()
        {
            return (int)(this.unum & OffsetMask);
        }

        private (long part1, long part2) GetSegments()
        {
            int offset = GetOffset();
            long p1 = (long)this.unum >> (offset << 3);
            if (offset == 1)
            {
                return (p1, 0);
            }

            ulong p2 = (this.unum >> 8) & offsetMasks[offset];
            if ((p2 & signMasks[offset]) != 0)
            {
                p2 = p2 | ~offsetMasks[offset];
            }
            return (p1, (long)p2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetSegmentOne()
        {
            return (long)this.unum >> (GetOffset() * 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetSegmentTwo()
        {
            int offset = GetOffset();
            ulong p2 = (this.unum & offsetMasks[offset]) >> 5;
            if ((p2 & signMasks[offset]) != 0)
            {
                p2 = p2 | ~offsetMasks[offset];
            }
            return (long)p2;
        }

        private static int CountMinimumByteSegments(ulong input)
        {
            if ((input >> 8) == 0) return 1;
            if ((input >> 16) == 0) return 2;
            if ((input >> 24) == 0) return 3;
            if ((input >> 32) == 0) return 4;
            if ((input >> 40) == 0) return 5;
            if ((input >> 48) == 0) return 6;
            if ((input >> 56) == 0) return 7;
            return 8;
        }

        private static int CountLeadingZeros(ulong input)
        {
            if (input == 0) return 64;

            int n = 1;

            if ((input >> 32) == 0) { n = n + 32; input = input << 32; }
            if ((input >> 48) == 0) { n = n + 16; input = input << 16; }
            if ((input >> 56) == 0) { n = n + 8; input = input << 8; }
            if ((input >> 60) == 0) { n = n + 4; input = input << 4; }
            if ((input >> 62) == 0) { n = n + 2; input = input << 2; }
            n = n - (int)(input >> 63);

            return n;
        }

        private bool TrySetSegmentOne(long number)
        {
            return false;
        }

        private bool TrySetSegmentTwo(long number)
        {
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Real64(ulong num)
        {
            this.header = 0;
            this.unum = num;
        }
        // int fiveBits = normal & 0x1f;
        // and for the reverse:
        // int normal = fiveBits < 16 ? fiveBits : fiveBits | -32;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Real64 IntegerAdd(Real64 r1, Real64 r2)
        {
            long result = (long)r1.unum + (long)r2.unum;
            result = result ^ 3u;
            return new Real64((ulong)result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Real64 IntegerSub(Real64 r1, Real64 r2)
        {
            long result = ((long)r1.unum >> 8) - ((long)r2.unum >> 8);
            result = (result << 8) ^ 1u;
            return new Real64((ulong)result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Real64 IntegerMul(Real64 r1, Real64 r2)
        {
            long result = ((long)r1.unum >> 8) * ((long)r2.unum >> 8);
            result = (result << 8) ^ 1u;
            return new Real64((ulong)result);
        }

        private static Real64 DecimalAdd(Real64 r1, Real64 r2)
        {
            long result;
            long expresult;
            int resultOffset;
            (long d1mantissa, long d1exp) = r1.GetSegments();
            (long d2mantissa, long d2exp) = r2.GetSegments();
            if (d1exp == d2exp)
            {
                result = d1mantissa + d2mantissa;
                expresult = d1exp;
                resultOffset = (expresult < 0) ? CountMinimumByteSegments((ulong)~expresult) : CountMinimumByteSegments((ulong)expresult);
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
                final |= (ulong)expresult << 8;
            }
            return new Real64(final);
        }

        private static Real64 DecimalSub(Real64 r1, Real64 r2)
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

        private static Real64 DecimalMul(Real64 r1, Real64 r2)
        {
            long result;
            long expresult;
            int resultOffset = 1;
            (long d1mantissa, long d1exp) = r1.GetSegments();
            (long d2mantissa, long d2exp) = r2.GetSegments();

            result = d1mantissa * d2mantissa;
            expresult = d1exp + d2exp;
            ulong final = ((ulong)result << (resultOffset * 8)) | (uint)resultOffset;

            if (expresult != 0)
            {
                ulong temp = ((ulong)expresult & offsetMasks[resultOffset]) << 8;
                final |= ((ulong)expresult & offsetMasks[resultOffset]) << 8;
            }
            return new Real64(final);
        }

        private static Real64 FractionAdd(Real64 r1, Real64 r2)
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

        private static Real64 FractionSub(Real64 r1, Real64 r2)
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

        private static Real64 FractionMul(Real64 r1, Real64 r2)
        {
            (long demoniator1, long numerator1) = r1.GetSegments();
            (long demoniator2, long numerator2) = r2.GetSegments();
            long finaldenominator = demoniator1 * demoniator2;
            long finalnumerator = numerator1 * numerator2;
            int offset = 1 + ((finalnumerator < 0) ? CountMinimumByteSegments((ulong)~finalnumerator) : CountMinimumByteSegments((ulong)finalnumerator));
            ulong h = ((ulong)finaldenominator << (8 * offset)) | ((ulong)finalnumerator << 8) | ((ulong)offset ^ flagFraction);
            return new Real64(h);
        }

        public static Real64 operator +(Real64 r1, Real64 r2)
        {
            if (r1.IsInteger && r2.IsInteger)
            {
                return IntegerAdd(r1, r2);
            }

            if (r1.IsInfinity || r2.IsInfinity)
            {
                return r2;
            }

            if (r1.IsDecimal && r2.IsDecimal)
            {
                return DecimalAdd(r1, r2);
            }

            if (r1.IsFraction && r2.IsFraction)
            {
                return FractionAdd(r1, r2);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static Real64 operator -(Real64 r1, Real64 r2)
        {
            if (r1.IsInteger && r2.IsInteger)
            {
                return IntegerSub(r1, r2);
            }

            if (r1.IsInfinity || r2.IsInfinity)
            {
                return r2;
            }

            if (r1.IsDecimal && r2.IsDecimal)
            {
                return DecimalSub(r1, r2);
            }

            if (r1.IsFraction && r2.IsFraction)
            {
                return FractionSub(r1, r2);
            }


            long result = 0;
            long expresult;
            int resultOffset;
            (long d1mantissa, long d1exp) = r1.GetSegments();
            (long d2mantissa, long d2exp) = r2.GetSegments();
            if (d1exp == d2exp)
            {
                result = d1mantissa - d2mantissa;
                expresult = d1exp;
                int sigbit2 = (result < 0) ? CountLeadingZeros((ulong)~result) / 8 : CountLeadingZeros((ulong)result) / 8;
                resultOffset = Math.Max(8 - sigbit2, 1);
            }
            else
            {
                result = 0;
                expresult = 0;
                resultOffset = 1;
            }
            if (expresult == 0)
            {
                ulong final = ((ulong)result << (resultOffset * 8)) |(uint)resultOffset;
                return new Real64(final);
            }
            else
            {
                expresult = (long)((ulong)expresult & offsetMasks[resultOffset]) << 8;
                ulong final = ((ulong)result << (resultOffset * 8)) | (uint)resultOffset;
                return new Real64(final);
            }
        }

        public static Real64 operator *(Real64 r1, Real64 r2)
        {
            if (r1.IsInteger && r2.IsInteger)
            {
                return IntegerMul(r1, r2);
            }

            if (r1.IsInfinity)
            {
                return r1;
            }

            if (r2.IsInfinity)
            {
                return r2;
            }

            if (r1.IsDecimal && r2.IsDecimal)
            {
                return DecimalMul(r1, r2);
            }

            if (r1.IsFraction && r2.IsFraction)
            {
                return FractionMul(r1, r2);
            }

            throw new NotImplementedException();
        }

        public static unsafe long DoubleToInt64Bits(double value)
        {
            return *((long*)&value);
        }

        public static unsafe double Int64BitsToDouble(long value)
        {
            return *((double*)&value);
        }

        public static implicit operator Real64(int v)
        {
            return FromInt(v);
        }

        public static explicit operator Real64(double d)
        {
            return FromDouble(d);
        }

        public static explicit operator Real64(decimal d)
        {
            return FromDecimal(d);
        }

        public static Real64 FromInt(int num)
        {
            ulong h = ((ulong)num << 8) | 1UL;
            return new Real64(h);
        }

        public int ToInteger()
        {
            long x = GetSegmentOne();

            // TODO: check for overflow

            return unchecked((int)x);
        }

        public static Real64 FromDouble(double num)
        {
            if (double.IsNaN(num))
            {
                return Real64.NaN;
            }
            if (double.IsPositiveInfinity(num))
            {
                return Real64.PositiveInfinity;
            }
            if (double.IsNegativeInfinity(num))
            {
                return Real64.NegativeInfinity;
            }
            decimal d = new decimal(num);
            return FromDecimal(d);
            /*
            long refernce = BitConverter.DoubleToInt64Bits(num);

            DoubleConvert dc = default;
            dc.Double = num;
            bool negative = dc.Bits >> 63 == 1;
            int exponent = (int)((dc.Bits >> 52) & 0x7ffL);
            ulong mantissa = dc.Bits & 0xfffffffffffffL;


            Real64 r = default;

            // Translate the double into sign, exponent and mantissa.
            long bits = DoubleToInt64Bits(num);

            // Note that the shift is sign-extended, hence the test against -1 not 1
            //bool negative = (bits < 0);
            //int exponent = (int)((bits >> 52) & 0x7ffL);
            //long mantissa = bits & 0xfffffffffffffL;

            Byte[] mm = BitConverter.GetBytes(mantissa);
            Array.Reverse(mm);


            // Subnormal numbers; exponent is effectively one higher,
            // but there's no extra normalisation bit in the mantissa
            if (exponent == 0)
            {
                exponent++;
            }
            // Normal numbers; leave exponent as it is but add extra
            // bit to the front of the mantissa
            else
            {
                mantissa = mantissa | (1L << 52);
            }

            exponent -= 1023;

            if (mantissa == 0)
            {
                r.unum = 0;
                return r;
            }

            return r;
            */
        }

        public double ToDouble()
        {
            if (IsPositiveInfinity)
            {
                return double.PositiveInfinity;
            }
            else if (IsNegativeInfinity)
            {
                return double.NegativeInfinity;
            }
            else if (IsNaN)
            {
                return double.NaN;
            }
            return Decimal.ToDouble(ToDecimal());

            /*
            long exponent = GetSegmentOne();
            long mantissa = GetSegmentTwo();

            return float_from_int(mantissa);
            */
        }

        ulong float_from_int(long x)
        {
            if (x == 0)
                return 0; // 0 is a special case because it has no 1 bits

            // Save the sign bit of the input and take the absolute value of the input.
            ulong signBit = 0;
            ulong absX = (ulong)x;
            if (x < 0)
            {
                signBit = 0x8000000000000000uL;
                absX = (uint)-x;
            }

            // Shift the input left until the high order bit is set to form the mantissa.
            // Form the floating exponent by subtracting the number of shifts from 158.
            uint exponent = 158;
            while ((absX & 0x80000000) == 0)
            {
                exponent--;
                absX <<= 1;
            }

            // compute mantissa
            ulong mantissa = absX >> 8;

            // Assemble the float from the sign, mantissa, and exponent.
            return signBit | (exponent << 23) | (mantissa & 0x7fffff);
        }

        public static Real64 FromDecimal(decimal num)
        {
            const int SignMask = unchecked((int)0x80000000);
            int[] dparts = decimal.GetBits(num);
            // lo, mid, hi (number) +  flags with scale power 10
            if (dparts[2] > 0)
            {
                throw new OverflowException();
            }
            ulong h = (uint)dparts[0] | ((ulong)dparts[1] << 32);

            int f = dparts[3];
            if ((f & SignMask) != 0)
            {
                h = ~h + 1;
            }

            f = (f & ~SignMask) >> 16;
            f = -f & 0xFF;

            h = h << 16 | ((ulong)f << 8) | 2ul;
            return new Real64(h);
        }

        private long Pow(long exp)
        {
            long x = 1;
            for (int i = 0; i < exp; i++)
            {
                x *= 10;
            }

            return x;
        }

        public Decimal ToDecimal()
        {
            if (IsFraction)
            {
                (long demoniator, long numerator) = GetSegments();
                decimal d = new decimal(numerator) / new decimal(demoniator);
                return d;
            }

            (long mantissa, long exp) = GetSegments();
            bool negative = false;
            if (mantissa < 0)
            {
                mantissa = -mantissa;
                negative = true;
            }
            if (exp < 0)
            {
                exp = -exp;
            }
            else
            {
                mantissa = mantissa * Pow(exp);
            }

            int low = (int)mantissa;
            int mid = (int)(mantissa >> 32);
            return new Decimal(low, mid, 0, negative, (byte)exp);
        }

        public static Real64 FromFraction(int numerator, int denominator)
        {
            long part2 = numerator;
            int sigbit2 = (part2 < 0) ? CountLeadingZeros((ulong)~part2) / 8 : CountLeadingZeros((ulong)part2) / 8;
            int numeratorOffset = Math.Max(8 - sigbit2, 2);
            part2 = (long)((ulong)part2 & offsetMasks[numeratorOffset]);

            long part1 = denominator;
            int sigbit1 = (part1 < 0) ? CountLeadingZeros((ulong)~part1) / 8 : CountLeadingZeros((ulong)part1) / 8;

            //TODO: check if demoninator will fit
            ulong h = ((ulong)part1 << (numeratorOffset * 8)) | ((ulong)(uint)part2 << 8) | ((ulong)numeratorOffset | flagFraction);
            return new Real64(h);
        }

        public (long numerator, long denominator) ToFraction()
        {
            (long denominator, long numerator) = GetSegments();
            return (numerator, denominator);
        }

        public static Real64 FromDecimalComponents(int manissa, int exponent)
        {
            long part1 = exponent;
            long part2 = manissa;
            int sigbit1 = (part1 < 0) ? CountLeadingZeros((ulong)~part1) / 8 : CountLeadingZeros((ulong)part1) / 8;
            int sigbit2 = (part2 < 0) ? CountLeadingZeros((ulong)~part2) / 8 : CountLeadingZeros((ulong)part2) / 8;
            uint offset = (uint)(-1 + sigbit1);
            ulong mask = (ulong)(1L << ((int)offset * 8)) - 1L;
            ulong part2set = (ulong)part2 & mask;
            long part1set = (part1 << ((int)offset * 8)) & (long)maxlong;
            ulong num = (ulong)part1set ^ part2set ^ ((ulong)offset << 59);
            return new Real64(num);
        }

        public bool Equals(Real64 other)
        {
            if (this.IsNaN || other.IsNaN)
            {
                return false;
            }

            return this.unum.Equals(other.unum);
        }

        public override bool Equals(object obj)
        {
            return (obj is Real64 r) && this.Equals(r);
        }

        public override int GetHashCode()
        {
            return unum.GetHashCode();
        }

        public int CompareTo(Real64 other)
        {
            throw new NotImplementedException();
        }

        public Real64 Sqrt()
        {
            ulong h = this.unum | flagRootNumbers;
            return new Real64(h);
        }
    }
}

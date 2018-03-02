namespace System
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

    class SpecialNumbers
    {
        public string Symbol;
        public ulong Decimal;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public readonly partial struct Real64 : IEquatable<Real64>, IComparable<Real64>, IRealNumber
    {
        private delegate Real64 RealBinaryOperator(Real64 r1, Real64 r2);

        // Flags
        private const byte flagFraction =       0b_0100_0000;
        private const byte flagFractionN =      0b_0101_0000;
        private const byte flagFractionD =      0b_0110_0000;
        private const byte flagFractionS =      0b_0111_0000;
        private const byte flagInteger =        0b_1111_1110;
        private const byte flagDecimal =        0b_1110_0000;
        private const byte flagRootNumbers =    0b_0010_0000;


        private const ulong specialPositiveInfinity = 0x00_00_00_00_00_00_02_10uL;
        private const ulong specialNegativeInfinity = 0x00_00_00_00_00_00_03_10uL;
        private const ulong specialNaN =              0x00_00_00_00_00_00_04_10uL;
        private const ulong specialPI =               0x00_00_00_00_00_00_10_11uL;
        private const ulong specialE =                0x00_00_00_00_00_00_20_11uL;

        private const ulong OffsetMask = 0x00_00_00_00_00_00_00_07uL;

        private static readonly SpecialNumbers[] specialNumbers = new SpecialNumbers[] {
            new SpecialNumbers{Symbol= "(nul)", Decimal = 0ul }, // offset 0
            new SpecialNumbers{Symbol= "\u03C0", Decimal = 0x1C_92_97_24_36_DA_01_02ul },  
            new SpecialNumbers{Symbol= "\u212f", Decimal = 0x18_B8_FE_3A_54_2F_01_02ul },
        };

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

        public static readonly Real64 PI = new Real64(specialPI);

        public static readonly Real64 E = new Real64(specialE);

        public bool IsInfinity => (IsPositiveInfinity || IsNegativeInfinity);

        public bool IsPositiveInfinity => unum == specialPositiveInfinity;

        public bool IsNegativeInfinity => unum == specialNegativeInfinity;

        public bool IsNaN => (unum & specialNaN) == specialNaN;

        public bool IsDecimal => (header & flagDecimal) == 0;

        public bool IsInteger => (header & flagInteger) == 0;

        public bool IsFraction => (header & flagFraction) == flagFraction;

        private bool IsSpecialDecimal => (header >> 4) == 1; 

        private static Real64 ConvertSpecialToDecimal(Real64 r)
        {
            ulong specindex = r.unum >> 12;
            return new Real64(specialNumbers[specindex].Decimal);
        }

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

        private static int Pairing(int max, int min)
        {
            return ((max * (max + 1)) / 2) + min;
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

        private static readonly ulong[] PowersOf10 = new ulong[20] 
            {1, 10, 100, 1000, 10000, 100000,
                 1000000, 10000000, 100000000, 1000000000,
                10000000000, 100000000000, 1000000000000,
                10000000000000, 100000000000000, 1000000000000000,
                10000000000000000, 100000000000000000, 1000000000000000000,
                10000000000000000000
            };

        private static readonly int[] MultiplyDeBruijnBitPosition2 = new int[64] {
                0, // change to 1 if you want bitSize(0) = 1
                1,  2, 53,  3,  7, 54, 27, 4, 38, 41,  8, 34, 55, 48, 28,
                62,  5, 39, 46, 44, 42, 22,  9, 24, 35, 59, 56, 49, 18, 29, 11,
                63, 52,  6, 26, 37, 40, 33, 47, 61, 45, 43, 21, 23, 58, 17, 10,
                51, 25, 36, 32, 60, 20, 57, 16, 50, 31, 19, 15, 30, 14, 13, 12
            }; // table taken from http://chessprogramming.wikispaces.com/De+Bruijn+Sequence+Generator

        private static readonly ulong multiplicator = 0x022fdd63cc95386dUL;

        private static int IntegerLogBase2(ulong v)
        {
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v |= v >> 32;
            // at this point you could also use popcount to find the number of set bits.
            // That might well be faster than a lookup table because you prevent a 
            // potential cache miss
            if (v == unchecked((ulong)-1)) return 64;
            v++;
            return MultiplyDeBruijnBitPosition2[(ulong)(v * multiplicator) >> 58];
        }

        private static int CountDigits(ulong v)
        {
            int t = (IntegerLogBase2(v) + 1) * 1233 >> 12;
            return 1 + t - ((v < PowersOf10[t]) ? 1 : 0);
        }

        private static int CountDigits(long v)
        {
            return (v < 0) ? CountDigits((ulong)(-v)) : CountDigits((ulong)v);
        }

        public static Real64 FromDecimal(decimal num)
        {
            const int SignMask = unchecked((int)0x80000000);

            num = decimal.Round(num, 18);
            int[] dparts = decimal.GetBits(num);
            // lo, mid, hi (number) +  flags with scale power 10

            ulong h = (uint)dparts[0] | ((ulong)dparts[1] << 32);
            int hdigits = CountDigits(h);

            int f = dparts[3];
            if ((f & SignMask) != 0)
            {
                h = ~h + 1;
            }

            f = (f & ~SignMask) >> 16;

            decimal positivenum = num < 0 ? decimal.Negate(num) : num;
            int c = CountDigits((ulong)(int)positivenum);

            f = (f > 0) ? hdigits - f : c;
            //f = (f > 0 && c == 0) ? -(f-1) : c; // -f & 0xFF;
            f = f & 0xFF;
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
                (long denominator, long numerator) = GetSegments();
                decimal dnum, dnum2;

                //TODO: add in support for specials
                if ((header & flagFractionN) == flagFractionN) // numerator
                {
                    numerator = numerator >> 4;
                    Real64 temp = new Real64(specialNumbers[numerator].Decimal);
                    dnum = temp.ToDecimal();
                }
                else
                {
                    dnum = new decimal(numerator);
                }

                if ((header & flagFractionD) == flagFractionD) // denominator
                {
                    denominator = denominator >> 4;
                    Real64 temp = new Real64(specialNumbers[denominator].Decimal);
                    dnum2 = temp.ToDecimal();
                }
                else
                {
                    dnum2 = new decimal(denominator);
                }



                decimal d = dnum / dnum2;
                return d;
            }

            if (IsSpecialDecimal)
            {
                return (ConvertSpecialToDecimal(this)).ToDecimal();
            }

            (long mantissa, long exp) = GetSegments();
            bool negative = false;
            if (mantissa < 0)
            {
                mantissa = -mantissa;
                negative = true;
            }
            int c = CountDigits((ulong)mantissa);
            if (exp < 0)
            {
                exp = -exp + c;
            }
            else
            {
                //mantissa = mantissa * Pow(exp);

                exp = c - exp;
                if (exp < 0)
                    exp = 0;
            }

            int low = (int)mantissa;
            int mid = (int)(mantissa >> 32);
            return new Decimal(low, mid, 0, negative, (byte)exp);
        }

        public static Real64 FromFraction(int numerator, Real64 denominator)
        {
            if (denominator.IsInteger)
            {
                return FromFraction(numerator, denominator.ToInteger());
            }

            if (denominator.IsSpecialDecimal)
            {
                long part2 = numerator;
                int sigbit2 = (part2 < 0) ? CountLeadingZeros((ulong)~part2) / 8 : CountLeadingZeros((ulong)part2) / 8;
                int numeratorOffset = Math.Max(8 - sigbit2, 2);
                part2 = (long)((ulong)part2 & offsetMasks[numeratorOffset]);
                long part1 = denominator.GetSegmentOne();
                int sigbit1 = (part1 < 0) ? CountLeadingZeros((ulong)~part1) / 8 : CountLeadingZeros((ulong)part1) / 8;

                //TODO: check if demoninator will fit
                ulong h = ((ulong)part1 << (numeratorOffset * 8)) | ((ulong)(uint)part2 << 8) | ((ulong)numeratorOffset | flagFractionD);
                return new Real64(h);

            }

            if (denominator.IsFraction)
            {
                throw new NotSupportedException("Cannot create a fraction from a fraction");
            }

            throw new NotSupportedException("Can only create a fraction from integers or specials");
        }

        public static Real64 FromFraction(Real64 numerator, int denominator)
        {
            if (numerator.IsInteger)
            {
                return FromFraction(numerator.ToInteger(), denominator);
            }

            if (numerator.IsSpecialDecimal)
            {
                long part2 = numerator.GetSegmentOne();
                int sigbit2 = (part2 < 0) ? CountLeadingZeros((ulong)~part2) / 8 : CountLeadingZeros((ulong)part2) / 8;
                int numeratorOffset = Math.Max(8 - sigbit2, 2);
                part2 = (long)((ulong)part2 & offsetMasks[numeratorOffset]);
                long part1 = denominator;
                int sigbit1 = (part1 < 0) ? CountLeadingZeros((ulong)~part1) / 8 : CountLeadingZeros((ulong)part1) / 8;

                //TODO: check if demoninator will fit
                ulong h = ((ulong)part1 << (numeratorOffset * 8)) | ((ulong)(uint)part2 << 8) | ((ulong)numeratorOffset | flagFractionN);
                return new Real64(h);

            }

            if (numerator.IsFraction)
            {
                throw new NotSupportedException("Cannot create a fraction from a fraction");
            }

            throw new NotSupportedException("Can only create a fraction from integers or specials");
        }

        public static Real64 FromFraction(Real64 numerator, Real64 denominator)
        {
            if (denominator.IsInteger)
            {
                return FromFraction(numerator, denominator.ToInteger());
            }
            if (numerator.IsInteger)
            {
                return FromFraction(numerator.ToInteger(), denominator);
            }

            if (denominator.IsSpecialDecimal && numerator.IsSpecialDecimal)
            {
                long part2 = numerator.GetSegmentOne();
                long part1 = denominator.GetSegmentOne();
                ulong h = ((ulong)part1 << 16) | ((ulong)part2 << 8) | ((ulong)2 | flagFractionS);
                return new Real64(h);
            }

            throw new NotSupportedException("Can only create a fraction from integers or specials");
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
    }
}

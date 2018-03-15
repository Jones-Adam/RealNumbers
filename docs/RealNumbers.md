# Real Number

A real number is storable as a fixed size representation.  Real32, Real64 and Real128 store the number to differing degrees of precision depending upon the type and size of the number

## Real Number precision

A real numer can hold a floating point decimal number (similiar to the Decimal Type), an integer, a well known irrational number (e.g. pi, e), a fractional number, a radical (algebraic root), an algebraic sin, an algebraic log.

The available precision of the number depends upon the type of information stored.  Additionally decimal precision depends upon the size of the number being stored.

Real numbers are typically able to store exact representations for a much great range of numbers than decimal or float/double types which can greatly minimise rounding errors.

Real numbers use segemnt sizes to split the information to be stored into its component parts.  By using segment sizes rather than a fixed number allows Reals to provide precision adapted storage over a very large range.

### Real32

Real32 can store the following number ranges:
Integers from -8,388,608 to 8,388,607

Segment sizes for Real32 are a nibble (4 bits).

### Real64

Real64 can store the following number ranges:
Integers from -36,028,797,018,963,968 to 36,028,797,018,963,967

Segment sizes for Real64 are 1 byte (8 bits)

### Real128

Real64 can store the following number ranges:
Integers from -2^119 to 2^119 - 1

Segment sizes for Real128 are 2 bytes (16 bits)

## Binary Format

Each real type consists of a header of 4 bits.  The first bit of this header indicates if the number is a fraction or a decimal.  The remaining 3 bits determine the offset of the split in the remaining content.

Bit1 | Bit2 | Bit3 | Bit4 | Decimal | Purpose
---- | ---- | ---- | ---- | ---- | ----
x|x|x|0|0|Decimal
x|x|x|1|1|Fraction

### Decimal Numbers

Decimal numbers consist of two parts.  The mantissa and a decimaloffset.  The offset to the mantissa itself is given by the header in number of segments.
the mantissa is a signed integer in twos complement representation.  The decimaloffset is a signed integer in twos complement representation indicating the offset of the the right most digit counting from the first decimal place after the decimal point.

i.e 0.3 is stored as 3 mantissa and 0 decimaloffset.  

0.112 is stored as 112 mantissa and a -2 decimaloffset.

3 x 10^3 is stored as 3 mantissa and a 4 decimaloffset.

### Fractions

Fractions consist of two fractional components, the numerator and the denominator.  The offset to the numerator is given by the header in number of segments.

The numerator is a signed integer, the denominator unsigned.

A fractional component can be one of several types of number:
- An integer
- A known special number
- An integer root
- An integer log

The type of number is determined by 4 reserved bits.

Bit1 | Bit2 | Bit3 | Bit4 | Decimal | Purpose
---- | ---- | ---- | ---- | ---- | ----
0|0|0|0|0|Integer
0|0|0|1|1|Known Special
0|0|1|0|2|Root Radical
0|0|1|1|3|Log Radical
0|1|0|0|4|Fractional Number, no specials
0|1|0|1|5|Fraction with numerator special
0|1|1|0|6|Fraction with denominator special
0|1|1|1|7|Fraction with both special
1|x|x|x|8-15|-Reserved-


#### Integers

A signed twos complement number

#### Known special numbers

Special numbers consist of well known numbers and radicals

Binary|Hex|Decimal|Description
---|---|---|---
0010|2|2|PositiveMaxValue - the largest real number
0011|3|3|NegativeMaxValue - the largest negative real number
0100|4|4|PI
0101|5|5|e
0110|6|6|The golden Ratio (https://en.wikipedia.org/wiki/Golden_ratio)
0111|7|7|Plastic Number (https://en.wikipedia.org/wiki/Plastic_number)
1000|8|8|Euler's constant (https://en.wikipedia.org/wiki/Euler–Mascheroni_constant)
1011|B|11|Apéry's constant (https://en.wikipedia.org/wiki/Apéry%27s_constant)

#### Integer root

4 bits represent the root index.  The remaining digits represent the radical content

#### Integer log

4 bits represent the base.  The remaining digits represent the log content

## Supported Operations

Addition
Subraction

## TODO

- IConvertable Interface
- ToString
- Parse/TryParse
 - implement bits from https://msdn.microsoft.com/en-us/library/microsoft.solverfoundation.common.rational(v=vs.93).aspx
 - Implement bits from https://msdn.microsoft.com/en-us/library/system.numerics.biginteger(v=vs.110).aspx
 - implement bits from https://codereview.stackexchange.com/questions/95681/rational-number-implementation
 - 
# Real Number

A real number is represented by the Real64 type

## Binary Format

A Real64 is an 8 byte type divided into a header (5 bits) and content (59 bits)

### Header Values (8 bits) 

Bit1 | Bit2 | Bit3 | Bit4 | Decimal | Purpose
---- | ---- | ---- | ---- | ---- | ----
0|0|0|0|0|Decimal
0|0|0|1|1|Special Decimal
0|0|1|0|2|Root numbers
0|0|1|1|3|Special Root numbers
0|1|0|0|4|Fractional Number, no specials
0|1|0|1|5|Fraction with numerator special
0|1|1|0|6|Fraction with denominator special
0|1|1|1|7|Fraction with both special
1|x|x|x|8-15|Reserved

Bits 5-8 indicate the position of the split between the components of the number type.

### Special Numbers

Binary|Hex|Decimal|Description
---|---|---|---
10|1|2|PositiveInfinity
11|2|3|NegativeInfinity
100|3|4|NaN
1000|8|8|PI
1001|9|9|e

### Fractions

Fractions are represented by 2 signed integers.
The header values in bits 5-8 provide the split between the two numbers in number of bytes counting from the right.

i.e. if the header value is 3  The first 16 (3 * 8) bits from the right after the header will be the numerator and the remaining (40 bits) will be the denominator

The field is always split according to the number of bytes necessary to represent the numerator.

### Decimals

Decimals are represented by 1-2 signed integers.
If the window is 0 or 1 then only 1 signed integer is used to represent the number
If the window is > 1 then 2 signed integers are used to represent the number with the first part refering to the exponent and the second part the mantissa
The first integer from the right represents the exponent of the number, the rest of the number the mantissa.


### Root Numbers

Root numbers are represented by an unsigned root number (1 byte) and a signed radicaland


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
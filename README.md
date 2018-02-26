# RealNumbers
This is an open source c# library for representing Real numbers.  In this library we used Real numbers to refer to the set of numbers which includes whole numbers (integers), rational numbers (fractions), decimal numbers

The library is build for both .netstandard2.0 and .net 4.7.1

Current CI build status is: [![Build status](https://ci.appveyor.com/api/projects/status/ixcmdlx1qaf2qner/branch/master?svg=true)](https://ci.appveyor.com/project/Jones-Adam/realnumbers/branch/master)

RealNumbers is covered under the terms of the [MIT/X11](LICENSE.md) license. You may therefore link to it and use it in both opensource and proprietary software projects.

## Installation Instructions

Eventually this will be published as a nuget package and that will be the recommended way to obtain this library.

To manually build the library from source you can use Visual Studio or using dotnet on the command line.


## Using the library

There will be 3 types provided publically.

Real64, Real128 and Real256 and an interface IRealNumber (to allow generic code to perform arithmetic operations on arbitary types).

The Real types can be used in substitute for any native number type on the .net platform and can be easily converted between them.

Real numbers can be used to represent any rational number exactly and irrational numbers to a number of decimal points which depends on the width of the Real type chosen.  Additionally Real numbers represent some irrational numbers as thier algebraic counterparts allowing final decimal conversion to be delayed.

When representing decimal numbers Real numbers, unlike the Decimal type, uses a variable precision range.  This allows Real numbers to provide a balance between number range and precision in a given fixed data size.  

For more information on Real numbers see the corresponding wiki page.

All typical arithmetic functions are supported along with a number of extended fuctions

## Library Goals

 - Provide a set of fixed size types which can represent commonly used Real numbers
 - Fulfill that gap where for a given project float/double rounding errors hurt, decimal is too slow or doesn't fit the range and BigInteger/BigRational is overkill
 - Performance at the very least comparable to the Decimal type on direct arithmetic while providing extended functionality.


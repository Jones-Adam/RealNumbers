using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    class RealFormatProvider : IFormatProvider, ICustomFormatter
    {
        public RealFormatProvider()
        {

        }

        /// <inheritdoc />
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (arg is Real64 number)
            {
                Real64 num = (Real64)arg;
                if (format.Substring(0, 1).Equals("G", StringComparison.OrdinalIgnoreCase))
                {
                    //General Format
                    return num.ToGeneralString(format, provider);
                }
                else if (format.Substring(0, 1).Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    return "Currency Format";
                }
                else if (format.Substring(0, 1).Equals("D", StringComparison.OrdinalIgnoreCase))
                {
                    return "Decimal Format";              
                }
                else if (format.Substring(0, 1).Equals("E", StringComparison.OrdinalIgnoreCase))
                {
                    return "Scientific Format";
                }
                else if (format.Substring(0, 1).Equals("F", StringComparison.OrdinalIgnoreCase))
                {
                    return "Fixed point Format";
                }
                else if (format.Substring(0, 1).Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    return "Number Format";
                }
                else if (format.Substring(0, 1).Equals("P", StringComparison.OrdinalIgnoreCase))
                {
                    return "Percent Format";
                }
                else if (format.Substring(0, 1).Equals("R", StringComparison.OrdinalIgnoreCase))
                {
                    return "RoundTrip Format";
                }
                else if (format.Substring(0, 1).Equals("P", StringComparison.OrdinalIgnoreCase))
                {
                    return "Hexadecimal Format";
                }
                else
                {
                    throw new FormatException("Unknown Format qualifier");
                }
            }
            else
            {
                if (arg is IFormattable)
                {
                    return ((IFormattable)arg).ToString(format, provider);
                }
                else if (arg != null)
                {
                    return arg.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}

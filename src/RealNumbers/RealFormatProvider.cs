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
                string fmtString = string.Empty;
                if (format.Length > 1)
                {
                    if (int.TryParse(format.Substring(1), out int precision))
                    {
                        fmtString = "N" + precision.ToString();
                    }
                    else
                    {
                        fmtString = format.Substring(1);
                    }
                }

                if (format.Substring(0, 1).Equals("D", StringComparison.OrdinalIgnoreCase))
                {
                    return "A number";
                }
                else
                {
                    return "";
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

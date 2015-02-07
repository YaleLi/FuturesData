using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class DoubleUtility
    {
        public static bool Equals(double value1, double value2)
        {
            long lValue1 = BitConverter.DoubleToInt64Bits(value1);
            long lValue2 = BitConverter.DoubleToInt64Bits(value2);

            // If the signs are different, return false except for +0 and -0. 
            if ((lValue1 >> 63) != (lValue2 >> 63))
            {
                if (lValue1 == lValue2)
                    return true;

                return false;
            }

            long diff = Math.Abs(lValue1 - lValue2);

            if (diff <= (long)2)
                return true;

            return false;
        }

        public static double Parse(string input, IFormatProvider formatProvider, double defaultValue)
        {
            if (string.IsNullOrEmpty(input))
            {
                return defaultValue;
            }

            double result = defaultValue;
            if (!Double.TryParse(input, NumberStyles.Any, formatProvider, out result))
            {
                result = defaultValue;
            }
            return result;
        }
    }
}

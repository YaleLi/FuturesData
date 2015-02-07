using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataType
{
    public sealed class GlobalDefinition
    {
        public static string DateFormat { get; } = "yyyyMMdd";
        public static IFormatProvider FormatProvider { get; } = new CultureInfo("zh-Hans");

        private GlobalDefinition() { }
    }
}

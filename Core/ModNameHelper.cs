using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.Core
{
    public static class ModNameHelper
    {
        public static string GetFriendlyModName(string fileName)
        {
            var name = System.Text.RegularExpressions.Regex.Replace(
                fileName,
                @"([-_ (]*\d{4,}.*$)|(\s*[\(\[]?v?\d+(\.\d+)*[\)\]]?$)",
                string.Empty,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
            name = name.Replace('_', ' ').Replace('-', ' ').Trim();
            name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            return string.IsNullOrWhiteSpace(name) ? fileName : name;
        }
    }
}

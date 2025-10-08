using System.Text.RegularExpressions;

namespace Modding_Assistant.Core.Utilities
{
    /// <summary>
    /// Class for convenient user-friendly mod name converting
    /// </summary>
    public static class ModNameHelper
    {
        private const string VersionPattern = @"\s*[\(\[]?v?\d+(\.\d+)*[\)\]]?$";
        private const string YearPattern = @"[-_ (]*\d{4,}.*$";

        private static readonly string CombinedPattern = $"({YearPattern})|({VersionPattern})";

        private static readonly Regex NameCleanerRegex =
            new(CombinedPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Converts technical mod file names to user-friendly display names
        /// by removing versions, years, and normalizing formatting
        /// </summary>
        /// <returns>Clean, user-friendly mod name</returns>
        public static string GetFriendlyModName(string fileName)
        {
            var name = NameCleanerRegex.Replace(fileName, string.Empty);

            name = name.Replace('_', ' ').Replace('-', ' ').Trim();
            name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            return string.IsNullOrWhiteSpace(name) ? fileName : name;
        }
    }
}

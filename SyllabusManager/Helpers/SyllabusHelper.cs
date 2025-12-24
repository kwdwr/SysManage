using System.Text.RegularExpressions;

namespace SyllabusManager.App.Helpers
{
    public static class SyllabusHelper
    {
        public static string NormalizeCode(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            // Remove all whitespace
            return Regex.Replace(input, @"\s+", "").ToUpperInvariant();
        }
    }
}

using System.Text.RegularExpressions;

namespace Test
{
    public static class Validation
    {
        public static bool CheckForDecimal(this string input)
        {
            Regex reg = new Regex(@"^[0-9][0-9\.]+\d$");
            Match match = reg.Match(input);
            return string.IsNullOrEmpty(match.Value) ? false : true;
        }

        public static bool CheckForInt(this string input)
        {
            Regex reg = new Regex(@"(?<![-.])\b[0-9]+\b(?!\.[0-9])");
            Match match = reg.Match(input);
            return string.IsNullOrEmpty(match.Value) ? false : true;
        }
    }
}

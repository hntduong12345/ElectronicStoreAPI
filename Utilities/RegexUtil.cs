using System.Text.RegularExpressions;

namespace Helper
{
    public static class RegexUtil
    {
        public static bool IsEmail(string input)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(input);
            return match.Success;
        }
    }
}
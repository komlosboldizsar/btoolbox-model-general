using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BToolbox.Helpers
{
    public static class IpHelpers
    {

        private static string REGEX_PART_STR_IP = "(25[0-5]|2[0-4][0-9]|[01]?[0-9]?[0-9])";
        public static string REGEX_STR_IP = $"^{REGEX_PART_STR_IP}\\.{REGEX_PART_STR_IP}\\.{REGEX_PART_STR_IP}\\.{REGEX_PART_STR_IP}$";
        public static Regex REGEX_IP = new(REGEX_STR_IP, RegexOptions.Compiled);

        public static bool CheckIP(this string ipAddressStr)
            => REGEX_IP.IsMatch(ipAddressStr);

    }
}

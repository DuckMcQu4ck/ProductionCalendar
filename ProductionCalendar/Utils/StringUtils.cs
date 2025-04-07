using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionCalendar.Utils
{
    internal static class StringUtils
    {
        public static string BuildUrl(string baseUrl, string method, string token, params string[] args)
            => $"{baseUrl}{method}{token}/{string.Join("/", args)}/json";

        public static string AppendParams(string url, params (string, string)[] args)
            => $"{url}?{string.Join("&", args.Select(a => $"{Uri.EscapeDataString(a.Item1)}={Uri.EscapeDataString(a.Item2)}"))}";

        public static string FormatDay(DateOnly date)
            => date.ToString("dd.MM.yyyy");

        public static string FormatMonth(DateOnly date)
            => date.ToString("MM.yyyy");
    }

}

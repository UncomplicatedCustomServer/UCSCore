using System;
using System.Collections.Generic;

namespace UncomplicatedCustomServerCore.Extensions
{
    internal static class TimeSpanExtension
    {
        public static string RenderDate(this TimeSpan time)
        {
            List<string> result = [];

            int days = time.Days;

            if (days > 365)
            {
                int years = days / 365;
                if (years > 1)
                    result.Add($"{years} {Plugin.Instance.Translation.Years}");
                else
                    result.Add($"{years} {Plugin.Instance.Translation.Year}");
            }

            days -= 365 * (days / 365);

            if (days > 30)
            {
                int months = days / 30;
                if (months > 1)
                    result.Add($"{months} {Plugin.Instance.Translation.Months}");
                else
                    result.Add($"{months} {Plugin.Instance.Translation.Month}");
            }

            days -= 30 * (days / 30);

            if (days > 0)
            {
                if (days > 1)
                    result.Add($"{days} {Plugin.Instance.Translation.Days}");
                else
                    result.Add($"{days} {Plugin.Instance.Translation.Day}");
            }

            if (time.Hours > 1 || time.Hours is 0)
                result.Add($"{time.Hours} {Plugin.Instance.Translation.Hours}");
            else
                result.Add($"{time.Hours} {Plugin.Instance.Translation.Hour}");

            if (time.Minutes > 1 || time.Minutes is 0)
                result.Add($"{time.Minutes} {Plugin.Instance.Translation.Minutes}");
            else
                result.Add($"{time.Minutes} {Plugin.Instance.Translation.Minute}");

            return string.Join(", ", result);
        }
    }
}
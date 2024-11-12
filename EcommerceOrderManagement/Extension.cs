using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ecommerce
{
    public static class Extension
    {
        private static readonly Regex EmailRegex = new Regex(
       @"^[a-zA-Z0-9._%+]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.IgnoreCase);

        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            return EmailRegex.IsMatch(email);
        }
    }
}

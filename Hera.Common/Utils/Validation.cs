using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Hera.Common.Utils
{
    public static class Validation
    {
        /// <summary>
        /// Test if the provided string is a valid email
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns></returns>
        public static bool IsValidEmail(string value)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");

            return regex.IsMatch(value);
        }

        /// <summary>
        /// Test if the provided string is a valid date
        /// </summary>
        /// <param name="value">Date string to test</param>
        /// <returns>bool</returns>
        public static bool IsValidDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }
    }
}

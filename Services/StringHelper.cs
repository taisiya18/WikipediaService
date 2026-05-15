using System;
using System.Linq;

namespace project_2.Services
{
    public static class StringHelper
    {
        public static string NormalizeString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input = input.Trim();

            if (input.Contains(','))
            {
                input = string.Join(",", input.Split(',').Select(genre => genre.Trim()));
            }

            return input;
        }
    }
}

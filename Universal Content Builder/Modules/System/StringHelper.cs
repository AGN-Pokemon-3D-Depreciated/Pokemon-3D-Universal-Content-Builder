using System;

namespace Universal_Content_Builder.Modules.System
{
    public static class StringHelper
    {
        /// <summary>
        /// Splits a string into substrings that are based on the characters in an array and gets a 32-bit integer that represents the total number of elements in all the dimensions of the <see cref="System.Array"/>.
        /// </summary>
        /// <param name="fullString">The full string to count the number of index.</param>
        /// <param name="seperator">The seperator to split the string.</param>
        public static int SplitCount(this string fullString, char seperator = '|')
        {
            return fullString.Contains(seperator.ToString()) ? fullString.Split(seperator).Length : 1;
        }

        /// <summary>
        /// Splits a string into substrings that are based on the characters in an array and gets the Char object at a specified position in the current String object.
        /// </summary>
        /// <param name="fullString">Represents text as a sequence of UTF-16 code units.</param>
        /// <param name="valueIndex">Gets the Char object at a specified position in the current String object.</param>
        /// <param name="seperator">A character that delimits the substrings in this string, an empty array that contains no delimiters, or null. </param>
        public static string GetSplit(this string fullString, int valueIndex, char seperator = '|')
        {
            int expectedSplitCount = fullString.SplitCount(seperator);

            if (expectedSplitCount == 1)
                return fullString;
            else if (valueIndex < expectedSplitCount)
                return fullString.Split(seperator)[valueIndex];
            else
                return fullString.Split(seperator)[expectedSplitCount - 1];
        }

        /// <summary>
        /// Determines whether two String objects have the same value.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        public static new bool Equals(this object a, object b)
        {
            if (string.Equals(a, b))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determines whether two String objects have the same value.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        public static bool Equals(this string a, params string[] b)
        {
            foreach (string item in b)
            {
                if (string.Equals(a, item, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
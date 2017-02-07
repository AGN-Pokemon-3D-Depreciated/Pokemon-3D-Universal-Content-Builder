using System;

namespace Modules.System
{
    public static class StringHelper
    {
        public static int SplitCount(this string fullString, char seperator = '|')
        {
            return fullString.Contains(seperator.ToString()) ? fullString.Split(seperator).Length : 1;
        }

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

        public static new bool Equals(this object a, object b)
        {
            if (string.Equals(a, b))
                return true;
            else
                return false;
        }

        public static bool Equals(this string a, params string[] b)
        {
            foreach (string item in b)
            {
                if (string.Equals(a, item, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static string RandomString(int length)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
                result[i] = Convert.ToChar(new Random().Next(33, 126));

            return string.Concat(result);
        }
    }
}
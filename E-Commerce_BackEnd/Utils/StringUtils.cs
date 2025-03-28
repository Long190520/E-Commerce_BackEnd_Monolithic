﻿using System.Text;
using System.Text.RegularExpressions;

namespace E_Commerce_BackEnd.Utils
{
    public static class StringUtils
    {
        public static string GenerateRandomToken(int? maxLength = 32)
        {
            var builder = new StringBuilder();
            var random = new Random();
            char ch;
            for (int i = 0; i < maxLength; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private static readonly Regex trimmer = new Regex("\\s\\s+");

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool HasAnyChar(this string value)
        {
            return value != null && !string.IsNullOrEmpty(value);
        }

        public static bool HasNonSpaceChar(this string value)
        {
            return value != null && !string.IsNullOrWhiteSpace(value);
        }

        public static int CountChar(this string word, char countableLetter)
        {
            int num = 0;
            foreach (char c in word)
            {
                if (countableLetter == c)
                {
                    num++;
                }
            }

            return num;
        }

        public static bool IsMatch(this string text, string regText)
        {
            Regex regex = new Regex(regText);
            return regex.IsMatch(text);
        }

        public static string TrimAndRemoveWhiteSpace(this string text)
        {
            if (text == null)
            {
                return null;
            }

            return trimmer.Replace(text.Trim(), " ");
        }

        public static string ToLower(this bool val)
        {
            return val.ToString().ToLower();
        }

        public static string SubStrIndex(this string value, int startIndex)
        {
            if (value == null)
            {
                return value;
            }

            if (startIndex < 0 || startIndex > value.Length - 1)
            {
                startIndex = 0;
            }

            return value.Substring(startIndex, value.Length - startIndex);
        }

        public static string SubStrIndex(this string value, int startIndex, int endIndex)
        {
            if (value == null)
            {
                return value;
            }

            if (startIndex < 0 || startIndex > value.Length - 1)
            {
                startIndex = 0;
            }

            if (endIndex < 0 || endIndex > value.Length - 1)
            {
                endIndex = value.Length - 1;
            }

            return value.Substring(startIndex, endIndex - startIndex);
        }
    }
}

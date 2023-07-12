using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Codebase.Utils
{
    public static class TextUtils
    {
        private static readonly List<string> NUMBER_LETTERS = new List<string> {"K", "M", "B", "t", "q", "Q", "s", "S", "O", "N", "d", "U", "D", "T"};

        public static double Round(double rawNumber)
        {
            if (rawNumber < 100) return rawNumber;

            var power10 = (int) Math.Floor(Math.Log(10, rawNumber));
            var roundBase = Math.Pow(10, power10 - 1);
            return (long) (Math.Round(rawNumber / roundBase) * roundBase);
        }

        public static string Truncate(double rawNumber) => Math.Truncate(rawNumber).ToString(CultureInfo.InvariantCulture);

        public static string FormatShort(long rawNumber)
        {
            if (rawNumber < 1000) return rawNumber.ToString();

            var rawPower10 = Mathf.Log(rawNumber, 10) - 0.000000005f;
            var power10 = (int) Mathf.Floor(rawPower10);
            var level = power10 / 3;

            var roundBase = Math.Pow(10, level * 3);
            return string.Format("{0:0.#}", rawNumber / roundBase) + NUMBER_LETTERS[level - 1];
        }
        
        public static string FormatShort(double rawNumber)
        {
            if (rawNumber < 1000) return Math.Round(rawNumber).ToString(CultureInfo.InvariantCulture);
            
            var rawPower10 = Math.Log(rawNumber, 10) + 0.000000005f;
            var power10 = (int) Math.Floor(rawPower10);
            var level = power10 / 3;
        
            var roundBase = Math.Pow(10, level * 3);

            int index = level - 1;
            index = index < 0 ? 0 : index;
            index = index > NUMBER_LETTERS.Count - 1 ? NUMBER_LETTERS.Count - 1 : index;
            return $"{rawNumber / roundBase:0.#}" + NUMBER_LETTERS[index];
        }
        

        public static string FormatLong(long rawNumber) => rawNumber.ToString(NumberFormatInfo.CurrentInfo);

        public static string FormatDouble(double rawNumber) => rawNumber.ToString(NumberFormatInfo.CurrentInfo);

        public static string Cut(string input, int symbolsCount)
        {
            if (input.Length > symbolsCount) 
                input = input.Substring(0, symbolsCount);
            
            return input;
        }
    }
}

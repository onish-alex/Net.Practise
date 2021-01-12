using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace AlgorithmsAndLoops
{
    public static class TaskUtilities
    {
        public static int[] GetDigits(int number)
        {
            var inversedDigits = new List<int>();

            int divider = 1;
            int modDivider = 10;

            var lastModDivision = number - 1;
            
            while (lastModDivision != number)
            {
                lastModDivision = number % modDivider;
                inversedDigits.Add(lastModDivision / divider);
                modDivider *= 10;
                divider *= 10;
            }

            var digits = new int[inversedDigits.Count];

            for (int i = 0; i < digits.Length; i++)
            {
                digits[i] = inversedDigits[digits.Length - i - 1];
            }

            return digits;
        }

        public static int[] GenerateArray(int length, int from, int to)
        {
            int[] array = new int[length];
            var rand = new Random();
            
            for (int i = 0; i < length; i++)
            {
                array[i] = (from < to) ? rand.Next(from, to)
                                       : rand.Next(to, from);
            }

            return array;
        }

        public static void PrintSeparateDigits(int number)
        {
            var digits = GetDigits(number); 
            
            for (int i = 0; i < digits.Length; i++)
            {
                Console.Write("{0} ", digits[i]);
            }
            
            Console.WriteLine();
        }

        public static void PrintIntegerAndFractionalParts(double number)
        {
            var integerPart = Math.Floor(number);
            var fractionalPart = Math.Round(number - integerPart, 3);
            Console.WriteLine("{0} and {1}", integerPart, fractionalPart);
        }

        public static int FindMaxDigit(int number)
        {
            var digits = GetDigits(number);
            var maxDigit = digits[0];
            
            for (int i = 1; i < digits.Length; i++)
            {
                if (digits[i] > maxDigit)
                {
                    maxDigit = digits[i];
                }
            }

            return maxDigit;
        }

        public static int FindMinDigit(int number)
        {
            var digits = GetDigits(number);
            var minDigit = digits[0];

            for (int i = 1; i < digits.Length; i++)
            {
                if (digits[i] < minDigit)
                {
                    minDigit = digits[i];
                }
            }

            return minDigit;
        }

        public static void PrintOnlyDigits(string str)
        {
            var digits = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            
            for (int i = 0; i < str.Length; i++)
            {
                for (int j = 0; j < digits.Length; j++)
                {
                    if (str[i] == digits[j]) 
                    {
                        Console.Write("{0} ", str[i]);
                    }
                }
            }
        }

        public static string GetCurrentDateTimeInISO8601()
        {
            return string.Format($"{DateTime.Now:o}");
        }

        public static DateTime ConvertToDateTime(string date, string pattern)
        {
            return DateTime.TryParseExact(date, pattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime output)
                ? output
                : new DateTime();
        }

        public static string[] ConvertToUpperCase(string[] data)
        {
            string[] upperCaseData = new string[data.Length];
            
            for (int i = 0; i < data.Length; i++)
            {
                var strParts = data[i].Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
                upperCaseData[i] = data[i];
                
                foreach (var part in strParts)
                {
                    upperCaseData[i] = upperCaseData[i].Replace(part, char.ToUpper(part[0]) + part.Substring(1));
                }
            }
            
            return upperCaseData;
        }

        public static string DecodeBase64(string encodedStr)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedStr));
        }

        public static void BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        var temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }

        public static void CompressArray(string fileName, int[] array)
        {
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (var zip = new GZipStream(file, CompressionMode.Compress))
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        zip.Write(BitConverter.GetBytes(array[i]));
                    } 
                }
            }
        }
        
        public static int[] DecompressArray(string fileName)
        {
            var decompressedValues = new List<int>();
            var stream = new MemoryStream();
            
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (var zip = new GZipStream(file, CompressionMode.Decompress))
                {
                    zip.CopyTo(stream);
                }
            }
            
            var byteArray = stream.ToArray();

            for (int i = 0; i < byteArray.Length; i += sizeof(int))
            {
                decompressedValues.Add(BitConverter.ToInt32(byteArray, i));
            }

            return decompressedValues.ToArray();
        }

        public static void QuickSort(int[] array)
        {
            QuickSortPart(array, 0, array.Length - 1);
        }

        private static void QuickSortPart(int[] array, int start, int end)
        {
            if (start >= end)
            {
                return;
            }

            int pivotIndex = (end + start) / 2;
            int pivot = array[pivotIndex];

            for (int i = start; i < pivotIndex; i++)
            {
                if (array[i] > pivot)
                {
                    int temp = array[pivotIndex - 1];
                    array[pivotIndex - 1] = array[i];
                    array[i] = temp;

                    temp = array[pivotIndex];
                    array[pivotIndex] = array[pivotIndex - 1];
                    array[pivotIndex - 1] = temp;

                    pivotIndex--;
                    i--;
                }
            }

            for (int j = end; j > pivotIndex; j--)
            {
                if (array[j] < pivot)
                {
                    int temp = array[pivotIndex + 1];
                    array[pivotIndex + 1] = array[j];
                    array[j] = temp;

                    temp = array[pivotIndex];
                    array[pivotIndex] = array[pivotIndex + 1];
                    array[pivotIndex + 1] = temp;

                    pivotIndex++;
                    j++;
                }
            }

            QuickSortPart(array, start, pivotIndex - 1);
            QuickSortPart(array, pivotIndex + 1, end);
        }
    }
}

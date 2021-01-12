using System;
using System.IO;

namespace AlgorithmsAndLoops
{
    class Program
    {
        static void Main(string[] args)
        {
            int testIntNumber1 = 12345, testIntNumber2 = 192837465;
            double testDoubleNumber = 2.543;
            string testStr = "Hell0, Wo21d!";
            string testZipPath = "test.zip";

            Console.Write("1. Separate digits of number {0} : ", testIntNumber1);
            TaskUtilities.PrintSeparateDigits(testIntNumber1);
            
            Console.Write("\n2. Separate integer and fractional parts of number {0} : ", testDoubleNumber);
            TaskUtilities.PrintIntegerAndFractionalParts(testDoubleNumber);
            
            Console.Write("\n3. Max digit of number {0} : ", testIntNumber2);
            var maxDigit = TaskUtilities.FindMaxDigit(testIntNumber2);
            Console.Write(maxDigit);

            Console.Write("\n\n4. Min digit of number {0} : ", testIntNumber2);
            var minDigit = TaskUtilities.FindMinDigit(testIntNumber2);
            Console.Write(minDigit);

            Console.Write("\n\n5. Digits from string \"{0}\" : ", testStr);
            TaskUtilities.PrintOnlyDigits(testStr);
            
            Console.Write("\n\n6. Date in ISO-8601: ");
            var dateStr = TaskUtilities.GetCurrentDateTimeInISO8601();
            Console.WriteLine(dateStr.ToString());

            Console.Write("\n7. Parsed date \"2016 21-07\": ");
            var convertedDate = TaskUtilities.ConvertToDateTime("2016 21-07", "yyyy dd-MM");
            Console.WriteLine(convertedDate.ToString());
            
            Console.WriteLine("\n8. Make upper case names: ");
            var upperCaseNames = TaskUtilities.ConvertToUpperCase(new string[] { "иван иванов", "светлана иванова-петренко" });
            foreach (var item in upperCaseNames)
            {
                Console.WriteLine(item);
            }
            
            Console.WriteLine("\n9. Decode base64 format string: ");
            var decodedBase64 = TaskUtilities.DecodeBase64(@"0JXRgdC70Lgg0YLRiyDRh9C40YLQsNC10YjRjCDRjdGC0L7RgiDRgtC10LrRgdGCLCDQt9C90LDRh9C40YIg0LfQsNC00LDQvdC40LUg0LLRi9C / 0L7Qu9C90LXQvdC + INCy0LXRgNC90L4gOik =");
            Console.WriteLine(decodedBase64);
            
            Console.WriteLine("\n10. Make bubble sort on 39 elements array: ");
            var array = TaskUtilities.GenerateArray(39, 0, 1000);
            Console.WriteLine("Initial: ");
            
            foreach (var item in array)
            {
                Console.Write("{0} ", item);
            }
            
            TaskUtilities.BubbleSort(array);
            
            Console.WriteLine("\n\nSorted: ");
            
            foreach (var item in array)
            {
                Console.Write("{0} ", item);
            }

            var tree = TreeNode<int>.GetDefaultTree();
            
            Console.WriteLine("\n\n11. DFS: ");
            
            foreach (var item in tree.DepthFirstTraversal())
            {
                Console.WriteLine(item.Data);
            }
            
            Console.WriteLine("\nBFT: ");
            
            foreach (var item in tree.BreadthFirstTraversal())
            {
                Console.WriteLine(item.Data);
            }

            Console.WriteLine("\n12. Compressing/Decompressing array: ");
            Console.Write("Initial array: ");
            array = TaskUtilities.GenerateArray(50, 0, 100);
            
            foreach (var item in array)
            {
                Console.Write("{0} ", item);
            }

            TaskUtilities.CompressArray(testZipPath, array);
            var compressedArray = File.ReadAllText(testZipPath);
            Console.WriteLine("\n\nCompressed array: " + compressedArray);
            Console.Write("\nDecompressed array: ");
            var decompressedArray = TaskUtilities.DecompressArray(testZipPath);
            
            foreach (var item in decompressedArray)
            {
                Console.Write("{0} ", item);
            }

            Console.WriteLine("\n\n13. Make quick sort on 39 elements array: ");
            array = TaskUtilities.GenerateArray(39, 0, 1000);
            Console.WriteLine("Initial: ");
            
            foreach (var item in array)
            {
                Console.Write("{0} ", item);
            }
            
            TaskUtilities.QuickSort(array);
            Console.WriteLine("\n\nSorted: ");
            
            foreach (var item in array)
            {
                Console.Write("{0} ", item);
            }

            Console.WriteLine();
        }
    }
}

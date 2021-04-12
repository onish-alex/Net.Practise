using System;
using System.Linq;

namespace Linq2
{
    class Program
    {
        static void Main(string[] args)
        {
            var testSentence = "aaa; abb; ccc; dap";
            var testWord = "abb";
            var testChar = 'a';
            var delimiter = "; ";

            Console.WriteLine("1)");
            FunWithLinq.PrintNumbers(10, 50);
            Console.Write("\n\n");

            Console.WriteLine("2)");
            FunWithLinq.PrintDivisibleNumbers(10, 50, 3);
            Console.Write("\n\n");

            Console.WriteLine("3)");
            FunWithLinq.PrintNTimes("Linq", 10);
            Console.Write("\n");

            Console.WriteLine("4)");
            FunWithLinq.PrintWordsWithLetter(testSentence, testChar, delimiter);
            Console.Write("\n");

            Console.WriteLine("5)");
            FunWithLinq.PrintWordsWithLetterLengths(testSentence, testChar, delimiter);
            Console.Write("\n\n");

            Console.WriteLine("6)");
            testSentence = "aaa; xabbx; abb; ccc; dap";
            Console.WriteLine(FunWithLinq.ContainsWord(testSentence, testWord, delimiter));
            Console.Write("\n");

            Console.WriteLine("7)");
            var longestWord = FunWithLinq.GetLongestWord(testSentence, delimiter);
            Console.WriteLine("The longest word in \"{0}\" is \"{1}\"", testSentence, longestWord);
            Console.Write("\n");

            Console.WriteLine("8)");
            var averageWordLength = FunWithLinq.GetAverageWordLength(testSentence, "; ");
            Console.WriteLine("Average word length in \"{0}\" equals {1:f3}", testSentence, averageWordLength);
            Console.Write("\n");

            Console.WriteLine("9)");
            testSentence = "aaa; xabbx; abb; ccc; dap; zh";
            FunWithLinq.PrintShortestWordInversed(testSentence, delimiter);
            Console.Write("\n");

            Console.WriteLine("10)");
            testSentence = "baaa; aabb; aaa; xabbx; abb; ccc; dap; zh";
            FunWithLinq.Task10(testSentence, delimiter);
            Console.Write("\n");

            Console.WriteLine("11)");
            FunWithLinq.Task11(testSentence, delimiter);

            Console.WriteLine("\n-----------\n");

            Console.WriteLine("1)");
            FunWithLinq.PrintActorsNames();
            Console.Write("\n");

            Console.WriteLine("2)");
            FunWithLinq.PrintActorsCountBornAtMonth(4);
            Console.Write("\n");

            Console.WriteLine("3)");
            FunWithLinq.PrintOldestActorsNames(2);
            Console.Write("\n");

            Console.WriteLine("4)");
            FunWithLinq.PrintArticlesCountByAuthor();
            Console.Write("\n");

            Console.WriteLine("5)");
            FunWithLinq.PrintArtObjectsCountByAuthorName("Martin Scorsese");
            FunWithLinq.PrintArtObjectsCountByAuthorName("Hilgendorf");
            Console.Write("\n");

            Console.WriteLine("6)");
            FunWithLinq.PrintDifferentLettersInActorsNames();
            Console.Write("\n");

            Console.WriteLine("7)");
            FunWithLinq.PrintArticlesNamesDoubleOrdered();
            Console.Write("\n");

            Console.WriteLine("8)");
            FunWithLinq.PrintActorsAndFilms();
            Console.Write("\n");

            Console.WriteLine("9)");
            FunWithLinq.PrintSumOfPagesAndOtherInts();
            Console.Write("\n");

            Console.WriteLine("10)");
            var dictionary = FunWithLinq.GetAuthorsAndArticlesDictionary();

            foreach (var item in dictionary)
            {
                Console.WriteLine("{0} - {1}", item.Key, string.Join(", ", item.Value.Select(a => a.Name)));
            }

            Console.Write("\n");
        }
    }
}

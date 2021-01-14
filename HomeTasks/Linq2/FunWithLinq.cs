using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq2
{
    public static class FunWithLinq
    {
        private static IEnumerable<int> GetRange(int from, int to)
        {
            if (from < to)
            {
                return Enumerable.Range(from, to - from + 1);
            }
            return Enumerable.Range(to, from - to + 1);
        }

        public static void PrintNumbers(int from, int to)
        {
            GetRange(from, to).Select(a => { Console.Write("{0}, ", a); return a; })
                              .ToArray();
        }

        public static void PrintDivisibleNumbers(int from, int to, int divideOn)
        {
            GetRange(from, to)
                .Where(a => a % divideOn == 0)
                .Select(a => { Console.Write("{0}, ", a); return a; })
                .ToArray();
        }

        public static void PrintNTimes(string word, int n)
        {
            Enumerable.Repeat(word, n)
                      .Select(a => { Console.WriteLine("{0}", a); return a; })
                      .ToArray();
        }

        public static void PrintWordsWithLetter(string sentence, char letter, string delimiter)
        {
            Console.WriteLine(string.Join(delimiter, sentence.Split(delimiter, StringSplitOptions.RemoveEmptyEntries).Where(a => a.Contains(letter))));
        }

        public static void PrintWordsWithLetterLengths(string sentence, char letter, string delimiter)
        {
            sentence.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                    .Where(a => a.Contains(letter))
                    .Select(a => { Console.Write("{0}, ", a.Length); return a; })
                    .ToArray();
        }

        public static bool ContainsWord(string sentence, string word, string delimiter)
        {
            return sentence.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                           .Any(a => a == word);
        }

        public static string GetLongestWord(string sentence, string delimiter)
        {
            var words = sentence.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            return words.OrderByDescending(a => a.Length)
                        .FirstOrDefault();
        }

        public static double GetAverageWordLength(string sentence, string delimiter)
        {
            return sentence.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                           .Average(a => a.Length);
        }

        public static void PrintShortestWordInversed(string sentence, string delimiter)
        {
            Console.WriteLine(new string(sentence.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                                                 .OrderBy(a => a.Length)
                                                 .FirstOrDefault()
                                                 .Reverse()
                                                 .ToArray()));

        }

        public static void Task10(string sentence, string delimiter)
        {
            Console.WriteLine(sentence.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                                      .Any(a => a.StartsWith("aa")
                                             && a.Skip(2).All(b => b == 'b')));
        }

        public static void Task11(string sentence, string delimiter)
        {
            var words = sentence.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            words.Where(a => a.EndsWith("bb")
                          || a == words.Last())
                 .Select(a => { Console.Write("{0}, ", a); return a; })
                 .ToArray();
        }

        private static IEnumerable<Film> GetFilms()
        {
            return ArtData.Get().OfType<Film>();
        }

        private static IEnumerable<Actor> GetActors()
        {
            return GetFilms()
                   .Select(a => a.Actors)
                   .SelectMany(a => a.AsEnumerable())
                   .GroupBy(a => a.Name)
                   .Select(a => a.First());
        }

        private static IEnumerable<Article> GetArticles()
        {
            return ArtData.Get().OfType<Article>();
        }

        public static void PrintActorsNames()
        {
            GetActors()
              .Select(a => { Console.WriteLine("{0}", a.Name); return a.Name; })
              .ToArray();
        }

        public static void PrintActorsCountBornAtMonth(int month)
        {
            Console.WriteLine(GetActors()
                                .Where(a => a.Birthdate.Month == month)
                                .Count());
        }

        public static void PrintOldestActorsNames(int count)
        {
            GetActors()
              .OrderBy(a => a.Birthdate)
              .Take(count)
              .Select(a => { Console.WriteLine("{0}, ", a.Name); return a.Name; })
              .ToArray();
        }

        public static void PrintArticlesCountByAuthor()
        {
            GetArticles()
              .GroupBy(a => a.Author)
              .Select(a => { Console.WriteLine("{0} - {1}", a.Key, a.Count()); return a; })
              .ToArray();
        }

        public static void PrintArtObjectsCountByAuthorName(string author)
        {
            Console.WriteLine(ArtData.Get()
                                .OfType<ArtObject>()
                                .Where(a => a.Author == author)
                                .Count());
        }

        public static void PrintDifferentLettersInActorsNames()
        {
            Console.WriteLine(string.Join("", GetActors().Select(a => a.Name.Replace(" ", "").ToLower()))
                                .Distinct()
                                .Count());
        }

        public static void PrintArticlesNamesDoubleOrdered()
        {
            GetArticles()
              .OrderBy(a => a.Author)
              .ThenBy(a => a.Pages)
              .Select(a => { Console.WriteLine("{0}, ", a.Name); return a.Name; })
              .ToArray();
        }

        public static void PrintActorsAndFilms()
        {

            GetActors().Select(actor =>
            {
                var films = GetFilms().Where(filtered => filtered.Actors
                                                         .Select(film => film.Name)
                                                         .Contains(actor.Name));

                var filmsStr = string.Join(", ", films.Select(film => film.Name));
                Console.WriteLine("{0} - {1}", actor.Name, filmsStr);
                return actor;
            }).ToArray();
        }

        public static void PrintSumOfPagesAndOtherInts()
        {
            var pagesSum = GetArticles()
                            .Select(a => a.Pages)
                            .Sum();

            var intsSum = ArtData.Get()
                                 .OfType<IEnumerable<int>>()
                                 .SelectMany(a => a.AsEnumerable())
                                 .Sum();

            Console.WriteLine(pagesSum + intsSum);
        }

        public static IDictionary<string, IEnumerable<Article>> GetAuthorsAndArticlesDictionary()
        {
            return GetArticles()
                     .GroupBy(a => a.Author)
                     .ToDictionary(key => key.Key, value => value.AsEnumerable());
        }
    }
}

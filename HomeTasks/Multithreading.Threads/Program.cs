namespace MultithreadingTasks
{
    using System;
    using System.Diagnostics;

    public class Program
    {
        public static void Main(string[] args)
        {
            LessonTask task = new LessonTask(10000000);

            var sw = Stopwatch.StartNew();
            var sum = task.Run(Environment.ProcessorCount);
            sw.Stop();
            Console.WriteLine("{0} threads - Sum: {1}, Time: {2} ms", Environment.ProcessorCount, sum, sw.ElapsedMilliseconds);

            sw.Restart();
            sum = task.Run(1);
            sw.Stop();
            Console.WriteLine("1 thread - Sum: {0}, Time: {1} ms", sum, sw.ElapsedMilliseconds);
            Console.WriteLine();

            var array = HomeTask.GenerateRandomArray(100);
            var from = 5;
            var to = 45;
            var tempArray = HomeTask.GetCopy(array, from, to);
            var min = HomeTask.GetMinimum(array);
            var avg = HomeTask.GetAverage(array);

            Console.WriteLine("Parallel generated array: ");

            foreach (var item in array)
            {
                Console.Write("{0} ", item);
            }

            Console.WriteLine("\n");
            Console.WriteLine("Copy of main array, from {0} to {1}: ", from, to);

            foreach (var item in tempArray)
            {
                Console.Write("{0} ", item);
            }

            Console.WriteLine("\n");
            Console.WriteLine("Minimum of main array: {0}", min);
            Console.WriteLine();
            Console.WriteLine("Average of main array: {0:f3}", avg);
        }
    }
}

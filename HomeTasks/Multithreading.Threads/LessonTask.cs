using System;
using System.Threading;
using System.Linq;

namespace MultithreadingTasks
{
    public class LessonTask
    {
        private decimal[] array;
        private int arrayLength;
        private int threadCount;
        decimal[] threadResults;

        public LessonTask(int arrayLength)
        {
            this.arrayLength = arrayLength;
            this.array = new decimal[arrayLength];
            var rand = new Random();

            for (int i = 0; i < arrayLength; i++)
            {
                array[i] = (decimal)rand.NextDouble();
            }
        }

        public decimal Run(int threadCount)
        {
            this.threadCount = threadCount;
            this.threadResults = new decimal[threadCount];
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                threadResults[i] = 0;
                threads[i] = new Thread((number) => GetPartialSum((int)number));
                threads[i].Start(i);
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

            return threadResults.Sum();
        }

        private void GetPartialSum(int number)
        {
            int start = number * (arrayLength / threadCount);
            int end = (number + 1) * (arrayLength / threadCount);
            for (int i = start; i < end; i++)
            {
                threadResults[number] += array[i];
            }
        }

    }
}

namespace MultithreadingTasks
{
    using System;
    using System.Linq;
    using System.Threading;

    public class LessonTask
    {
        private decimal[] array;
        private int arrayLength;
        private int threadCount;
        private decimal[] threadResults;

        public LessonTask(int arrayLength)
        {
            this.arrayLength = arrayLength;
            this.array = new decimal[arrayLength];
            var rand = new Random();

            for (int i = 0; i < arrayLength; i++)
            {
                this.array[i] = (decimal)rand.NextDouble();
            }
        }

        public decimal Run(int threadCount)
        {
            this.threadCount = threadCount;
            this.threadResults = new decimal[threadCount];
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                this.threadResults[i] = 0;
                threads[i] = new Thread((number) => this.GetPartialSum((int)number));
                threads[i].Start(i);
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

            return this.threadResults.Sum();
        }

        private void GetPartialSum(int number)
        {
            int start = number * (this.arrayLength / this.threadCount);
            int end = (number + 1) * (this.arrayLength / this.threadCount);
            for (int i = start; i < end; i++)
            {
                this.threadResults[number] += this.array[i];
            }
        }
    }
}

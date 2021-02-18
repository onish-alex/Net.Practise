namespace MultithreadingTasks
{
    using System;
    using System.Linq;
    using System.Threading;

    public static class HomeTask
    {
        private static readonly Random rand = new Random();

        public static void DoParallelTask(ParameterizedThreadStart threadStart, ThreadDataBlock blockParams)
        {
            var procCount = Environment.ProcessorCount;
            var threads = new Thread[procCount];

            for (int i = 0; i < procCount; i++)
            {
                var startIndex = i * (int)Math.Ceiling((double)blockParams.Array.Length / procCount);
                var endIndex = (i + 1) * (int)Math.Ceiling((double)blockParams.Array.Length / procCount);

                if (startIndex >= blockParams.Array.Length)
                {
                    endIndex = startIndex - 1;
                }

                if (endIndex > blockParams.Array.Length)
                {
                    endIndex = blockParams.Array.Length;
                }

                var blockInfo = new ThreadDataBlock()
                {
                    Array = blockParams.Array,
                    ResultContainer = blockParams.ResultContainer,
                    StartIndex = (blockParams.StartIndex == null) ? startIndex : blockParams.StartIndex,
                    EndIndex = (blockParams.EndIndex == null) ? endIndex : blockParams.EndIndex,
                    Number = (blockParams.Number == null) ? i : blockParams.Number,
                };

                threads[i] = new Thread(threadStart);
                threads[i].Start(blockInfo);
            }

            for (int i = 0; i < procCount; i++)
            {
                threads[i].Join();
            }
        }

        public static decimal[] GenerateRandomArray(int length)
        {
            var array = new decimal[length];

            DoParallelTask((blockInfo) => FillArray((ThreadDataBlock)blockInfo), new ThreadDataBlock() { Array = array });

            return array;
        }

        public static decimal[] GetCopy(decimal[] array, int start, int end)
        {
            var tempArrayLength = end - start + 1;
            var tempArray = new decimal[tempArrayLength];

            DoParallelTask((blockInfo) => CopyArray((ThreadDataBlock)blockInfo), new ThreadDataBlock() { Array = tempArray, ResultContainer = array, Number = start });

            return tempArray;
        }

        public static decimal GetMinimum(decimal[] array)
        {
            var partialMinimums = new decimal[Environment.ProcessorCount];
            var threadBlock = new ThreadDataBlock() { Array = array, ResultContainer = partialMinimums };
            DoParallelTask((blockInfo) => GetPartialMinimum((ThreadDataBlock)blockInfo), threadBlock);
            return threadBlock.ResultContainer.Min();
        }

        public static decimal GetAverage(decimal[] array)
        {
            var partialAverages = new decimal[Environment.ProcessorCount];
            var threadBlock = new ThreadDataBlock() { Array = array, ResultContainer = partialAverages };
            DoParallelTask((blockInfo) => GetPartialAverage((ThreadDataBlock)blockInfo), threadBlock);
            return threadBlock.ResultContainer.Average();
        }

        private static void FillArray(ThreadDataBlock blockInfo)
        {
            for (int i = blockInfo.StartIndex.Value; i < blockInfo.EndIndex.Value; i++)
            {
                decimal randValue = 0;

                lock (rand)
                {
                    randValue = rand.Next(0, blockInfo.Array.Length);
                }

                blockInfo.Array[i] = randValue;
            }
        }

        private static void CopyArray(ThreadDataBlock blockInfo)
        {
            for (int i = blockInfo.StartIndex.Value; i < blockInfo.EndIndex.Value; i++)
            {
                blockInfo.Array[i] = blockInfo.ResultContainer[i + blockInfo.Number.Value];
            }
        }

        private static void GetPartialMinimum(ThreadDataBlock blockInfo)
        {
            decimal min = blockInfo.Array[blockInfo.StartIndex.Value];
            for (int i = blockInfo.StartIndex.Value + 1; i < blockInfo.EndIndex.Value; i++)
            {
                if (blockInfo.Array[i] < min)
                {
                    min = blockInfo.Array[i];
                }
            }

            blockInfo.ResultContainer[blockInfo.Number.Value] = min;
        }

        private static void GetPartialAverage(ThreadDataBlock blockInfo)
        {
            for (int i = blockInfo.StartIndex.Value + 1; i < blockInfo.EndIndex.Value; i++)
            {
                blockInfo.ResultContainer[blockInfo.Number.Value] += blockInfo.Array[i];
            }

            blockInfo.ResultContainer[blockInfo.Number.Value] /= blockInfo.EndIndex.Value - blockInfo.StartIndex.Value + 1;
        }
    }
}

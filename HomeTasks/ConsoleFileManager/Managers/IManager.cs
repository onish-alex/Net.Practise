namespace ConsoleFileManager
{
    using System.Collections.Generic;

    public interface IManager
    {
        public IEnumerable<string> GetInitialData();

        public IEnumerable<string> GetData(string param);

        public IEnumerable<string> GetPreviousData();
    }
}

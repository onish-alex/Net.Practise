using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleFileManager
{
    public interface IManager
    {
        public IEnumerable<string> GetInitialData();

        public IEnumerable<string> GetData(string param);

        public IEnumerable<string> GetPreviousData();
    }
}

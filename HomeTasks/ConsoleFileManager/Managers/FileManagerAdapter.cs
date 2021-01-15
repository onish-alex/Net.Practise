using System.Collections.Generic;

namespace ConsoleFileManager
{
    public class FileManagerAdapter : IManager
    {
        private FileManager fileManager;
        public FileManagerAdapter()
        {
            fileManager = new FileManager();
        }

        public IEnumerable<string> GetData(string param)
        {
            return fileManager.GetContentFromPath(param);
        }

        public IEnumerable<string> GetInitialData()
        {
            return fileManager.GetRootContent();
        }

        public IEnumerable<string> GetPreviousData()
        {
            return fileManager.GetPreviousPathEntries();
        }
    }
}

namespace ConsoleFileManager
{
    using System.Collections.Generic;

    public class FileManagerAdapter : IManager
    {
        private FileManager fileManager;

        public FileManagerAdapter()
        {
            this.fileManager = new FileManager();
        }

        public IEnumerable<string> GetData(string param)
        {
            return this.fileManager.GetContentFromPath(param);
        }

        public IEnumerable<string> GetInitialData()
        {
            return this.fileManager.GetRootContent();
        }

        public IEnumerable<string> GetPreviousData()
        {
            return this.fileManager.GetPreviousPathEntries();
        }
    }
}

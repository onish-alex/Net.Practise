namespace ConsoleFileManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FileManager
    {
        public static readonly string BackCommand = ":back";

        private static IEnumerable<string> drivesNames = DriveInfo.GetDrives()
                                                                  .Where(drive => drive.IsReady)
                                                                  .Select(drive => drive.Name)
                                                                  .ToArray();

        private static IEnumerable<string> textReadExtensions = new string[] { ".txt" };

        private bool isInFile;
        private string path;

        public FileManager()
        {
            this.path = string.Empty;
        }

        public IEnumerable<string> GetRootContent()
        {
            return drivesNames;
        }

        public IEnumerable<string> GetContentFromPath(string relativePath)
        {
            if (this.path == string.Empty)
            {
                if (drivesNames.Contains(relativePath))
                {
                    this.path = relativePath;
                    relativePath = string.Empty;
                }
                else
                {
                    return null;
                }
            }

            // if drive root part
            if (drivesNames.Contains(this.path + relativePath))
            {
                return this.GetFileSystemEntries(string.Empty);
            }

            if (this.isInFile)
            {
                return null;
            }

            // if input is directory name
            if (Directory.GetDirectories(this.path).GetRelativePaths(this.path).Contains(relativePath))
            {
                var fileSystemEntries = this.GetFileSystemEntries(relativePath);
                this.path += relativePath + Path.DirectorySeparatorChar;
                return fileSystemEntries;
            }

            // if input if file name
            if (Directory.GetFiles(this.path)
                         .GetRelativePaths(this.path)
                         .Contains(relativePath))
            {
                var fileContent = Enumerable.Repeat(this.GetFileContent(relativePath), 1);
                this.path += relativePath + Path.DirectorySeparatorChar;

                this.isInFile = true;

                return fileContent;
            }

            return null;
        }

        public IEnumerable<string> GetPreviousPathEntries()
        {
            if (this.isInFile)
            {
                this.isInFile = false;
            }

            if (drivesNames.Contains(this.path)
             || this.path == string.Empty)
            {
                this.path = string.Empty;
                return drivesNames;
            }
            else
            {
                var fileSystemEntries = this.GetFileSystemEntries(Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar);
                this.path = string.Join(Path.DirectorySeparatorChar, this.path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries).SkipLast(1));
                this.path += Path.DirectorySeparatorChar;

                return fileSystemEntries;
            }
        }

        private IEnumerable<string> GetFileSystemEntries(string relativePath)
        {
            var fileSystemEntries = new List<string>();
            fileSystemEntries.AddRange(Directory.GetDirectories(this.path + relativePath).Where(a => !new DirectoryInfo(a).Attributes.HasFlag(FileAttributes.Hidden)));
            fileSystemEntries.AddRange(Directory.GetFiles(this.path + relativePath).Where(a => !new FileInfo(a).Attributes.HasFlag(FileAttributes.Hidden)));

            return fileSystemEntries.Select(entry => entry.Split(Path.DirectorySeparatorChar).Last());
        }

        private string GetFileContent(string relativePath)
        {
            if (textReadExtensions.Contains(Path.GetExtension(relativePath)))
            {
                char[] buffer = new char[1024];

                using (var file = File.OpenText(this.path + relativePath))
                {
                    file.ReadBlock(buffer, 0, 1024);
                }

                return new string(buffer);
            }
            else
            {
                byte[] buffer = new byte[2048];

                using (var file = File.OpenRead(this.path + relativePath))
                {
                    file.Read(buffer, 0, 2048);
                }

                return BitConverter.ToString(buffer);
            }
        }
    }
}

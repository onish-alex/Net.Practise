using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleFileManager
{
    public class FileManager
    {
        private bool isInFile;
        private string path;
        private static IEnumerable<string> drivesNames = DriveInfo.GetDrives()
                                                                  .Where(drive => drive.IsReady)
                                                                  .Select(drive => drive.Name)
                                                                  .ToArray();

        private static IEnumerable<string> textReadExtensions = new string[] { ".txt" };

        public static readonly string backCommand = ":back";

        public FileManager()
        {
            path = string.Empty;
        }

        public IEnumerable<string> GetRootContent()
        {
            return drivesNames;
        }

        public IEnumerable<string> GetContentFromPath(string relativePath)
        {
            if (path == string.Empty)
            {
                if (drivesNames.Contains(relativePath))
                {
                    path = relativePath;
                    relativePath = string.Empty;
                }
                else
                {
                    return null;
                }
            }

            //if drive root part
            if (drivesNames.Contains(path+relativePath))
            {
                return GetFileSystemEntries(string.Empty);
            }

            if (isInFile)
            {
                return null;
            }

            //if input is directory name
            if (Directory.GetDirectories(path).GetRelativePaths(path).Contains(relativePath))
            {
                var fileSystemEntries = GetFileSystemEntries(relativePath);
                path += relativePath + Path.DirectorySeparatorChar;
                
                return fileSystemEntries;
            }

            //if input if file name
            if (Directory.GetFiles(path).GetRelativePaths(path).Contains(relativePath))
            {
                var fileContent = Enumerable.Repeat(GetFileContent(relativePath), 1);
                path += relativePath + Path.DirectorySeparatorChar;

                isInFile = true;
                
                return fileContent;
            }

            return null; 
        } 

        private IEnumerable<string> GetFileSystemEntries(string relativePath)
        {
            var fileSystemEntries = new List<string>();
            fileSystemEntries.AddRange(Directory.GetDirectories(path + relativePath).Where(a => !new DirectoryInfo(a).Attributes.HasFlag(FileAttributes.Hidden)));
            fileSystemEntries.AddRange(Directory.GetFiles(path + relativePath).Where(a => !new FileInfo(a).Attributes.HasFlag(FileAttributes.Hidden)));
            
            return fileSystemEntries.Select(entry => entry.Split(Path.DirectorySeparatorChar).Last());
        }

        private string GetFileContent(string relativePath)
        {
            if (textReadExtensions.Contains(Path.GetExtension(relativePath)))
            {
                char[] buffer = new char[1024];

                using (var file = File.OpenText(path + relativePath))
                {
                    file.ReadBlock(buffer, 0, 1024);
                }

                return new string(buffer);
            }
            else
            {
                byte[] buffer = new byte[2048];

                using (var file = File.OpenRead(path + relativePath))
                {
                    file.Read(buffer, 0, 2048);
                }

                return BitConverter.ToString(buffer);
            }
        }

        public IEnumerable<string> GetPreviousPathEntries()
        {
            if (isInFile)
            {
                isInFile = false;
            }

            if (drivesNames.Contains(path) 
             || path == string.Empty)
            {
                path = string.Empty;
                return drivesNames;
            } 
            else
            {
                var fileSystemEntries = GetFileSystemEntries(Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar);
                path = string.Join(Path.DirectorySeparatorChar, path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries).SkipLast(1));
                path += Path.DirectorySeparatorChar;;
                
                return fileSystemEntries;
            }
        }
    }
}

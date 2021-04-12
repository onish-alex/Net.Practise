using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace File.Manager
{
    public static class ExtensionForLinq
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
    }

    public static class TypeExtension
    {
        public static T CreateInstance<T>(this Type type)
            where T : class
        {
            return Activator.CreateInstance(type) as T;
        }
    }

    class PathView
    {
        private string _currentPath;
        private IEnumerable<Type> readers;

        public PathView(string basePath = @"c:\")
        {
            if (!Path.IsPathRooted(basePath))
            {
                throw new ArgumentException(nameof(basePath));
            }
            this._currentPath = basePath;

            this.readers = Assembly.GetExecutingAssembly()
                                   .GetTypes()
                                   .Where(type => type.GetInterfaces().Contains(typeof(IFileReader)));
        }

        public void Show()
        {
            this.TryShowFile();

            var query = Directory.EnumerateDirectories(this._currentPath).Select(d => new DirectoryInfo(d))
                .OrderBy(d => d.Name).ThenBy(d => d.LastWriteTime).Cast<FileSystemInfo>();

            query = query.Concat(Directory.EnumerateFiles(_currentPath).Select(f => new FileInfo(f))
                .OrderBy(f => f.Name).ThenBy(f => f.LastWriteTime));

            Console.Clear();
            Console.WriteLine(this._currentPath);

            foreach (var d in query.Where(x => !x.Attributes.HasFlag(FileAttributes.Hidden)))
            {
                Console.WriteLine($"\t{d.Name}");
            }
        }

        public void Go(string dirName)
        {
            var newPath = Path.Combine(this._currentPath, dirName);
            newPath = Path.GetFullPath(newPath);
            if (Directory.Exists(newPath) || System.IO.File.Exists(newPath))
                this._currentPath = newPath;
        }

        private void TryShowFile()
        {
            if (!System.IO.File.Exists(this._currentPath))
                return;
            
            var extension = Path.GetExtension(_currentPath).ToLower();
            var typeByExtension = readers
                .FirstOrDefault(type => type.CreateInstance<IFileReader>().Extensions.Contains(extension)) 
                ?? typeof(HexFileReader);

            string resultStr = typeByExtension.CreateInstance<IFileReader>().GetContent(this._currentPath);

            Console.Clear();
            Console.WriteLine(resultStr);
            Console.ReadLine();
            this.Go(Path.GetDirectoryName(this._currentPath));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var pv = new PathView();

            do
            {
                pv.Show();
                pv.Go(Console.ReadLine());
            }
            while (true);
        }
    }

    public interface IFileReader
    {
        IEnumerable<string> Extensions { get; }
        
        int Count { get; }
        
        string GetContent(string path);
    }

    public class HexFileReader : IFileReader
    {
        public IEnumerable<string> Extensions => new string[] { };

        public int Count => 2048;

        public string GetContent(string path)
        {
            var buffer = new byte[Count];
            int readCount = 0;
            using (var stream = System.IO.File.OpenRead(path))
            {
                readCount = stream.Read(buffer, 0, Count);
            }
            return BitConverter.ToString(buffer, 0, readCount).Replace('-', ' ');
        }
    }

    public class StringFileReader : IFileReader
    {
        public IEnumerable<string> Extensions => new string[] { ".txt", ".log"};

        public int Count => 1024;

        public string GetContent(string path)
        {
            var buffer = new char[Count];
            using (var stream = System.IO.File.OpenText(path))
            {
                stream.Read(buffer, 0, Count);
            }
            return new string(buffer);
            //return Convert.ToString(buffer[0], 2);
        }
    }

    public class BinaryFileReader : IFileReader
    {
        public IEnumerable<string> Extensions => new string[] { ".exe" };

        public int Count => 512;

        public string GetContent(string path)
        {
            var buffer = new byte[Count];
            using (var stream = System.IO.File.OpenRead(path))
            {
                stream.Read(buffer, 0, Count);
            }
            var builder = new StringBuilder();

            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(Convert.ToString(buffer[i], 2));
            }
            return builder.ToString();
        }
    }

}

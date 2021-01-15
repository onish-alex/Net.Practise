using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ConsoleFileManager
{
    public static class Extensions
    {
        public static IEnumerable<string> GetRelativePaths(this IEnumerable<string> paths, string relativeTo)
        {
            return paths.Select(entry => Path.GetRelativePath(relativeTo, entry));
        }
    }
}

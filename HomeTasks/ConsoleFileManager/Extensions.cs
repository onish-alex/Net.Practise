namespace ConsoleFileManager
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Extensions
    {
        public static IEnumerable<string> GetRelativePaths(this IEnumerable<string> paths, string relativeTo)
        {
            return paths.Select(entry => Path.GetRelativePath(relativeTo, entry));
        }
    }
}

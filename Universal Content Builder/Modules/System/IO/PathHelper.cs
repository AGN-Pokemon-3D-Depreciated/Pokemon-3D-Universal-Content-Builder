using System.IO;

namespace Modules.System.IO
{
    public static class PathHelper
    {
        public static string GetFullPath(this string path)
        {
            return NormalizeFilePath(Path.GetFullPath(path)).TrimEnd('\\', '/');
        }

        public static string NormalizeFilePath(this string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
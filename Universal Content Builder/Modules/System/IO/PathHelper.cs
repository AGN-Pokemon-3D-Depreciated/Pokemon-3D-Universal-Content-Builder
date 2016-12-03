using System.IO;

namespace Universal_Content_Builder.Modules.System.IO
{
    public static class PathHelper
    {
        /// <summary>
        /// Get full path of a selected directory.
        /// </summary>
        /// <param name="path">Path to resolve.</param>
        public static string GetFullPath(this string path)
        {
            return Path.GetFullPath(NormalizeFilePath(path)).TrimEnd('\\', '/');
        }

        /// <summary>
        /// Standardise file path to a universal platform seperator.
        /// </summary>
        /// <param name="path">Path to normalize.</param>
        public static string NormalizeFilePath(this string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
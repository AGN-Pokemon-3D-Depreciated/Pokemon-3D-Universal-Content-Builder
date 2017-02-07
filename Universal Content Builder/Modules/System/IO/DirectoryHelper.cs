using System.IO;

namespace Modules.System.IO
{
    public static class DirectoryHelper
    {
        public static void CreateDirectoryIfNotExists(this string name)
        {
            if (!Directory.Exists(PathHelper.GetFullPath(name)))
                Directory.CreateDirectory(PathHelper.GetFullPath(name));
        }
    }
}
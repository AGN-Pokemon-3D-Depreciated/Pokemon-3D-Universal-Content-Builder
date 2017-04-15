using Amib.Threading;
using Modules.System.IO;
using Modules.YamlDotNet.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Universal_Content_Builder.Core;

namespace Universal_Content_Builder.Content
{
    public class ContentCollection
    {
        public List<Content> ContentFiles { get; set; } = new List<Content>();

        private IWorkItemsGroup ThreadPool;

        public ContentCollection()
        {
            ThreadPool = new SmartThreadPool().CreateWorkItemsGroup(Program.Arguments.NumThread);
        }

        private void LoadOldContentFiles()
        {
            string contentPath = $"{Program.Arguments.IntermediateDirectory}/Content.yml".GetFullPath();

            if (File.Exists(contentPath))
                Program.ContentCollection = contentPath.Deserialize<ContentCollection>() ?? new ContentCollection();
        }

        private void LoadNewContentFiles()
        {
            foreach (string file in Directory.GetFiles(Program.Arguments.WorkingDirectory, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = file.GetFullPath().Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');

                // Ignore folder.
                if (Directory.Exists(file))
                    continue;

                // Global Ignore.
                if (relativePath.ToLower().StartsWith("bin") || relativePath.ToLower().StartsWith("obj"))
                    continue;

                // Git Ignore.
                if (file.ToLower().EndsWith(".gitignore") ||
                    file.ToLower().EndsWith(".git") ||
                    file.ToLower().EndsWith(".cab") ||
                    file.ToLower().EndsWith(".msi") ||
                    file.ToLower().EndsWith(".msm") ||
                    file.ToLower().EndsWith(".msp") ||
                    file.ToLower().EndsWith(".lnk") ||
                    file.ToLower().EndsWith(".directory") ||
                    file.ToLower().EndsWith(".ds_store") ||
                    file.ToLower().EndsWith(".appledouble") ||
                    file.ToLower().EndsWith(".lsoverride") ||
                    file.ToLower().EndsWith(".documentrevisions-v100") ||
                    file.ToLower().EndsWith(".fseventsd") ||
                    file.ToLower().EndsWith(".spotlight-v100") ||
                    file.ToLower().EndsWith(".temporaryitems") ||
                    file.ToLower().EndsWith(".trashes") ||
                    file.ToLower().EndsWith(".volumeicon.icns") ||
                    file.ToLower().EndsWith(".com.apple.timemachine.donotpresent") ||
                    file.ToLower().EndsWith("thumbs.db") ||
                    file.ToLower().EndsWith("ehthumbs.db") ||
                    file.ToLower().EndsWith("desktop.ini") ||
                    file.ToLower().EndsWith(".appledb") ||
                    file.ToLower().EndsWith(".appledesktop") ||
                    file.ToLower().EndsWith(".apdisk") ||
                    file.ToUpper().EndsWith("LICENSE") ||
                    file.ToUpper().EndsWith("README") ||
                    file.ToLower().EndsWith(".mgcb") ||
                    file.ToLower().EndsWith(".ignore") ||
                    file.ToLower().EndsWith(".lock") ||
                    Path.GetFileName(file).ToLower() == "mgcb_tool.exe" ||
                    Path.GetFileName(file).ToLower() == "meta")
                    continue;

                Assets assets = new Assets(file);

                // Model texture Ignore.
                if (relativePath.ToLower().StartsWith("models/".NormalizeFilePath()) ||
                    relativePath.ToLower().Contains("content/models/".NormalizeFilePath()) ||
                    relativePath.ToLower().Contains("sharedresources/models/".NormalizeFilePath()))
                {
                    if (!assets.IsModelAssets())
                        continue;
                }

                if (Program.ContentCollection.ContentFiles.Where(a => string.Equals(a.SourceFile, file, System.StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    Program.ContentCollection.ContentFiles.Add(new Content(file, Program.Arguments.Platform));
            }
        }

        public void BuildAllContent()
        {
            LoadOldContentFiles();
            LoadNewContentFiles();

            Program.ContentCollection.ContentFiles.ForEach(a => ThreadPool.QueueWorkItem(() => a.BuildContent()));
            ThreadPool.WaitForIdle();

            Program.ContentCollection.ContentFiles = Program.ContentCollection.ContentFiles.Where(a => !a.DeleteFlag).ToList();

            if (Program.ContentCollection.ContentFiles.Any(a => a.RebuildFlag))
            {
                Program.ContentCollection.Serialize($"{Program.Arguments.IntermediateDirectory}/Content.yml".GetFullPath());

                foreach (string dir in Directory.GetDirectories(Program.Arguments.OutputDirectory, "*", SearchOption.AllDirectories))
                {
                    if (Directory.Exists(dir))
                    {
                        bool CanDelete = true;

                        foreach (string file in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
                        {
                            if (File.Exists(file))
                            {
                                CanDelete = false;
                                break;
                            }
                        }

                        if (CanDelete)
                            Directory.Delete(dir, true);
                    }
                }
            }
        }
    }
}
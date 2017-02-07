using Amib.Threading;
using Modules.System;
using Modules.System.IO;
using Modules.YamlDotNet.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using Universal_Content_Builder.Core;

namespace Universal_Content_Builder.Content
{
    public class ContentCollection : Dictionary<string, Content>
    {
        private IWorkItemsGroup ThreadPool;

        public ContentCollection(int numThread)
        {
            ThreadPool = new SmartThreadPool().CreateWorkItemsGroup(numThread);
        }

        /// <summary>
        /// Clean the workspace before next build.
        /// </summary>
        /// <param name="forced">Force to clean if object file exist.</param>
        public void CleanContent(bool forced)
        {
            if (Directory.Exists(Program.Arguments.IntermediateDirectory))
            {
                foreach (string file in Directory.GetFiles(Program.Arguments.IntermediateDirectory, "*.yml", SearchOption.AllDirectories))
                {
                    Content OldContent = file.Deserialize<Content>();

                    if (OldContent != null)
                    {
                        if (!File.Exists(OldContent.SourceFile.GetFullPath()))
                        {
                            if (!File.Exists(OldContent.DestinationFile.GetFullPath()))
                                File.Delete(file);
                            else
                            {
                                if (StringHelper.Equals(OldContent.Processor, "ModelProcessor"))
                                    CleanFile(OldContent.DestinationFile.GetFullPath(), true);
                                else
                                    CleanFile(OldContent.DestinationFile.GetFullPath(), false);

                                File.Delete(file);
                            }
                        }

                        if (forced)
                        {
                            if (StringHelper.Equals(OldContent.Processor, "ModelProcessor"))
                                CleanFile(OldContent.DestinationFile.GetFullPath(), true);
                            else
                                CleanFile(OldContent.DestinationFile.GetFullPath(), false);
                        }
                    }
                    else
                        File.Delete(file);
                }

                foreach (string Dir in Directory.GetDirectories(Program.Arguments.IntermediateDirectory, "*", SearchOption.AllDirectories))
                {
                    string RelativePath = Dir.Replace(Program.Arguments.IntermediateDirectory, "").Trim('/', '\\');

                    CleanDirectory((Program.Arguments.OutputDirectory + "/" + RelativePath).GetFullPath());
                    CleanDirectory(Dir);
                }
            }
        }

        private void CleanFile(string path, bool recursive)
        {
            string RelativePath = path.GetFullPath().Replace(Program.Arguments.OutputDirectory, "").Trim('/', '\\');

            if (File.Exists(path.GetFullPath()))
            {
                if (!Program.Arguments.Quiet)
                    Console.WriteLine("Cleaning file: " + RelativePath);

                File.Delete(path.GetFullPath());
            }

            if (File.Exists(path.GetFullPath().Replace(".xnb", ".ogg")))
            {
                if (!Program.Arguments.Quiet)
                    Console.WriteLine("Cleaning file: " + path.GetFullPath().Replace(".xnb", ".ogg"));

                File.Delete(path.GetFullPath().Replace(".xnb", ".ogg"));
            }
            else if (File.Exists(path.GetFullPath().Replace(".xnb", ".wma")))
            {
                if (!Program.Arguments.Quiet)
                    Console.WriteLine("Cleaning file: " + RelativePath.Replace(".xnb", ".wma"));

                File.Delete(path.GetFullPath().Replace(".xnb", ".wma"));
            }

            if (recursive)
            {
                foreach (string tempfile in Directory.GetFiles(path.GetFullPath().Remove(path.LastIndexOfAny(new char[] { '/', '\\' })), "*.*", SearchOption.AllDirectories))
                {
                    RelativePath = tempfile.GetFullPath().Replace(Program.Arguments.OutputDirectory, "").Trim('/', '\\');

                    if (!Program.Arguments.Quiet)
                        Console.WriteLine("Cleaning file: " + RelativePath);

                    File.Delete(tempfile);
                }
            }
        }

        private void CleanDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                bool CanDelete = true;

                foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    if (File.Exists(file))
                    {
                        CanDelete = false;
                        break;
                    }
                }

                if (CanDelete)
                    Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Build the content in the workspace.
        /// </summary>
        public void BuildContent()
        {
#if XNA
            int index = 1;
#endif

            foreach (string file in Directory.GetFiles(Program.Arguments.WorkingDirectory, "*.*", SearchOption.AllDirectories))
            {
                string RelativePath = file.GetFullPath().Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');

                // Ignore folder.
                if (Directory.Exists(file))
                    continue;

                // Global Ignore.
                if (RelativePath.ToLower().StartsWith("bin") || RelativePath.ToLower().StartsWith("obj"))
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
                    file.ToLower().EndsWith(".lock"))
                    continue;

                // Model texture Ignore.
                if (RelativePath.ToLower().StartsWith("content/models".NormalizeFilePath()))
                {
                    if (!file.ToLower().EndsWith(".x") && !file.ToLower().EndsWith(".fbx"))
                        continue;
                }

#if MonoGame
                if (StringHelper.Equals(Program.Arguments.Platform, "DesktopGL", "Windows"))
                {
                    Content TempContent = new Content(new MonoGame.Content.Content(file.GetFullPath()));
                    Add(RelativePath, TempContent);
                    ThreadPool.QueueWorkItem(() => TempContent.Build());
                }
#else
                if (StringHelper.Equals(Program.Arguments.Platform, "XNA"))
                {
                    Content TempContent = new Content(new XNA.Content.Content(index++, file.GetFullPath()));
                    Add(RelativePath, TempContent);
                    ThreadPool.QueueWorkItem(() => TempContent.Build());
                }
#endif
            }

            ThreadPool.WaitForIdle();

#if MonoGame
            // Remove MonoGame Content (obj).
            foreach (string file in Directory.GetFiles(Program.Arguments.IntermediateDirectory, "*.mgcontent", SearchOption.AllDirectories))
                File.Delete(file);
#else
            // Remove XNA Content (obj).
            foreach (string file in Directory.GetFiles(Program.Arguments.IntermediateDirectory, "*.xml", SearchOption.AllDirectories))
                File.Delete(file);

            if (Directory.Exists((Program.Arguments.IntermediateDirectory + "/Xml").GetFullPath()))
                Directory.Delete((Program.Arguments.IntermediateDirectory + "/Xml").GetFullPath(), true);
#endif
        }
    }
}
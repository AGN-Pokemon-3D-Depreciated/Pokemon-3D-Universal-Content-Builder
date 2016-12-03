#if XNA
extern alias XNA;
using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Universal_Content_Builder.Content.Interface;
using Universal_Content_Builder.Core;
using Universal_Content_Builder.Modules.System;
using Universal_Content_Builder.Modules.System.IO;
using XNA::Microsoft.Xna.Framework.Content.Pipeline.Tasks;
using Microsoft.Build.Utilities;

namespace Universal_Content_Builder.XNA.Content
{
    public class Content : BuildContent, IBuildEngine, IContent
    {
        private string SourceFile;
        private string Importer;
        private string Processor;
        private Dictionary<string, string> ProcessorParam = new Dictionary<string, string>();

        public bool ContinueOnError { get { return true; } }

        public int LineNumberOfTaskNode { get { return 0; } }

        public int ColumnNumberOfTaskNode { get { return 0; } }

        public string ProjectFileOfTaskNode { get { return null; } }

        public Content(string file)
        {
            SourceFile = file.GetFullPath();
            string RelativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');

            // Effect Importer - XNA
            if (SourceFile.ToLower().EndsWith(".fx"))
            {
                string Temp;

                using (FileStream FileStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader Reader = new StreamReader(FileStream, Encoding.UTF8))
                        Temp = Reader.ReadToEnd();

                    Temp = Temp.Replace("vs_1_1", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_2_0");
                    Temp = Temp.Replace("vs_2_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_2_0");
                    Temp = Temp.Replace("vs_4_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_2_0");
                    Temp = Temp.Replace("ps_1_1", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_2_0");
                    Temp = Temp.Replace("ps_2_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_2_0");
                    Temp = Temp.Replace("ps_4_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_2_0");
                }

                using (FileStream FileStream = new FileStream(SourceFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter Writer = new StreamWriter(FileStream, Encoding.UTF8))
                        Writer.Write(Temp);
                }

                Importer = "EffectImporter";
                Processor = "EffectProcessor";
            }

            // Fbx Importer - XNA
            if (SourceFile.ToLower().EndsWith(".fbx"))
            {
                Importer = "FbxImporter";
                Processor = "ModelProcessor";
            }

            // Sprite Font Importer - XNA
            if (SourceFile.ToLower().EndsWith(".spritefont"))
            {
                Importer = "FontDescriptionImporter";
                Processor = "FontDescriptionProcessor";
            }

            // Mp3 Importer - XNA
            if (SourceFile.ToLower().EndsWith(".mp3"))
            {
                if (RelativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "Mp3Importer";
                    Processor = "SongProcessor";
                }
                else if (RelativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "Mp3Importer";
                    Processor = "SoundEffectProcessor";
                }
            }

            // Texture Importer - XNA
            if (SourceFile.ToLower().EndsWith(".png") || SourceFile.ToLower().EndsWith(".jpg") || SourceFile.ToLower().EndsWith(".bmp") || SourceFile.ToLower().EndsWith(".tga"))
            {
                if (RelativePath.ToLower().StartsWith("fonts/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/fonts/".NormalizeFilePath()))
                {
                    Importer = "TextureImporter";
                    Processor = "FontTextureProcessor";
                }
                else
                {
                    Importer = "TextureImporter";
                    Processor = "TextureProcessor";
                }
            }

            // Wav Importer - XNA
            if (SourceFile.ToLower().EndsWith(".wav"))
            {
                if (RelativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "WavImporter";
                    Processor = "SongProcessor";
                }
                else if (RelativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "WavImporter";
                    Processor = "SoundEffectProcessor";
                }
            }

            // Wma Importer - XNA
            if (SourceFile.ToLower().EndsWith(".wma"))
            {
                if (RelativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "WmaImporter";
                    Processor = "SongProcessor";
                }
                else if (RelativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "WmaImporter";
                    Processor = "SoundEffectProcessor";
                }
            }

            // Wmv Importer - XNA
            // Not implemented.

            // X Importer - XNA
            if (SourceFile.ToLower().EndsWith(".x"))
            {
                Importer = "XImporter";
                Processor = "ModelProcessor";
            }

            // Xml Importer - XNA
            // Not implemented.
        }

        public string GetSourceFile()
        {
            return SourceFile;
        }

        /// <summary>
        /// Get Content importer.
        /// </summary>
        public string GetImporter()
        {
            return Importer;
        }

        /// <summary>
        /// Get Content processor.
        /// </summary>
        public string GetProcessor()
        {
            return Processor;
        }

        /// <summary>
        /// Get Content processor param.
        /// </summary>
        public Dictionary<string, string> GetProcessorParam()
        {
            return ProcessorParam;
        }

        /// <summary>
        /// Build file.
        /// </summary>
        public bool BuildFile()
        {
            string RelativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');

            TargetPlatform = "Windows";

            if (StringHelper.Equals(Program.Arguments.Profile, "Reach", "HiDef"))
                TargetProfile = Program.Arguments.Profile;

            CompressContent = Program.Arguments.Compress;
            RootDirectory = Program.Arguments.WorkingDirectory;
            OutputDirectory = Program.Arguments.OutputDirectory;
            IntermediateDirectory = Program.Arguments.IntermediateDirectory;
            SourceAssets = new TaskItem[1];

            Dictionary<string, object> metaData = new Dictionary<string, object>();
            metaData.Add("Name", Path.GetFileNameWithoutExtension(SourceFile.GetFullPath()));
            metaData.Add("Importer", Importer);
            metaData.Add("Processor", Processor);
            SourceAssets[0] = new TaskItem(SourceFile, metaData);

            PipelineAssemblies = new TaskItem[]
            {
                new TaskItem(Environment.ExpandEnvironmentVariables("%XNAGSv4%") + "\\References\\Windows\\x86\\Microsoft.Xna.Framework.dll"),
                new TaskItem(Environment.ExpandEnvironmentVariables("%XNAGSv4%") + "\\References\\Windows\\x86\\Microsoft.Xna.Framework.Content.Pipeline.dll"),
                new TaskItem(Environment.ExpandEnvironmentVariables("%XNAGSv4%") + "\\References\\Windows\\x86\\Microsoft.Xna.Framework.Content.Pipeline.AudioImporters.dll"),
                new TaskItem(Environment.ExpandEnvironmentVariables("%XNAGSv4%") + "\\References\\Windows\\x86\\Microsoft.Xna.Framework.Content.Pipeline.EffectImporter.dll"),
                new TaskItem(Environment.ExpandEnvironmentVariables("%XNAGSv4%") + "\\References\\Windows\\x86\\Microsoft.Xna.Framework.Content.Pipeline.FBXImporter.dll"),
                new TaskItem(Environment.ExpandEnvironmentVariables("%XNAGSv4%") + "\\References\\Windows\\x86\\Microsoft.Xna.Framework.Content.Pipeline.TextureImporter.dll"),
                new TaskItem(Environment.ExpandEnvironmentVariables("%XNAGSv4%") + "\\References\\Windows\\x86\\Microsoft.Xna.Framework.Content.Pipeline.VideoImporters.dll"),
                new TaskItem(Environment.ExpandEnvironmentVariables("%XNAGSv4%") + "\\References\\Windows\\x86\\Microsoft.Xna.Framework.Content.Pipeline.XImporter.dll"),
            };

            BuildEngine = this;
            
            try
            {
                if (!Program.Arguments.Quiet)
                    Console.WriteLine("Building: " + RelativePath);

                if (File.Exists(Program.Arguments.IntermediateDirectory + "/ContentPipeline-.xml"))
                    File.Delete(Program.Arguments.IntermediateDirectory + "/ContentPipeline-.xml");

                return Execute();
            }
            catch (Exception ex)
            {
                if (!Program.Arguments.Quiet)
                    Console.WriteLine("Content Error (" + RelativePath + "): " + ex.Message);

                return false;
            }
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            string RelativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');

            if (!Program.Arguments.Quiet)
                Console.Error.WriteLine("Content Error (" + RelativePath + "): " + e.Message);
        }

        public void LogWarningEvent(BuildWarningEventArgs e) { }

        public void LogMessageEvent(BuildMessageEventArgs e) { }

        public void LogCustomEvent(CustomBuildEventArgs e) { }

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs) { return true; }
    }
}
#endif
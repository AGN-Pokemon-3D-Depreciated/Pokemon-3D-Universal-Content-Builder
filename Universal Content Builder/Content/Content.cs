using System;
using System.Collections.Generic;
using System.IO;
using Universal_Content_Builder.Content.Interface;
using Universal_Content_Builder.Core;
using Universal_Content_Builder.Modules.System.IO;
using Universal_Content_Builder.Modules.System.Security.Cryptography;
using Universal_Content_Builder.Modules.YamlDotNet.Serialization;
using YamlDotNet.Serialization;

namespace Universal_Content_Builder.Content
{
    public class Content
    {
        private IContent ContentProcessor;
        private bool MustBuild = false;

        /// <summary>
        /// Get Content source file.
        /// </summary>
        public string SourceFile { get; private set; }

        /// <summary>
        /// Get Content destination file.
        /// </summary>
        public string DestinationFile { get; private set; }

        /// <summary>
        /// Get Content importer.
        /// </summary>
        public string Importer { get; private set; }

        /// <summary>
        /// Get Content processor.
        /// </summary>
        public string Processor { get; private set; }

        /// <summary>
        /// Get Content processor param.
        /// </summary>
        public Dictionary<string, string> ProcessorParam { get; private set; }

        /// <summary>
        /// Get Content output hash checksum.
        /// </summary>
        public string OutputHash { get; private set; }

        /// <summary>
        /// Get Build Status.
        /// </summary>
        [YamlIgnore]
        public bool BuildStatus { get; private set; }

        public Content() { }

        public Content(IContent ContentProcessor)
        {
            this.ContentProcessor = ContentProcessor;
        }

        public void Build()
        {
            try
            {
                SourceFile = ContentProcessor.GetSourceFile();
                Importer = ContentProcessor.GetImporter();
                Processor = ContentProcessor.GetProcessor();
                ProcessorParam = ContentProcessor.GetProcessorParam();

                string RelativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');
                string RelativePathWithoutExtension = RelativePath.Remove(RelativePath.LastIndexOf('.'));
                string RelativeDirWithoutExtension = RelativePath.Contains("/") || RelativePath.Contains("\\") ? RelativePath.Remove(RelativePath.LastIndexOfAny(new char[] { '/', '\\' })) : "";

                if (Importer == null || Processor == null)
                    DestinationFile = (Program.Arguments.OutputDirectory + "/" + RelativePath).GetFullPath();
                else
                    DestinationFile = (Program.Arguments.OutputDirectory + "/" + RelativePathWithoutExtension + ".xnb").GetFullPath();

                OutputHash = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite).ToMD5();

                // Load obj file.
                if (File.Exists((Program.Arguments.IntermediateDirectory + "/" + RelativePath + ".yml").GetFullPath()))
                {
                    Content OldContent = (Program.Arguments.IntermediateDirectory + "/" + RelativePath + ".yml").GetFullPath().Deserialize<Content>();

                    if (OldContent != null)
                    {
                        if (OldContent.OutputHash != OutputHash)
                        {
                            if (File.Exists(OldContent.DestinationFile.GetFullPath()))
                            {
                                if (!Program.Arguments.Quiet)
                                    Console.WriteLine("Cleaning file: " + OldContent.DestinationFile.GetFullPath());

                                File.Delete(OldContent.DestinationFile.GetFullPath());
                            }

                            MustBuild = true;
                        }
                        else if (!File.Exists(OldContent.DestinationFile))
                            MustBuild = true;
                    }
                    else
                        MustBuild = true;
                }
                else
                    MustBuild = true;

                if (MustBuild)
                {
                    // Create obj file.
                    if (!Directory.Exists((Program.Arguments.IntermediateDirectory + "/" + RelativeDirWithoutExtension).GetFullPath()))
                        Directory.CreateDirectory((Program.Arguments.IntermediateDirectory + "/" + RelativeDirWithoutExtension).GetFullPath());

                    SerializerHelper.Serialize(this, (Program.Arguments.IntermediateDirectory + "/" + RelativePath + ".yml").GetFullPath());

                    if (Importer == null || Processor == null)
                    {
                        if (!Directory.Exists((Program.Arguments.OutputDirectory + "/" + RelativeDirWithoutExtension).GetFullPath()))
                            Directory.CreateDirectory((Program.Arguments.OutputDirectory + "/" + RelativeDirWithoutExtension).GetFullPath());

                        if (!Program.Arguments.Quiet)
                            Console.WriteLine("Copying: " + RelativePath);

                        using (FileStream FileStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (FileStream FileStream2 = new FileStream((Program.Arguments.OutputDirectory + "/" + RelativePath).GetFullPath(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                                FileStream.CopyTo(FileStream2);
                        }

                        BuildStatus = true;
                    }
                    else
                        BuildStatus = ContentProcessor.BuildFile();
                }
                else
                {
                    BuildStatus = true;

                    if (!Program.Arguments.Quiet)
                        Console.WriteLine("Skip: " + RelativePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine();
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Dump content into MGCB format.
        /// </summary>
        public override string ToString()
        {
            string RelativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');
            List<string> Result = new List<string>();

            Result.Add("#begin " + RelativePath);

            if (Importer == null || Processor == null)
                Result.Add("/copy:" + RelativePath);
            else
            {
                Result.Add("/importer:" + Importer);
                Result.Add("/processor:" + Processor);

                foreach (KeyValuePair<string, string> item in ProcessorParam)
                    Result.Add("/processorParam:" + item.Key + "=" + item.Value);

                Result.Add("/build:" + RelativePath);
            }

            Result.Add(Environment.NewLine);

            return string.Join(Environment.NewLine, Result);
        }
    }
}

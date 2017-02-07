using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Universal_Content_Builder.Content;
using Modules.System;
using Modules.System.IO;
using Modules.System.Security.Cryptography;

namespace Universal_Content_Builder.Core
{
    public class Program
    {
        public static ArgumentsHandler Arguments;

        public static int Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (obj, e) =>
            {
                string AssemblyToLoad = e.Name.GetSplit(0, ',');

                if (File.Exists((AppDomain.CurrentDomain.BaseDirectory + "/References/" + AssemblyToLoad + ".dll").GetFullPath()))
                    return Assembly.LoadFile((AppDomain.CurrentDomain.BaseDirectory + "/References/" + AssemblyToLoad + ".dll").GetFullPath());
                else
                    return null;
            };

            try
            {
                Arguments = new ArgumentsHandler(args);

                Stopwatch StopWatch = new Stopwatch();
                ContentCollection ContentCollection = new ContentCollection(Arguments.NumThread);

                StopWatch.Start();

                if (Arguments.Rebuild)
                    ContentCollection.CleanContent(true);
                else
                    ContentCollection.CleanContent(false);

                ContentCollection.BuildContent();

                StopWatch.Stop();

                if (ContentCollection.Where(a => a.Value.BuildStatus == false).Count() > 0)
                {
                    Console.WriteLine("Build failed!");
                    return -1;
                }
                else
                {
                    Console.WriteLine("Build completed!");
                    Console.WriteLine("Total Elapsed time: " + StopWatch.Elapsed.ToString());
                }

                GenerateMetaHash(ContentCollection);
                GenerateMGCBFile(ContentCollection);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                return ex.HResult;
            }

            return 0;
        }

        /// <summary>
        /// Extra Component: Meta hash generation.
        /// </summary>
        /// <param name="ContentCollection">Content Collection.</param>
        private static void GenerateMetaHash(ContentCollection ContentCollection)
        {
            if (Arguments.GenerateMetaHash)
            {
                string Temp = null;
                string MetaHash = null;
                long MeasuredSize = 0;
                List<string> FinalResult = new List<string>();

                foreach (KeyValuePair<string, Content.Content> item in ContentCollection.Where(item => !item.Key.ToLower().StartsWith("localization/".NormalizeFilePath()) && !item.Key.ToLower().StartsWith("content/localization/".NormalizeFilePath()) && (item.Key.ToLower().EndsWith(".dat") || item.Key.ToLower().EndsWith(".trainer") || item.Key.ToLower().EndsWith(".poke"))).ToList())
                {
                    string RelativePath = item.Value.SourceFile.Replace(Arguments.WorkingDirectory, "").Trim('/', '\\');

                    using (FileStream FileStream = new FileStream(item.Value.DestinationFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        MeasuredSize += FileStream.Length;

                    FinalResult.Add(RelativePath.Replace("/", "\\") + ":" + item.Value.OutputHash);
                }

                if (!Arguments.Quiet)
                    Console.WriteLine("Generating MetaHash.");

                using (FileStream FileStream = new FileStream((Arguments.OutputDirectory + "/meta").GetFullPath(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter Writer = new StreamWriter(FileStream, new UTF8Encoding(false)) { AutoFlush = true })
                        Writer.Write(string.Join(",", FinalResult));
                }

                MetaHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(new FileStream((Arguments.OutputDirectory + "/meta").GetFullPath(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite).ToMD5()));

                if (!Arguments.Quiet)
                    Console.WriteLine("FileValidation.vb: Meta created! Expected Size: " + MeasuredSize.ToString() + " | MetaHash: " + MetaHash);

                string FileValidationPath;

                if (File.Exists((Arguments.WorkingDirectory + "/../2.5DHero/Security/FileValidation.vb").GetFullPath()))
                    FileValidationPath = (Arguments.WorkingDirectory + "/../2.5DHero/Security/FileValidation.vb").GetFullPath();
                else if (File.Exists((Arguments.WorkingDirectory + "/../../2.5DHero/Security/FileValidation.vb").GetFullPath()))
                    FileValidationPath = (Arguments.WorkingDirectory + "/../../2.5DHero/Security/FileValidation.vb").GetFullPath();
                else if (File.Exists((Arguments.WorkingDirectory + "/../../../2.5DHero/Security/FileValidation.vb").GetFullPath()))
                    FileValidationPath = (Arguments.WorkingDirectory + "/../../../2.5DHero/Security/FileValidation.vb").GetFullPath();
                else
                    return;

                if (!Arguments.Quiet)
                    Console.WriteLine("Generating FileValidation.vb");

                using (FileStream FileStream = new FileStream(FileValidationPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader Reader = new StreamReader(FileStream))
                        Temp = Reader.ReadToEnd();
                }

                string[] Temp2 = Temp.Split('\n');

                for (int i = 0; i < Temp2.Length; i++)
                {
                    if (Temp2[i].Trim().Contains("Const EXPECTEDSIZE As Integer ="))
                        Temp2[i] = "        Const EXPECTEDSIZE As Integer = " + MeasuredSize.ToString() + "\r";
                    else if (Temp2[i].Trim().Contains("Const METAHASH As String ="))
                        Temp2[i] = "        Const METAHASH As String = \"" + MetaHash + "\"" + "\r";
                }

                using (FileStream FileStream = new FileStream(FileValidationPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter Writer = new StreamWriter(FileStream, Encoding.UTF8) { AutoFlush = true })
                        Writer.Write(string.Join("\n", Temp2));
                }
            }
        }

        /// <summary>
        /// Extra Component: MGCB file generation.
        /// </summary>
        /// <param name="ContentCollection">Content Collection.</param>
        private static void GenerateMGCBFile(ContentCollection ContentCollection)
        {
            if (Arguments.GenerateMGCB)
            {
                if (!Arguments.Quiet)
                    Console.WriteLine("Generating Content.mgcb");

                using (FileStream FileStream = new FileStream((Arguments.WorkingDirectory + "/Content.mgcb").GetFullPath(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter Writer = new StreamWriter(FileStream, Encoding.UTF8) { AutoFlush = true })
                    {
                        Writer.Write(Arguments.GenerateMGCBProperty());

                        foreach (KeyValuePair<string, Content.Content> item in ContentCollection)
                            Writer.Write(item.Value.ToString());
                    }
                }
            }
        }
    }
}
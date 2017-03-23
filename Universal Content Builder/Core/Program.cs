using Modules.System;
using Modules.System.IO;
using Modules.System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Universal_Content_Builder.Content;

namespace Universal_Content_Builder.Core
{
    public class Program
    {
        public static ArgumentsHandler Arguments { get; private set; }
        public static ContentCollection ContentCollection { get; set; }

        public static int Main(string[] args)
        {
            try
            {
                Arguments = new ArgumentsHandler(args);
                ContentCollection = new ContentCollection();

                Stopwatch StopWatch = new Stopwatch();
                StopWatch.Start();

                ContentCollection.BuildAllContent();

                StopWatch.Stop();

                if (ContentCollection.ContentFiles.Any(a => !a.BuildSuccess))
                {
                    Console.WriteLine("Build failed!");
                    Console.WriteLine("Total Elapsed time: " + StopWatch.Elapsed.ToString());
                    return -1;
                }
                else
                {
                    Console.WriteLine("Build completed!");
                    Console.WriteLine("Total Elapsed time: " + StopWatch.Elapsed.ToString());
                }

                GenerateMetaHash();
                GenerateMGCBFile();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                return ex.HResult;
            }

            return 0;
        }

        private static void GenerateMetaHash()
        {
            if (Arguments.GenerateMetaHash)
            {
                string Temp = null;
                string MetaHash = null;
                long MeasuredSize = 0;
                List<string> FinalResult = new List<string>();

                foreach (Content.Content item in ContentCollection.ContentFiles.Where(item =>
                {
                    string relativePath = item.SourceFile.Replace(Arguments.WorkingDirectory, "").Trim('/', '\\');
                    return !relativePath.ToLower().StartsWith("content/localization/".NormalizeFilePath()) && (relativePath.ToLower().EndsWith(".dat") || relativePath.ToLower().EndsWith(".trainer") || relativePath.ToLower().EndsWith(".poke"));
                }))
                {
                    string relativePath = item.SourceFile.Replace(Arguments.WorkingDirectory, "").Trim('/', '\\');

                    using (FileStream FileStream = new FileStream(item.DestinationFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        MeasuredSize += FileStream.Length;

                    FinalResult.Add(relativePath.Replace("/", "\\") + ":" + item.MetaHash);
                }

                if (!Arguments.Quiet)
                    Console.WriteLine("Generating MetaHash.");

                using (StreamWriter Writer = new StreamWriter(new FileStream((Arguments.OutputDirectory + "/meta").GetFullPath(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite), new UTF8Encoding(false)) { AutoFlush = true })
                    Writer.Write(string.Join(",", FinalResult));

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

                using (StreamReader Reader = new StreamReader(new FileStream(FileValidationPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    Temp = Reader.ReadToEnd();

                string[] Temp2 = Temp.Split('\n');

                for (int i = 0; i < Temp2.Length; i++)
                {
                    if (Temp2[i].Trim().Contains("Const EXPECTEDSIZE As Integer ="))
                        Temp2[i] = "        Const EXPECTEDSIZE As Integer = " + MeasuredSize.ToString() + "\r";
                    else if (Temp2[i].Trim().Contains("Const METAHASH As String ="))
                        Temp2[i] = "        Const METAHASH As String = \"" + MetaHash + "\"" + "\r";
                }

                using (StreamWriter Writer = new StreamWriter(new FileStream(FileValidationPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8) { AutoFlush = true })
                    Writer.Write(string.Join("\n", Temp2));
            }
        }

        private static void GenerateMGCBFile()
        {
            if (Arguments.GenerateMGCB)
            {
                if (!Arguments.Quiet)
                    Console.WriteLine("Generating Content.mgcb");

                using (StreamWriter Writer = new StreamWriter(new FileStream((Arguments.WorkingDirectory + "/Content.mgcb").GetFullPath(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8) { AutoFlush = true })
                {
                    Writer.Write(Arguments.GenerateMGCBProperty());

                    foreach (Content.Content item in ContentCollection.ContentFiles)
                        Writer.Write(item.ToString());
                }
            }
        }
    }
}
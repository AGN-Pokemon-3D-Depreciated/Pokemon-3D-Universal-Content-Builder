using Modules.System;
using Modules.System.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Universal_Content_Builder.Core
{
    public class ArgumentsHandler
    {
        #region Build Directory

        public string WorkingDirectory { get; private set; }
        public string OutputDirectory { get; private set; }
        public string IntermediateDirectory { get; private set; }

        #endregion Build Directory

        #region Build Config

        public string Platform { get; private set; }
        public string Profile { get; private set; }
        public bool Compress { get; private set; }
        public int NumThread { get; private set; }
        public bool Rebuild { get; private set; }
        public bool Quiet { get; private set; }

        #endregion Build Config

        #region Additional Features

        public bool GenerateMetaHash { get; private set; }
        public bool GenerateMGCB { get; private set; }

        #endregion Additional Features

        public Version BuildTool { get; private set; }

        public ArgumentsHandler(string[] args)
        {
            WorkingDirectory = Environment.CurrentDirectory.GetFullPath();

#if MonoGame
            Platform = "DesktopGL";
#else
            Platform = "XNA";
#endif
            Profile = "Reach";
            Compress = true;
            NumThread = Environment.ProcessorCount;
            Rebuild = false;
            Quiet = false;

            OutputDirectory = (WorkingDirectory + "/bin/" + Platform).GetFullPath();
            IntermediateDirectory = (WorkingDirectory + "/obj/" + Platform).GetFullPath();

            GenerateMetaHash = false;
            GenerateMGCB = false;

            foreach (KeyValuePair<string, string> item in ProcessArguement(args))
            {
                try
                {
                    if (item.Key.Equals("Content", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("ContentDir", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("ContentDirectory", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("WorkingDir", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("WorkingDirectory", StringComparison.OrdinalIgnoreCase))
                        WorkingDirectory = item.Value.GetFullPath();
                    else if (item.Key.Equals("Bin", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("BinDir", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("BinDirectory", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("OutputDir", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("OutputDirectory", StringComparison.OrdinalIgnoreCase))
                        OutputDirectory = item.Value.GetFullPath();
                    else if (item.Key.Equals("Obj", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("ObjDir", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("ObjDirectory", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("IntermediateDir", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("IntermediateDirectory", StringComparison.OrdinalIgnoreCase))
                        IntermediateDirectory = item.Value.GetFullPath();
                    else if (item.Key.Equals("Platform", StringComparison.OrdinalIgnoreCase))
                        Platform = item.Value;
                    else if (item.Key.Equals("Profile", StringComparison.OrdinalIgnoreCase))
                        Profile = item.Value;
                    else if (item.Key.Equals("Compress", StringComparison.OrdinalIgnoreCase))
                        Compress = item.Value.ToBoolean();
                    else if (item.Key.Equals("Thread", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("NumThread", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("NumberOfThread", StringComparison.OrdinalIgnoreCase))
                        NumThread = item.Value.ToInt32();
                    else if (item.Key.Equals("Rebuild", StringComparison.OrdinalIgnoreCase))
                        Rebuild = item.Value.ToBoolean();
                    else if (item.Key.Equals("Quiet", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("Slient", StringComparison.OrdinalIgnoreCase))
                        Quiet = item.Value.ToBoolean();
                    else if (item.Key.Equals("DumpMeta", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("DumpMetaHash", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("GenMeta", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("GenerateMeta", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("GenMetaHash", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("GenerateMataHash", StringComparison.OrdinalIgnoreCase))
                        GenerateMetaHash = item.Value.ToBoolean();
                    else if (item.Key.Equals("DumpMGCB", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("GenMGCB", StringComparison.OrdinalIgnoreCase) ||
                        item.Key.Equals("GenerateMGCB", StringComparison.OrdinalIgnoreCase))
                        GenerateMGCB = item.Value.ToBoolean();
                }
                catch (Exception) { }
            }

            if (!Directory.Exists(WorkingDirectory))
            {
                Console.Error.WriteLine("Invalid working directory.");
                Environment.Exit(-1);
            }

            if (Platform.Equals("XNA", StringComparison.OrdinalIgnoreCase))
            {
                if (Environment.Is64BitProcess)
                {
                    Console.Error.WriteLine("The XNA content tools only work on a 32 bit application.");
                    Environment.Exit(-1);
                }
                else if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                {
                    Console.Error.WriteLine("The XNA content tools only work on a windows platform.");
                    Environment.Exit(-1);
                }
                else if (Environment.ExpandEnvironmentVariables("%XNAGSv4%").Equals("%XNAGSv4%", StringComparison.OrdinalIgnoreCase) || !Directory.Exists(Environment.ExpandEnvironmentVariables("%XNAGSv4%").GetFullPath()))
                {
                    Console.Error.WriteLine("Unable to find XNA game studio 4.0.");
                    Environment.Exit(-1);
                }

                BuildTool = new Version(1, 4, 0, 0);
            }
            else if (Platform.Equals("DesktopGL", StringComparison.OrdinalIgnoreCase) || Platform.Equals("Windows", StringComparison.OrdinalIgnoreCase))
            {
                if (!Environment.Is64BitProcess)
                {
                    Console.Error.WriteLine("The MonoGame content tools only work on a 64 bit application.");
                    Environment.Exit(-1);
                }
                else if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                {
                    Console.Error.WriteLine("The MonoGame content tools only work on a windows platform.");
                    Environment.Exit(-1);
                }

                BuildTool = Assembly.LoadFile((AppDomain.CurrentDomain.BaseDirectory + "/MonoGame.Framework.Content.Pipeline.dll").GetFullPath()).GetName().Version;
            }
            else
            {
                Console.Error.WriteLine("Unknown Platform.");
                Environment.Exit(-1);
            }

            if (!Profile.Equals("Reach", StringComparison.OrdinalIgnoreCase) && !Profile.Equals("HiDef", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Unknown Profile.");
                Environment.Exit(-1);
            }

            OutputDirectory.CreateDirectoryIfNotExists();
            IntermediateDirectory.CreateDirectoryIfNotExists();
        }

        public string GenerateMGCBProperty()
        {
            List<string> Result = new List<string>
            {
                "",
                "#----------------------------- Global Properties ----------------------------#",
                "",
                "/outputDir:bin",
                "/intermediateDir:obj",
                "/platform:" + Platform,
                "/config:",
                "/profile:" + Profile,
                "/compress:" + Compress.ToString(),
                "",
                "#-------------------------------- References --------------------------------#",
                "",
                "",
                "#---------------------------------- Content ---------------------------------#",
                Environment.NewLine
            };
            return string.Join(Environment.NewLine, Result);
        }

        private Dictionary<string, string> ProcessArguement(string[] args)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            string TempName, TempValue;

            foreach (string item in args)
            {
                if (item.StartsWith("/") && item.Contains(":"))
                {
                    TempName = item.Remove(item.IndexOf(':')).Remove(0, 1);
                    TempValue = item.Remove(0, item.IndexOf(':') + 1).Trim('\'', '"');
                    Result.Add(TempName, TempValue);
                }
            }

            return Result;
        }
    }
}
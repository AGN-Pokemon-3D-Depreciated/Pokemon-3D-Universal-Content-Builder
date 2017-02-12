﻿using Modules.System;
using Modules.System.IO;
using System;
using System.Collections.Generic;
using System.IO;

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

        public ArgumentsHandler(string[] args)
        {
            WorkingDirectory = Environment.CurrentDirectory.GetFullPath();
            OutputDirectory = (WorkingDirectory + "/bin/").GetFullPath();
            IntermediateDirectory = (WorkingDirectory + "/obj/").GetFullPath();

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

            GenerateMetaHash = false;
            GenerateMGCB = false;

            foreach (KeyValuePair<string, string> item in ProcessArguement(args))
            {
                try
                {
                    if (StringHelper.Equals(item.Key, "Content", "ContentDir", "ContentDirectory", "WorkingDir", "WorkingDirectory"))
                        WorkingDirectory = item.Value.GetFullPath();
                    else if (StringHelper.Equals(item.Key, "Bin", "BinDir", "OutputDir", "OutputDirectory"))
                        OutputDirectory = item.Value.GetFullPath();
                    else if (StringHelper.Equals(item.Key, "Obj", "ObjDir", "IntermediateDir", "IntermediateDirectory"))
                        IntermediateDirectory = item.Value.GetFullPath();
                    else if (StringHelper.Equals(item.Key, "Platform"))
                        Platform = item.Value;
                    else if (StringHelper.Equals(item.Key, "Profile"))
                        Profile = item.Value;
                    else if (StringHelper.Equals(item.Key, "Compress"))
                        Compress = Convert.ToBoolean(item.Value);
                    else if (StringHelper.Equals(item.Key, "Thread", "NumThread", "NumberOfThread"))
                        NumThread = Convert.ToInt32(item.Value);
                    else if (StringHelper.Equals(item.Key, "Rebuild"))
                        Rebuild = Convert.ToBoolean(item.Value);
                    else if (StringHelper.Equals(item.Key, "Quiet", "Slient"))
                        Quiet = Convert.ToBoolean(item.Value);
                    else if (StringHelper.Equals(item.Key, "DumpMeta", "DumpMetaHash", "GenMeta", "GenerateMeta", "GenMetaHash", "GenerateMataHash"))
                        GenerateMetaHash = Convert.ToBoolean(item.Value);
                    else if (StringHelper.Equals(item.Key, "DumpMGCB", "GenMGCB", "GenerateMGCB"))
                        GenerateMGCB = Convert.ToBoolean(item.Value);
                }
                catch (Exception) { }
            }

            if (!Directory.Exists(WorkingDirectory))
            {
                Console.Error.WriteLine("Invalid working directory.");
                Environment.Exit(-1);
            }
            else if (StringHelper.Equals(Platform, "XNA"))
            {
                if (Environment.Is64BitProcess || Environment.OSVersion.Platform != PlatformID.Win32NT)
                {
                    Console.Error.WriteLine("The XNA content tools only work on a 32 bit application.");
                    Environment.Exit(-1);
                }
                else if (StringHelper.Equals(Environment.ExpandEnvironmentVariables("%XNAGSv4%"), "%XNAGSv4%") || !Directory.Exists(Environment.ExpandEnvironmentVariables("%XNAGSv4%").GetFullPath()))
                {
                    Console.Error.WriteLine("Unable to find XNA game studio 4.0.");
                    Environment.Exit(-1);
                }
            }
            else if (StringHelper.Equals(Platform, "DesktopGL", "Windows"))
            {
                if (!Environment.Is64BitProcess && Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    Console.Error.WriteLine("The MonoGame content tools only work on a 64 bit application.");
                    Environment.Exit(-1);
                }
            }
            else if (!StringHelper.Equals(Platform, "XNA", "DesktopGL", "Windows"))
            {
                Console.Error.WriteLine("Unknown Platform.");
                Environment.Exit(-1);
            }
            else if (!StringHelper.Equals(Profile, "Reach", "HiDef"))
            {
                Console.Error.WriteLine("Unknown Profile.");
                Environment.Exit(-1);
            }
            else
            {
                if (!Directory.Exists(OutputDirectory))
                    Directory.CreateDirectory(OutputDirectory);

                if (!Directory.Exists(IntermediateDirectory))
                    Directory.CreateDirectory(OutputDirectory);
            }
        }

        public string GenerateMGCBProperty()
        {
            List<string> Result = new List<string>();

            Result.Add("");
            Result.Add("#----------------------------- Global Properties ----------------------------#");
            Result.Add("");
            Result.Add("/outputDir:bin");
            Result.Add("/intermediateDir:obj");
            Result.Add("/platform:" + Platform);
            Result.Add("/config:");
            Result.Add("/profile:" + Profile);
            Result.Add("/compress:" + Compress.ToString());
            Result.Add("");
            Result.Add("#-------------------------------- References --------------------------------#");
            Result.Add("");
            Result.Add("");
            Result.Add("#---------------------------------- Content ---------------------------------#");
            Result.Add(Environment.NewLine);

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
extern alias MonoGame;

using Modules.System;
using Modules.System.IO;
using Modules.System.Security.Cryptography;
using MonoGame::Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame::Microsoft.Xna.Framework.Graphics;
using MonoGame::MonoGame.Framework.Content.Pipeline.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Universal_Content_Builder.Core;
using YamlDotNet.Serialization;

namespace Universal_Content_Builder.Content
{
    public class Content
    {
        public string BuildTool { get; set; }

        public string SourceFile { get; set; }
        public string DestinationFile { get; set; }
        public string Importer { get; set; }
        public string Processor { get; set; }
        public Dictionary<string, string> ProcessorParam { get; set; } = new Dictionary<string, string>();

        public string MetaHash { get; set; }

        public List<string> BuildOutput { get; set; } = new List<string>();
        public List<string> BuildAsset { get; set; } = new List<string>();

        [YamlIgnore]
        public bool DeleteFlag { get; private set; } = false;

        [YamlIgnore]
        public bool BuildSuccess { get; private set; } = false;

        [YamlIgnore]
        public bool RebuildFlag { get; private set; } = false;

        private bool FirstBuild = false;

        private string Platform;

        public Content()
        {
        }

        public Content(string sourceFile, string platform)
        {
            SourceFile = sourceFile;
            Platform = platform;
            BuildTool = Program.Arguments.BuildTool.ToString();
            FirstBuild = true;
            RebuildFlag = true;
        }

        private void CleanFile()
        {
            List<string> fileToRemove = new List<string>();

            if (DestinationFile != null)
                fileToRemove.Add(DestinationFile.GetFullPath());

            fileToRemove = fileToRemove.Union(BuildOutput).Union(BuildAsset).ToList();

            foreach (string file in fileToRemove)
            {
                if (File.Exists(file))
                {
                    if (!Program.Arguments.Quiet)
                        Console.WriteLine($"Cleaning file: {file}");

                    File.Delete(file);
                }
            }
        }

        public void BuildContent()
        {
            try
            {
                Assets assets = new Assets(SourceFile);
                string relativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');
                string relativePathWithoutExtension = relativePath.Contains(".") ? relativePath.Remove(relativePath.LastIndexOf('.')) : relativePath;
                string relativeDirWithoutExtension = relativePath.Contains("/") || relativePath.Contains("\\") ? relativePath.Remove(relativePath.LastIndexOfAny(new char[] { '/', '\\' })) : "";
                string tempImporter = null;
                string tempProcessor = null;
                Dictionary<string, string> tempProcessorParam = new Dictionary<string, string>();

                if (!FirstBuild)
                {
                    tempImporter = Importer;
                    tempProcessor = Processor;
                    tempProcessorParam.Union(ProcessorParam);
                    ProcessorParam.Clear();

                    if (!File.Exists(SourceFile))
                        DeleteFlag = true;
                    else if (!BuildTool.Equals(Program.Arguments.BuildTool.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        BuildTool = Program.Arguments.BuildTool.ToString();
                        RebuildFlag = true;
                    }
                    else
                    {
                        string newHash;

                        using (FileStream fileStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            newHash = fileStream.ToMD5();

                        if (!MetaHash.Equals(newHash, StringComparison.OrdinalIgnoreCase))
                        {
                            MetaHash = newHash;
                            RebuildFlag = true;
                        }
                        else
                        {
                            List<string> fileToCheck = new List<string>();

                            if (DestinationFile != null)
                                fileToCheck.Add(DestinationFile.GetFullPath());

                            fileToCheck = fileToCheck.Union(BuildOutput).Union(BuildAsset).ToList();

                            foreach (string file in fileToCheck)
                            {
                                if (!File.Exists(file))
                                {
                                    RebuildFlag = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (FileStream fileStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        MetaHash = fileStream.ToMD5();
                }

#if MonoGame
                if (assets.IsEffectAssets())
                {
                    // Effect Importer - MonoGame
                    string temp;

                    using (StreamReader reader = new StreamReader(new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.UTF8))
                        temp = reader.ReadToEnd();

                    temp = temp.Replace("vs_1_1", Program.Arguments.Platform.Equals("DesktopGL", StringComparison.OrdinalIgnoreCase) ? "vs_2_0" : "vs_4_0");
                    temp = temp.Replace("vs_2_0", Program.Arguments.Platform.Equals("DesktopGL", StringComparison.OrdinalIgnoreCase) ? "vs_2_0" : "vs_4_0");
                    temp = temp.Replace("vs_4_0", Program.Arguments.Platform.Equals("DesktopGL", StringComparison.OrdinalIgnoreCase) ? "vs_2_0" : "vs_4_0");
                    temp = temp.Replace("ps_1_1", Program.Arguments.Platform.Equals("DesktopGL", StringComparison.OrdinalIgnoreCase) ? "ps_2_0" : "ps_4_0");
                    temp = temp.Replace("ps_2_0", Program.Arguments.Platform.Equals("DesktopGL", StringComparison.OrdinalIgnoreCase) ? "ps_2_0" : "ps_4_0");
                    temp = temp.Replace("ps_4_0", Program.Arguments.Platform.Equals("DesktopGL", StringComparison.OrdinalIgnoreCase) ? "ps_2_0" : "ps_4_0");

                    using (StreamWriter Writer = new StreamWriter(new FileStream(SourceFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8) { AutoFlush = true })
                        Writer.Write(temp);

                    temp = null;

                    Importer = "EffectImporter";
                    Processor = "EffectProcessor";
                    ProcessorParam.Add("DebugMode", "Auto");
                }
                else if (assets.IsFbxModelAssets())
                {
                    // Fbx Importer - MonoGame
                    Importer = "FbxImporter";
                    Processor = "ModelProcessor";
                    ProcessorParam.Add("ColorKeyColor", "0,0,0,0");
                    ProcessorParam.Add("ColorKeyEnabled", "True");
                    ProcessorParam.Add("DefaultEffect", "BasicEffect");
                    ProcessorParam.Add("GenerateMipmaps", "True");
                    ProcessorParam.Add("GenerateTangentFrames", "False");
                    ProcessorParam.Add("PremultiplyTextureAlpha", "True");
                    ProcessorParam.Add("PremultiplyVertexColors", "True");
                    ProcessorParam.Add("ResizeTexturesToPowerOfTwo", "False");
                    ProcessorParam.Add("RotationX", "0");
                    ProcessorParam.Add("RotationY", "0");
                    ProcessorParam.Add("RotationZ", "0");
                    ProcessorParam.Add("Scale", "1");
                    ProcessorParam.Add("SwapWindingOrder", "False");
                    ProcessorParam.Add("TextureFormat", "Color");
                }
                else if (assets.IsXModelAssets())
                {
                    // X Importer - MonoGame
                    Importer = "XImporter";
                    Processor = "ModelProcessor";
                    ProcessorParam.Add("ColorKeyColor", "0,0,0,0");
                    ProcessorParam.Add("ColorKeyEnabled", "True");
                    ProcessorParam.Add("DefaultEffect", "BasicEffect");
                    ProcessorParam.Add("GenerateMipmaps", "True");
                    ProcessorParam.Add("GenerateTangentFrames", "False");
                    ProcessorParam.Add("PremultiplyTextureAlpha", "True");
                    ProcessorParam.Add("PremultiplyVertexColors", "True");
                    ProcessorParam.Add("ResizeTexturesToPowerOfTwo", "False");
                    ProcessorParam.Add("RotationX", "0");
                    ProcessorParam.Add("RotationY", "0");
                    ProcessorParam.Add("RotationZ", "0");
                    ProcessorParam.Add("Scale", "1");
                    ProcessorParam.Add("SwapWindingOrder", "False");
                    ProcessorParam.Add("TextureFormat", "Color");
                }
                else if (assets.IsOpenModelAssets())
                {
                    // Open Asset Import Library - MonoGame
                    Importer = "OpenAssetImporter";
                    Processor = "ModelProcessor";
                    ProcessorParam.Add("ColorKeyColor", "0,0,0,0");
                    ProcessorParam.Add("ColorKeyEnabled", "True");
                    ProcessorParam.Add("DefaultEffect", "BasicEffect");
                    ProcessorParam.Add("GenerateMipmaps", "True");
                    ProcessorParam.Add("GenerateTangentFrames", "False");
                    ProcessorParam.Add("PremultiplyTextureAlpha", "True");
                    ProcessorParam.Add("PremultiplyVertexColors", "True");
                    ProcessorParam.Add("ResizeTexturesToPowerOfTwo", "False");
                    ProcessorParam.Add("RotationX", "0");
                    ProcessorParam.Add("RotationY", "0");
                    ProcessorParam.Add("RotationZ", "0");
                    ProcessorParam.Add("Scale", "1");
                    ProcessorParam.Add("SwapWindingOrder", "False");
                    ProcessorParam.Add("TextureFormat", "Color");
                }
                else if (assets.IsSpriteFontAssets())
                {
                    // Sprite Font Importer - MonoGame
                    Importer = "FontDescriptionImporter";
                    Processor = "FontDescriptionProcessor";
                    ProcessorParam.Add("TextureFormat", "Color");
                }
                else if (assets.IsTextureAssets())
                {
                    // Texture Importer - MonoGame
                    if (assets.IsFontAssets())
                    {
                        Importer = "TextureImporter";
                        Processor = "FontTextureProcessor";
                        ProcessorParam.Add("FirstCharacter", " ");
                        ProcessorParam.Add("PremultiplyAlpha", "True");
                        ProcessorParam.Add("TextureFormat", "Color");
                    }
                    else
                    {
                        Importer = "TextureImporter";
                        Processor = "TextureProcessor";
                        ProcessorParam.Add("ColorKeyColor", "255,0,255,255");
                        ProcessorParam.Add("ColorKeyEnabled", "True");
                        ProcessorParam.Add("GenerateMipmaps", "False");
                        ProcessorParam.Add("PremultiplyAlpha", "True");
                        ProcessorParam.Add("ResizeToPowerOfTwo", "False");
                        ProcessorParam.Add("MakeSquare", "False");
                        ProcessorParam.Add("TextureFormat", "Color");
                    }
                }
                else if (assets.IsMp3Assets())
                {
                    // Mp3 Importer - MonoGame
                    if (assets.IsMusicAssets())
                    {
                        Importer = "Mp3Importer";
                        Processor = "SongProcessor";
                        ProcessorParam.Add("Quality", "Low");
                    }
                    else if (assets.IsSoundAssets())
                    {
                        Importer = "Mp3Importer";
                        Processor = "SoundEffectProcessor";
                        ProcessorParam.Add("Quality", "Low");
                    }
                }
                else if (assets.IsOggAssets())
                {
                    // Ogg Importer - MonoGame
                    if (assets.IsMusicAssets())
                    {
                        Importer = "OggImporter";
                        Processor = "SongProcessor";
                        ProcessorParam.Add("Quality", "Low");
                    }
                    else if (assets.IsSoundAssets())
                    {
                        Importer = "OggImporter";
                        Processor = "SoundEffectProcessor";
                        ProcessorParam.Add("Quality", "Low");
                    }
                }
                else if (assets.IsWavAssets())
                {
                    // Wav Importer - MonoGame
                    if (assets.IsMusicAssets())
                    {
                        Importer = "WavImporter";
                        Processor = "SongProcessor";
                        ProcessorParam.Add("Quality", "Low");
                    }
                    else if (assets.IsSoundAssets())
                    {
                        Importer = "WavImporter";
                        Processor = "SoundEffectProcessor";
                        ProcessorParam.Add("Quality", "Low");
                    }
                }
                else if (assets.IsWmaAssets())
                {
                    // Wma Importer - MonoGame
                    if (assets.IsMusicAssets())
                    {
                        Importer = "WmaImporter";
                        Processor = "SongProcessor";
                        ProcessorParam.Add("Quality", "Low");
                    }
                    else if (assets.IsSoundAssets())
                    {
                        Importer = "WmaImporter";
                        Processor = "SoundEffectProcessor";
                        ProcessorParam.Add("Quality", "Low");
                    }
                }
                else if (assets.IsMp4Assets())
                {
                    // H.264 Video - MonoGame
                    Importer = "H264Importer";
                    Processor = "VideoProcessor";
                }
                else if (assets.IsWmvAssets())
                {
                    // Wmv Importer - MonoGame
                    Importer = "WmvImporter";
                    Processor = "VideoProcessor";
                }
                else if (assets.IsXMLAssets())
                {
                    // Xml Importer - MonoGame
                    Importer = "XmlImporter";
                    Processor = "PassThroughProcessor";
                }

                if (!FirstBuild)
                {
                    if (Importer != null && Processor != null)
                    {
                        if (!tempImporter.Equals(Importer, StringComparison.OrdinalIgnoreCase) ||
                        !tempProcessor.Equals(Processor, StringComparison.OrdinalIgnoreCase) ||
                        tempProcessorParam.Select(a => a.Key + "=" + a.Value).Join(";").Equals(ProcessorParam.Select(a => a.Key + "=" + a.Value).Join(";"), StringComparison.OrdinalIgnoreCase))
                            RebuildFlag = true;
                    }

                    tempImporter = null;
                    tempProcessor = null;
                    tempProcessorParam = null;
                }
#endif

                if (RebuildFlag || DeleteFlag)
                    CleanFile();

                if (Importer.IsNullOrEmpty() || Processor.IsNullOrEmpty())
                    DestinationFile = (Program.Arguments.OutputDirectory + "/" + relativePath).GetFullPath();
                else
                    DestinationFile = (Program.Arguments.OutputDirectory + "/" + relativePathWithoutExtension + ".xnb").GetFullPath();

                if (RebuildFlag || Program.Arguments.Rebuild)
                {
                    if (Importer.IsNullOrEmpty() || Processor.IsNullOrEmpty())
                    {
                        DirectoryHelper.CreateDirectoryIfNotExists((Program.Arguments.OutputDirectory + "/" + relativeDirWithoutExtension).GetFullPath());

                        if (!Program.Arguments.Quiet)
                            Console.WriteLine("Copying: " + relativePath);

                        using (FileStream fileStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (FileStream fileStream2 = new FileStream((Program.Arguments.OutputDirectory + "/" + relativePath).GetFullPath(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                                fileStream.CopyTo(fileStream2);
                        }

                        BuildSuccess = true;
                    }
                    else
                    {
#if MonoGame
                        PipelineManager manager = new PipelineManager(Program.Arguments.WorkingDirectory, Program.Arguments.OutputDirectory, Program.Arguments.IntermediateDirectory)
                        {
                            CompressContent = Program.Arguments.Compress
                        };

                        if (Program.Arguments.Platform.Equals("DesktopGL", StringComparison.OrdinalIgnoreCase))
                            manager.Platform = TargetPlatform.DesktopGL;
                        else if (Program.Arguments.Platform.Equals("Windows", StringComparison.OrdinalIgnoreCase))
                            manager.Platform = TargetPlatform.Windows;

                        if (Program.Arguments.Profile.Equals("Reach", StringComparison.OrdinalIgnoreCase))
                            manager.Profile = GraphicsProfile.Reach;
                        else if (Program.Arguments.Profile.Equals("HiDef", StringComparison.OrdinalIgnoreCase))
                            manager.Profile = GraphicsProfile.HiDef;

                        OpaqueDataDictionary Param = new OpaqueDataDictionary();

                        foreach (KeyValuePair<string, string> item in ProcessorParam)
                            Param.Add(item.Key, item.Value);

                        try
                        {
                            if (!Program.Arguments.Quiet)
                                Console.WriteLine("Building: " + relativePath);

                            PipelineBuildEvent buildResult = manager.BuildContent(SourceFile, null, Importer, Processor, Param);

                            BuildAsset = buildResult.BuildAsset;
                            BuildOutput = buildResult.BuildOutput;

                            BuildSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            if (!Program.Arguments.Quiet)
                                Console.Error.WriteLine("Content Error (" + relativePath + "): " + ex.Message);

                            BuildSuccess = false;
                        }
#endif
                    }
                }
                else if (DeleteFlag)
                    BuildSuccess = true;
                else
                {
                    if (!Program.Arguments.Quiet)
                        Console.WriteLine("Skip: " + relativePath);

                    BuildSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine();
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        public override string ToString()
        {
            string relativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');
            List<string> result = new List<string> { "#begin " + relativePath };

            if (string.IsNullOrEmpty(Importer) || string.IsNullOrEmpty(Processor))
                result.Add("/copy:" + relativePath);
            else
            {
                result.Add("/importer:" + Importer);
                result.Add("/processor:" + Processor);

                foreach (KeyValuePair<string, string> item in ProcessorParam)
                    result.Add("/processorParam:" + item.Key + "=" + item.Value);

                result.Add("/build:" + relativePath);
            }

            result.Add(Environment.NewLine);

            return string.Join(Environment.NewLine, result);
        }
    }
}
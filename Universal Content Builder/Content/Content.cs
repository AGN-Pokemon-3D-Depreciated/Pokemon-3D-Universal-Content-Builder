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

        private string Platform;

        // Effect Importer - MonoGame
        private List<string> EffectImporter = new List<string>() { ".fx" };

        // Fbx Importer - MonoGame
        private List<string> FbxImporter = new List<string>() { ".fbx" };

        // Sprite Font Importer - MonoGame
        private List<string> FontDescriptionImporter = new List<string>() { ".spritefont" };

        // H.264 Video - MonoGame
        private List<string> H264Importer = new List<string>() { ".mp4" };

        // Mp3 Importer - MonoGame
        private List<string> Mp3Importer = new List<string>() { ".mp3" };

        // Ogg Importer - MonoGame
        private List<string> OggImporter = new List<string>() { ".ogg" };

        // Open Asset Import Library - MonoGame
        private List<string> OpenAssetImporter = new List<string>()
        {
            ".dae", // Collada
            ".gltf", "glb", // glTF
            ".blend", // Blender 3D
            ".3ds", // 3ds Max 3DS
            ".ase", // 3ds Max ASE
            ".obj", // Wavefront Object
            ".ifc", // Industry Foundation Classes (IFC/Step)
            ".xgl", ".zgl", // XGL
            ".ply", // Stanford Polygon Library
            ".dxf", // AutoCAD DXF
            ".lwo", // LightWave
            ".lws", // LightWave Scene
            ".lxo", // Modo
            ".stl", // Stereolithography
            ".ac", // AC3D
            ".ms3d", // Milkshape 3D
            ".cob", ".scn", // TrueSpace
            ".bvh", // Biovision BVH
            ".csm", // CharacterStudio Motion
            ".irrmesh", // Irrlicht Mesh
            ".irr", // Irrlicht Scene
            ".mdl", // Quake I, 3D GameStudio (3DGS)
            ".md2", // Quake II
            ".md3", // Quake III Mesh
            ".pk3", // Quake III Map/BSP
            ".mdc", // Return to Castle Wolfenstein
            ".md5", // Doom 3
            ".smd", ".vta", // Valve Model
            ".ogex", // Open Game Engine Exchange
            ".3d", // Unreal
            ".b3d", // BlitzBasic 3D
            ".q3d", ".q3s", // Quick3D
            ".nff", // Neutral File Format, Sense8 WorldToolKit
            ".off", // Object File Format
            ".ter", // Terragen Terrain
            ".hmp", // 3D GameStudio (3DGS) Terrain
            ".ndo", // Izware Nendo
        };

        // Texture Importer - MonoGame
        private List<string> TextureImporter = new List<string>()
        {
            ".bmp", // Bitmap Image File
            ".cut", // Dr Halo CUT
            ".dds", // Direct Draw Surface
            ".g3", // Raw Fax G3
            ".hdr", // RGBE
            ".gif", // Graphcis Interchange Format
            ".ico", // Microsoft Windows Icon
            ".iff", // Interchange File Format
            ".jbg", ".jbig", // JBIG
            ".jng", ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", // JPEG
            ".jp2", ".j2k", ".jpf", ".jpx", ".jpm", ".mj2", // JPEG 2000
            ".jxr", ".hdp", ".wdp", // JPEG XR
            ".koa", ".gg", // Koala
            ".pcd", // Kodak PhotoCD
            ".mng", // Multiple-Image Network Graphics
            ".pcx", //Personal Computer Exchange
            ".pbm", ".pgm", ".ppm", ".pnm", // Netpbm
            ".pfm", // Printer Font Metrics
            ".png", //Portable Network Graphics
            ".pict", ".pct", ".pic", // PICT
            ".psd", // Photoshop
            ".3fr", ".ari", ".arw", ".bay", ".crw", ".cr2", ".cap", ".dcs", // RAW
            ".dcr", ".dng", ".drf", ".eip", ".erf", ".fff", ".iiq", ".k25", // RAW
            ".kdc", ".mdc", ".mef", ".mos", ".mrw", ".nef", ".nrw", ".obm", // RAW
            ".orf", ".pef", ".ptx", ".pxn", ".r3d", ".raf", ".raw", ".rwl", // RAW
            ".rw2", ".rwz", ".sr2", ".srf", ".srw", ".x3f", // RAW
            ".ras", ".sun", // Sun RAS
            ".sgi", ".rgba", ".bw", ".int", ".inta", // Silicon Graphics Image
            ".tga", // Truevision TGA/TARGA
            ".tiff", ".tif", // Tagged Image File Format
            ".wbmp", // Wireless Application Protocol Bitmap Format
            ".webp", // WebP
            ".xbm", // X BitMap
            ".xpm", // X PixMap
        };

        // Wav Importer - MonoGame
        private List<string> WavImporter = new List<string>() { ".wav" };

        // Wma Importer - MonoGame
        private List<string> WmaImporter = new List<string>() { ".wma" };

        // Wmv Importer - MonoGame
        private List<string> WmvImporter = new List<string>() { ".wmv" };

        // X Importer - MonoGame
        private List<string> XImporter = new List<string>() { ".x" };

        // Xml Importer - MonoGame
        private List<string> XmlImporter = new List<string>() { ".xml" };

        public Content()
        {
        }

        public Content(string sourceFile, string platform)
        {
            SourceFile = sourceFile;
            Platform = platform;
            BuildTool = Program.Arguments.BuildTool.ToString();
        }

        private void CheckFileHash()
        {
            if (!File.Exists(SourceFile))
            {
                CleanFile();
                DeleteFlag = true;
            }
            else
            {
                string newHash;

                using (FileStream fileStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    newHash = fileStream.ToMD5();

                if (!StringHelper.Equals(MetaHash, newHash) || !StringHelper.Equals(Program.Arguments.BuildTool.ToString(), BuildTool))
                {
                    CleanFile();
                    BuildTool = Program.Arguments.BuildTool.ToString();
                    MetaHash = newHash;
                    RebuildFlag = true;
                }
                else
                {
                    List<string> fileToCheck = new List<string> { DestinationFile.GetFullPath() };
                    fileToCheck.AddRange(BuildOutput.Select(a => a.GetFullPath()));
                    fileToCheck.AddRange(BuildAsset.Select(a => a.GetFullPath()));

                    foreach (string file in fileToCheck)
                    {
                        if (!File.Exists(file))
                        {
                            CleanFile();
                            RebuildFlag = true;
                            break;
                        }
                    }
                }
            }
        }

        private void CleanFile()
        {
            List<string> fileToRemove = new List<string> { DestinationFile.GetFullPath() };
            fileToRemove.AddRange(BuildOutput.Select(a => a.GetFullPath()));
            fileToRemove.AddRange(BuildAsset.Select(a => a.GetFullPath()));

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

        private void CheckBuildConfig()
        {
            string relativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');

            string tempImporter = Importer;
            string tempProcessor = Processor;

            Dictionary<string, string> tempProcessorParam = new Dictionary<string, string>();
            tempProcessorParam.Concat(ProcessorParam);
            ProcessorParam.Clear();

#if MonoGame
            if (EffectImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // Effect Importer - MonoGame
                string temp;

                using (StreamReader reader = new StreamReader(new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.UTF8))
                    temp = reader.ReadToEnd();

                temp = temp.Replace("vs_1_1", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_4_0");
                temp = temp.Replace("vs_2_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_4_0");
                temp = temp.Replace("vs_4_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_4_0");
                temp = temp.Replace("ps_1_1", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_4_0");
                temp = temp.Replace("ps_2_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_4_0");
                temp = temp.Replace("ps_4_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_4_0");

                using (StreamWriter Writer = new StreamWriter(new FileStream(SourceFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8) { AutoFlush = true })
                    Writer.Write(temp);

                Importer = "EffectImporter";
                Processor = "EffectProcessor";
                ProcessorParam.Add("DebugMode", "Auto");
            }
            else if (FbxImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
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
            else if (XImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
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
            else if (OpenAssetImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
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
            else if (FontDescriptionImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // Sprite Font Importer - MonoGame
                Importer = "FontDescriptionImporter";
                Processor = "FontDescriptionProcessor";
                ProcessorParam.Add("TextureFormat", "Color");
            }
            else if (TextureImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // Texture Importer - MonoGame
                if (IsFontAssets(relativePath))
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
            else if (H264Importer.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // H.264 Video - MonoGame
                Importer = "H264Importer";
                Processor = "VideoProcessor";
            }
            else if (Mp3Importer.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // Mp3 Importer - MonoGame
                if (IsMusicAssets(relativePath))
                {
                    Importer = "Mp3Importer";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (IsSoundAssets(relativePath))
                {
                    Importer = "Mp3Importer";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }
            else if (OggImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // Ogg Importer - MonoGame
                if (IsMusicAssets(relativePath))
                {
                    Importer = "OggImporter";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (IsSoundAssets(relativePath))
                {
                    Importer = "OggImporter";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }
            else if (WavImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // Wav Importer - MonoGame
                if (IsMusicAssets(relativePath))
                {
                    Importer = "WavImporter";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (IsSoundAssets(relativePath))
                {
                    Importer = "WavImporter";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }
            else if (WmaImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // Wma Importer - MonoGame
                if (IsMusicAssets(relativePath))
                {
                    Importer = "WmaImporter";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (IsSoundAssets(relativePath))
                {
                    Importer = "WmaImporter";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }
            else if (WmvImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // Wmv Importer - MonoGame
                Importer = "WmvImporter";
                Processor = "VideoProcessor";
            }
            else if (XmlImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                // Xml Importer - MonoGame
                Importer = "XmlImporter";
                Processor = "PassThroughProcessor";
            }
#endif

            if (tempImporter != Importer || tempProcessor != Processor || tempProcessorParam.Count() != ProcessorParam.Count())
            {
                CleanFile();
                BuildTool = Program.Arguments.BuildTool.ToString();
                RebuildFlag = true;
            }
            else
            {
                foreach (KeyValuePair<string, string> item in tempProcessorParam)
                {
                    if (!ProcessorParam.ContainsKey(item.Key) || ProcessorParam[item.Key] != item.Value)
                    {
                        CleanFile();
                        BuildTool = Program.Arguments.BuildTool.ToString();
                        RebuildFlag = true;
                        break;
                    }
                }
            }
        }

        private bool IsMusicAssets(string value)
        {
            if (value.ToLower().StartsWith("songs/".NormalizeFilePath()) ||
                value.ToLower().Contains("content/songs/".NormalizeFilePath()) ||
                value.ToLower().Contains("content/music/".NormalizeFilePath()) ||
                value.ToLower().Contains("SharedResources/songs/".NormalizeFilePath()) ||
                value.ToLower().Contains("SharedResources/music/".NormalizeFilePath()))
                return true;
            else
                return false;
        }

        private bool IsSoundAssets(string value)
        {
            if (value.ToLower().StartsWith("sounds/".NormalizeFilePath()) ||
                value.ToLower().Contains("content/sounds/".NormalizeFilePath()) ||
                value.ToLower().Contains("content/sound effects/".NormalizeFilePath()) ||
                value.ToLower().Contains("sharedresources/sounds/".NormalizeFilePath()) ||
                value.ToLower().Contains("sharedresources/sound effects/".NormalizeFilePath()))
                return true;
            else
                return false;
        }

        private bool IsFontAssets(string value)
        {
            if (value.ToLower().StartsWith("fonts/".NormalizeFilePath()) ||
                value.ToLower().Contains("content/fonts/".NormalizeFilePath()) ||
                value.ToLower().Contains("sharedresources/fonts/".NormalizeFilePath()))
                return true;
            else
                return false;
        }

        public void BuildContent()
        {
            try
            {
                string relativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');
                string relativePathWithoutExtension = relativePath.Contains(".") ? relativePath.Remove(relativePath.LastIndexOf('.')) : relativePath;
                string relativeDirWithoutExtension = relativePath.Contains("/") || relativePath.Contains("\\") ? relativePath.Remove(relativePath.LastIndexOfAny(new char[] { '/', '\\' })) : "";

                CheckBuildConfig();

                if (string.IsNullOrEmpty(Importer) || string.IsNullOrEmpty(Processor))
                    DestinationFile = (Program.Arguments.OutputDirectory + "/" + relativePath).GetFullPath();
                else
                    DestinationFile = (Program.Arguments.OutputDirectory + "/" + relativePathWithoutExtension + ".xnb").GetFullPath();

                CheckFileHash();

                if (RebuildFlag || Program.Arguments.Rebuild)
                {
                    if (string.IsNullOrEmpty(Importer) || string.IsNullOrEmpty(Processor))
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
                        InternalBuildContent();
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

        private void InternalBuildContent()
        {
            string relativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');

#if MonoGame
            PipelineManager manager = new PipelineManager(Program.Arguments.WorkingDirectory, Program.Arguments.OutputDirectory, Program.Arguments.IntermediateDirectory)
            {
                CompressContent = Program.Arguments.Compress
            };

            if (StringHelper.Equals(Program.Arguments.Platform, "DesktopGL"))
                manager.Platform = TargetPlatform.DesktopGL;

            if (StringHelper.Equals(Program.Arguments.Profile, "Reach"))
                manager.Profile = GraphicsProfile.Reach;
            else if (StringHelper.Equals(Program.Arguments.Profile, "HiDef"))
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
                    Console.WriteLine("Content Error (" + relativePath + "): " + ex.Message);

                BuildSuccess = false;
            }
#endif
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
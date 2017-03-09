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
        public string SourceFile { get; private set; }
        public string DestinationFile { get; private set; }
        public string Importer { get; private set; }
        public string Processor { get; private set; }
        public Dictionary<string, string> ProcessorParam { get; private set; } = new Dictionary<string, string>();

        public string OutputHash { get; private set; }

        public List<string> BuildOutput { get; private set; } = new List<string>();
        public List<string> BuildAsset { get; private set; } = new List<string>();

        [YamlIgnore]
        public bool DeleteFlag { get; private set; } = false;

        [YamlIgnore]
        public bool BuildSuccess { get; private set; } = false;

        private string Platform;
        private bool RebuildFlag = false;

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
                List<string> fileToCheck = new List<string>();
                fileToCheck.Add(DestinationFile.GetFullPath());
                fileToCheck.AddRange(BuildOutput.Select(a => a.GetFullPath()));
                fileToCheck.AddRange(BuildAsset.Select(a => a.GetFullPath()));

                string newHash;

                using (FileStream fileStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    newHash = fileStream.ToMD5();

                if (!StringHelper.Equals(OutputHash, newHash))
                {
                    CleanFile();
                    OutputHash = newHash;
                    RebuildFlag = true;
                }
                else
                {
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
            List<string> fileToRemove = new List<string>();
            fileToRemove.Add(DestinationFile.GetFullPath());
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

#if MonoGame
            // Effect Importer - MonoGame
            if (EffectImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                string temp;

                using (StreamReader reader = new StreamReader(new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.UTF8))
                    temp = reader.ReadToEnd();

                temp = temp.Replace("vs_1_1", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_4_0");
                temp = temp.Replace("vs_2_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_4_0");
                temp = temp.Replace("vs_4_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_4_0");
                temp = temp.Replace("ps_1_1", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_4_0");
                temp = temp.Replace("ps_2_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_4_0");
                temp = temp.Replace("ps_4_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_4_0");

                using (FileStream FileStream = new FileStream(SourceFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter Writer = new StreamWriter(FileStream, Encoding.UTF8) { AutoFlush = true })
                        Writer.Write(temp);
                }

                Importer = "EffectImporter";
                Processor = "EffectProcessor";
                ProcessorParam.Add("DebugMode", "Auto");
            }

            // Fbx Importer - MonoGame
            if (FbxImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
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

            // Sprite Font Importer - MonoGame
            if (FontDescriptionImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                Importer = "FontDescriptionImporter";
                Processor = "FontDescriptionProcessor";
                ProcessorParam.Add("TextureFormat", "Color");
            }

            // H.264 Video - MonoGame
            if (H264Importer.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                Importer = "H264Importer";
                Processor = "VideoProcessor";
            }

            // Mp3 Importer - MonoGame
            if (Mp3Importer.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                if (relativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || relativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "Mp3Importer";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (relativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || relativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "Mp3Importer";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }

            // Ogg Importer - MonoGame
            if (OggImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                if (relativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || relativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "OggImporter";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (relativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || relativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "OggImporter";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }

            // Open Asset Import Library - MonoGame
            if (OpenAssetImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
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

            // Texture Importer - MonoGame
            if (TextureImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                if (relativePath.ToLower().StartsWith("fonts/".NormalizeFilePath()) || relativePath.ToLower().StartsWith("content/fonts/".NormalizeFilePath()))
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

            // Wav Importer - MonoGame
            if (WavImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                if (relativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || relativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "WavImporter";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (relativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || relativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "WavImporter";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }

            // Wma Importer - MonoGame
            if (WmaImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                if (relativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || relativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "WmaImporter";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (relativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || relativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "WmaImporter";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }

            // Wmv Importer - MonoGame
            if (WmvImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                Importer = "WmvImporter";
                Processor = "VideoProcessor";
            }

            // X Importer - MonoGame
            if (XImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
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

            // Xml Importer - MonoGame
            if (XmlImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                Importer = "XmlImporter";
                Processor = "PassThroughProcessor";
            }
#endif
        }

        public void BuildContent()
        {
            try
            {
                string relativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');
                string relativePathWithoutExtension = relativePath.Remove(relativePath.LastIndexOf('.'));
                string relativeDirWithoutExtension = relativePath.Contains("/") || relativePath.Contains("\\") ? relativePath.Remove(relativePath.LastIndexOfAny(new char[] { '/', '\\' })) : "";

                CheckFileHash();

                if (RebuildFlag || Program.Arguments.Rebuild)
                {
                    CheckBuildConfig();

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
            PipelineManager manager = new PipelineManager(Program.Arguments.WorkingDirectory, Program.Arguments.OutputDirectory, Program.Arguments.IntermediateDirectory);
            manager.CompressContent = Program.Arguments.Compress;

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
            List<string> result = new List<string>();

            result.Add("#begin " + relativePath);

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
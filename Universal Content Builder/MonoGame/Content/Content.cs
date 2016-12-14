#if MonoGame
extern alias MonoGame;
using MonoGame::Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame::Microsoft.Xna.Framework.Graphics;
using MonoGame::MonoGame.Framework.Content.Pipeline.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Universal_Content_Builder.Content.Interface;
using Universal_Content_Builder.Core;
using Universal_Content_Builder.Modules.System;
using Universal_Content_Builder.Modules.System.IO;

namespace Universal_Content_Builder.MonoGame.Content
{
    public class Content : IContent
    {
        private string SourceFile;
        private string Importer;
        private string Processor;
        private Dictionary<string, string> ProcessorParam = new Dictionary<string, string>();

        public Content(string file)
        {
            SourceFile = file.GetFullPath();
            string RelativePath = SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');

            // Effect Importer - MonoGame
            if (SourceFile.ToLower().EndsWith(".fx"))
            {
                string Temp;

                using (FileStream FileStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader Reader = new StreamReader(FileStream, Encoding.UTF8))
                        Temp = Reader.ReadToEnd();

                    Temp = Temp.Replace("vs_1_1", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_4_0");
                    Temp = Temp.Replace("vs_2_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_4_0");
                    Temp = Temp.Replace("vs_4_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "vs_2_0" : "vs_4_0");
                    Temp = Temp.Replace("ps_1_1", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_4_0");
                    Temp = Temp.Replace("ps_2_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_4_0");
                    Temp = Temp.Replace("ps_4_0", StringHelper.Equals(Program.Arguments.Platform, "DesktopGL") ? "ps_2_0" : "ps_4_0");
                }

                using (FileStream FileStream = new FileStream(SourceFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter Writer = new StreamWriter(FileStream, Encoding.UTF8) { AutoFlush = true })
                        Writer.Write(Temp);
                }

                Importer = "EffectImporter";
                Processor = "EffectProcessor";
                ProcessorParam.Add("DebugMode", "Auto");
            }

            // Fbx Importer - MonoGame
            if (SourceFile.ToLower().EndsWith(".fbx"))
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
            if (SourceFile.ToLower().EndsWith(".spritefont"))
            {
                Importer = "FontDescriptionImporter";
                Processor = "FontDescriptionProcessor";
                ProcessorParam.Add("TextureFormat", "Color");
            }

            // H.264 Video - MonoGame
            // Not implemented.

            // Mp3 Importer - MonoGame
            if (SourceFile.ToLower().EndsWith(".mp3"))
            {
                if (RelativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "Mp3Importer";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (RelativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "Mp3Importer";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }

            // Ogg Importer - MonoGame
            if (SourceFile.ToLower().EndsWith(".ogg"))
            {
                if (RelativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "OggImporter";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (RelativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "OggImporter";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }

            // Open Asset Import Library - MonoGame
            // Not implemented.

            // Texture Importer - MonoGame
            if (SourceFile.ToLower().EndsWith(".png") || SourceFile.ToLower().EndsWith(".jpg") || SourceFile.ToLower().EndsWith(".bmp") || SourceFile.ToLower().EndsWith(".tga"))
            {
                if (RelativePath.ToLower().StartsWith("fonts/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/fonts/".NormalizeFilePath()))
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
            if (SourceFile.ToLower().EndsWith(".wav"))
            {
                if (RelativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "WavImporter";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (RelativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "WavImporter";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }

            // Wma Importer - MonoGame
            if (SourceFile.ToLower().EndsWith(".wma"))
            {
                if (RelativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/songs/".NormalizeFilePath()))
                {
                    Importer = "WmaImporter";
                    Processor = "SongProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
                else if (RelativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) || RelativePath.ToLower().StartsWith("content/sounds/".NormalizeFilePath()))
                {
                    Importer = "WmaImporter";
                    Processor = "SoundEffectProcessor";
                    ProcessorParam.Add("Quality", "Low");
                }
            }

            // Wmv Importer - MonoGame
            // Not implemented.

            // X Importer - MonoGame
            if (SourceFile.ToLower().EndsWith(".x"))
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

            PipelineManager Manager = new PipelineManager(Program.Arguments.WorkingDirectory, Program.Arguments.OutputDirectory, Program.Arguments.IntermediateDirectory);
            Manager.CompressContent = Program.Arguments.Compress;

            if (StringHelper.Equals(Program.Arguments.Platform, "DesktopGL"))
                Manager.Platform = TargetPlatform.DesktopGL;

            if (StringHelper.Equals(Program.Arguments.Profile, "Reach"))
                Manager.Profile = GraphicsProfile.Reach;
            else if (StringHelper.Equals(Program.Arguments.Profile, "HiDef"))
                Manager.Profile = GraphicsProfile.HiDef;

            OpaqueDataDictionary Param = new OpaqueDataDictionary();

            foreach (KeyValuePair<string, string> item in ProcessorParam)
                Param.Add(item.Key, item.Value);

            try
            {
                if (!Program.Arguments.Quiet)
                    Console.WriteLine("Building: " + RelativePath);

                Manager.BuildContent(SourceFile, null, Importer, Processor, Param);
                return true;
            }
            catch (Exception ex)
            {
                if (!Program.Arguments.Quiet)
                    Console.WriteLine("Content Error (" + RelativePath + "): " + ex.Message);

                return false;
            }
        }
    }
}
#endif
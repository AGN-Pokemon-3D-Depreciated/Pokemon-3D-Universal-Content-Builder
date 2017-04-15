using Modules.System.IO;
using System.Linq;
using Universal_Content_Builder.Core;

namespace Universal_Content_Builder.Content
{
    public class Assets
    {
        // Effect Importer - MonoGame
        private string[] EffectImporter = new string[] { ".fx" };

        // Fbx Importer - MonoGame
        private string[] FbxImporter = new string[] { ".fbx" };

        // X Importer - MonoGame
        private string[] XImporter = new string[] { ".x" };

        // Open Asset Import Library - MonoGame
        private string[] OpenAssetImporter = new string[]
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

        // Sprite Font Importer - MonoGame
        private string[] FontDescriptionImporter = new string[] { ".spritefont" };

        // Texture Importer - MonoGame
        private string[] TextureImporter = new string[]
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

        // Mp3 Importer - MonoGame
        private string[] Mp3Importer = new string[] { ".mp3" };

        // Ogg Importer - MonoGame
        private string[] OggImporter = new string[] { ".ogg" };

        // Wav Importer - MonoGame
        private string[] WavImporter = new string[] { ".wav" };

        // Wma Importer - MonoGame
        private string[] WmaImporter = new string[] { ".wma" };

        // Wmv Importer - MonoGame
        private string[] WmvImporter = new string[] { ".wmv" };

        // H.264 Video - MonoGame
        private string[] H264Importer = new string[] { ".mp4" };

        // Xml Importer - MonoGame
        private string[] XmlImporter = new string[] { ".xml" };

        private string SourceFile;
        private string RelativePath { get { return SourceFile.Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\'); } }

        public Assets(string sourceFile)
        {
            SourceFile = sourceFile;
        }

        public bool IsEffectAssets()
        {
            if (EffectImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsFbxModelAssets()
        {
            if (FbxImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsXModelAssets()
        {
            if (XImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsOpenModelAssets()
        {
            if (OpenAssetImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsModelAssets()
        {
            if (IsFbxModelAssets() || IsXModelAssets() || IsOpenModelAssets())
            {
                if (RelativePath.ToLower().StartsWith("models/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("content/models/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("sharedresources/models/".NormalizeFilePath()))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsSpriteFontAssets()
        {
            if (FontDescriptionImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                if (RelativePath.ToLower().StartsWith("fonts/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("content/fonts/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("sharedresources/fonts/".NormalizeFilePath()))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsFontAssets()
        {
            if (TextureImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
            {
                if (RelativePath.ToLower().StartsWith("fonts/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("content/fonts/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("sharedresources/fonts/".NormalizeFilePath()))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsTextureAssets()
        {
            if (TextureImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsMp3Assets()
        {
            if (Mp3Importer.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsOggAssets()
        {
            if (OggImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsWavAssets()
        {
            if (WavImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsWmaAssets()
        {
            if (WmaImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsMusicAssets()
        {
            if (IsMp3Assets() || IsOggAssets() || IsWavAssets() || IsWavAssets())
            {
                if (RelativePath.ToLower().StartsWith("songs/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("content/songs/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("sharedresources/songs/".NormalizeFilePath()) ||
                    RelativePath.ToLower().StartsWith("music/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("content/music/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("sharedresources/music/".NormalizeFilePath()))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsSoundAssets()
        {
            if (IsMp3Assets() || IsOggAssets() || IsWavAssets() || IsWavAssets())
            {
                if (RelativePath.ToLower().StartsWith("sounds/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("content/sounds/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("sharedresources/sounds/".NormalizeFilePath()) ||
                    RelativePath.ToLower().StartsWith("sound effects/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("content/sound effects/".NormalizeFilePath()) ||
                    RelativePath.ToLower().Contains("sharedresources/sound effects/".NormalizeFilePath()))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsWmvAssets()
        {
            if (WmvImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsMp4Assets()
        {
            if (H264Importer.Any(a => SourceFile.ToLower().EndsWith(a)))
                return true;
            else
                return false;
        }

        public bool IsXMLAssets()
        {
            if (XmlImporter.Any(a => SourceFile.ToLower().EndsWith(a)))
                return false;
            else
                return false;
        }
    }
}

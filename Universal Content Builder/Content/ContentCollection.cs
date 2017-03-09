using Amib.Threading;
using Modules.System.IO;
using Modules.YamlDotNet.Serialization;
using System.Collections.Generic;
using System.IO;
using Universal_Content_Builder.Core;
using System.Linq;

namespace Universal_Content_Builder.Content
{
    public class ContentCollection
    {
        public List<Content> ContentFiles { get; private set; } = new List<Content>();

        private IWorkItemsGroup ThreadPool;

        // Fbx Importer - MonoGame
        private List<string> FbxImporter = new List<string>() { ".fbx" };

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

        // X Importer - MonoGame
        private List<string> XImporter = new List<string>() { ".x" };

        public ContentCollection()
        {
            ThreadPool = new SmartThreadPool().CreateWorkItemsGroup(Program.Arguments.NumThread, new WIGStartInfo() { StartSuspended = true });
        }

        private void LoadOldContentFiles()
        {
            string contentPath = $"{Program.Arguments.IntermediateDirectory}/Content.yml".GetFullPath();

            if (File.Exists(contentPath))
                Program.ContentCollection = contentPath.Deserialize<ContentCollection>() ?? new ContentCollection();
        }

        private void LoadNewContentFiles()
        {
            foreach (string file in Directory.GetFiles(Program.Arguments.WorkingDirectory, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = file.GetFullPath().Replace(Program.Arguments.WorkingDirectory, "").Trim('/', '\\');

                // Ignore folder.
                if (Directory.Exists(file))
                    continue;

                // Global Ignore.
                if (relativePath.ToLower().StartsWith("bin") || relativePath.ToLower().StartsWith("obj"))
                    continue;

                // Git Ignore.
                if (file.ToLower().EndsWith(".gitignore") ||
                    file.ToLower().EndsWith(".git") ||
                    file.ToLower().EndsWith(".cab") ||
                    file.ToLower().EndsWith(".msi") ||
                    file.ToLower().EndsWith(".msm") ||
                    file.ToLower().EndsWith(".msp") ||
                    file.ToLower().EndsWith(".lnk") ||
                    file.ToLower().EndsWith(".directory") ||
                    file.ToLower().EndsWith(".ds_store") ||
                    file.ToLower().EndsWith(".appledouble") ||
                    file.ToLower().EndsWith(".lsoverride") ||
                    file.ToLower().EndsWith(".documentrevisions-v100") ||
                    file.ToLower().EndsWith(".fseventsd") ||
                    file.ToLower().EndsWith(".spotlight-v100") ||
                    file.ToLower().EndsWith(".temporaryitems") ||
                    file.ToLower().EndsWith(".trashes") ||
                    file.ToLower().EndsWith(".volumeicon.icns") ||
                    file.ToLower().EndsWith(".com.apple.timemachine.donotpresent") ||
                    file.ToLower().EndsWith("thumbs.db") ||
                    file.ToLower().EndsWith("ehthumbs.db") ||
                    file.ToLower().EndsWith("desktop.ini") ||
                    file.ToLower().EndsWith(".appledb") ||
                    file.ToLower().EndsWith(".appledesktop") ||
                    file.ToLower().EndsWith(".apdisk") ||
                    file.ToUpper().EndsWith("LICENSE") ||
                    file.ToUpper().EndsWith("README") ||
                    file.ToLower().EndsWith(".mgcb") ||
                    file.ToLower().EndsWith(".ignore") ||
                    file.ToLower().EndsWith(".lock"))
                    continue;

                // Model texture Ignore.
                if (relativePath.ToLower().StartsWith("content/models/".NormalizeFilePath()) ||
                    relativePath.ToLower().StartsWith("models/".NormalizeFilePath()))
                {
                    if (!FbxImporter.Union(OpenAssetImporter).Union(XImporter).Any(a => file.ToLower().EndsWith(a)))
                        continue;
                }

                if (ContentFiles.Where(a => string.Equals(a.SourceFile, file, System.StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    ContentFiles.Add(new Content(file, Program.Arguments.Platform));
            }
        }

        public void BuildAllContent()
        {
            LoadOldContentFiles();
            LoadNewContentFiles();

            foreach (Content content in ContentFiles)
                ThreadPool.QueueWorkItem(() => content.BuildContent());

            ThreadPool.Start();
            ThreadPool.WaitForIdle();

            ContentFiles = ContentFiles.Where(a => !a.DeleteFlag).ToList();
            this.Serialize($"{Program.Arguments.IntermediateDirectory}/Content.yml".GetFullPath());

#if MonoGame
            // Remove MonoGame Content (obj).
            foreach (string file in Directory.GetFiles(Program.Arguments.IntermediateDirectory, "*.mgcontent", SearchOption.AllDirectories))
                File.Delete(file);
#endif
        }
    }
}

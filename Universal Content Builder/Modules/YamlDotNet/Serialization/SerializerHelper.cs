using System;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace Universal_Content_Builder.Modules.YamlDotNet.Serialization
{
    public static class SerializerHelper
    {
        /// <summary>
        /// Serialize Yaml Model.
        /// </summary>
        /// <param name="Object">Object to serialize.</param>
        /// <param name="File">File to save.</param>
        public static bool Serialize(this object Object, string File)
        {
            try
            {
                using (FileStream FileStream = new FileStream(File, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter Writer = new StreamWriter(FileStream, Encoding.UTF8))
                    {
                        Serializer Serializer = new Serializer();
                        Serializer.Serialize(Writer, Object);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Serialize Yaml Model.
        /// </summary>
        /// <param name="Object">Object to serialize.</param>
        /// <param name="File">File to save.</param>
        /// <param name="ex">Ex exception.</param>
        public static bool Serialize(this object Object, string File, out Exception ex)
        {
            try
            {
                using (FileStream FileStream = new FileStream(File, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter Writer = new StreamWriter(FileStream, Encoding.UTF8))
                    {
                        Serializer Serializer = new Serializer();
                        Serializer.Serialize(Writer, Object);
                        ex = null;
                        return true;
                    }
                }
            }
            catch (Exception ex2)
            {
                ex = ex2;
                return false;
            }
        }
    }
}
using System;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace Universal_Content_Builder.Modules.YamlDotNet.Serialization
{
    public static class DeserializerHelper
    {
        /// <summary>
        /// Deserialize Yaml Model.
        /// </summary>
        /// <typeparam name="T">Yaml Model Class.</typeparam>
        /// <param name="File">File to load.</param>
        public static T Deserialize<T>(this string File) where T : class
        {
            try
            {
                using (FileStream FileStream = new FileStream(File, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader Reader = new StreamReader(FileStream, Encoding.UTF8))
                    {
                        Deserializer Deserializer = new Deserializer();
                        return Deserializer.Deserialize<T>(Reader);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Deserialize Yaml Model.
        /// </summary>
        /// <typeparam name="T">Yaml Model Class.</typeparam>
        /// <param name="File">File to load.</param>
        /// <param name="ex">Ex exception.</param>
        public static T Deserialize<T>(this string File, out Exception ex) where T : class
        {
            try
            {
                using (FileStream FileStream = new FileStream(File, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader Reader = new StreamReader(FileStream, Encoding.UTF8))
                    {
                        Deserializer Deserializer = new Deserializer();
                        ex = null;
                        return Deserializer.Deserialize<T>(Reader);
                    }
                }
            }
            catch (Exception ex2)
            {
                ex = ex2;
                return null;
            }
        }
    }
}
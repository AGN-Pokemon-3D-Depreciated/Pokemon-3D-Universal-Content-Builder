using System;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace Modules.YamlDotNet.Serialization
{
    public static class DeserializerHelper
    {
        public static T Deserialize<T>(this string file) where T : class
        {
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.UTF8))
                {
                    Deserializer deserializer = new Deserializer();
                    return deserializer.Deserialize<T>(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static T Deserialize<T>(this string file, out Exception ex) where T : class
        {
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.UTF8))
                {
                    Deserializer deserializer = new Deserializer();
                    ex = null;
                    return deserializer.Deserialize<T>(reader);
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
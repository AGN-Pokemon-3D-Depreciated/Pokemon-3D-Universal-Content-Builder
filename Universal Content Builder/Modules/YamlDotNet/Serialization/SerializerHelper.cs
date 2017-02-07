using System;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace Modules.YamlDotNet.Serialization
{
    public static class SerializerHelper
    {
        public static bool Serialize(this object obj, string file)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8))
                {
                    SerializerBuilder serializer = new SerializerBuilder();
                    serializer.EmitDefaults().Build().Serialize(writer, obj);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Serialize(this object obj, string file, out Exception ex)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8))
                {
                    SerializerBuilder serializer = new SerializerBuilder();
                    serializer.EmitDefaults().Build().Serialize(writer, obj);
                    ex = null;
                    return true;
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
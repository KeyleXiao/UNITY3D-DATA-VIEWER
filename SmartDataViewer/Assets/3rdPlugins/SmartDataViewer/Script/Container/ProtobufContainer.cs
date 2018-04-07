using System;
using System.IO;
using System.Runtime.Remoting;
using ProtoBuf;
using ProtoBuf.Meta;
using SmartDataViewer.Helpers;

namespace SmartDataViewer
{
    public class ProtobufContainer : ConfigContainerBase, IConfigContainer
    {
        /// <summary>
        /// 加载配置(静态)
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public V LoadConfig<V>(string path) where V : new()
        {
            path = PathMapping.GetInstance().DecodePath(path);
            byte[] byteArray =File.ReadAllBytes(path);

            if (byteArray.Length == 0)  return new V();

            using (var ms = new MemoryStream(byteArray))
            {
                return Serializer.Deserialize<V>(ms);
            }
        }


        public object LoadConfig(Type t, string path)
        {
            path = PathMapping.GetInstance().DecodePath(path);
            
            byte[] byteArray =File.ReadAllBytes(path);
            
            if (byteArray.Length == 0)  return null;

            using (var ms = new MemoryStream(byteArray))
            {
                return RuntimeTypeModel.Default.Deserialize(ms, null, t);
            }
        }


        /// <summary>
        /// 保存配置到本地
        /// </summary>
        /// <param name="path"></param>
        public bool SaveToDisk(string path, object target)
        {
            path = PathMapping.GetInstance().DecodePath(path);
            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, target);   
                bytes = new byte[ms.Position];  
                var fullBytes = ms.GetBuffer();  
                Array.Copy(fullBytes, bytes, bytes.Length);   
            }

            DeleteFromDisk(path);
            
            File.WriteAllBytes(path , bytes);

            return true;
        }
    }
}
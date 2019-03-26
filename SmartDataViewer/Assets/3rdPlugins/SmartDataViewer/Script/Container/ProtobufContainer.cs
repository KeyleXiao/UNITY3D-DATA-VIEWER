using System;
using System.IO;
using System.Runtime.Remoting;
using ProtoBuf;
using ProtoBuf.Meta;
using SmartDataViewer.Helpers;
using UnityEngine;

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
        public V LoadConfig<V>(string path) 
        {
            path = PathMapping.GetInstance().DecodePath(path);
            byte[] byteArray =File.ReadAllBytes(path);

            if (byteArray.Length == 0)  return default(V);

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
				return Serializer.Deserialize(t, ms);
			}
		}

		/// <summary>
		/// 保存配置到本地
		/// </summary>
		/// <param name="path"></param>
		public bool SaveToDisk(string path, object target)
		{
			path = PathMapping.GetInstance().DecodePath(path);
			DeleteFromDisk(path);	
			
			MemoryStream memoryStream = new MemoryStream();
			Serializer.Serialize(memoryStream, target);
			var array = new byte[memoryStream.Length];
			memoryStream.Position = 0L;
			memoryStream.Read(array, 0, array.Length);
			File.WriteAllBytes(path, array);
			return true;
		}
	}
}
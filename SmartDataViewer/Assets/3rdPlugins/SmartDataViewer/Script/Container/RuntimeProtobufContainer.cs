using System;
using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;


namespace SmartDataViewer
{
	/// <summary>
	/// 运行时只支持数据读取
	/// </summary>
	public class RuntimeProtobufContainer : ConfigContainerBase,IConfigContainer
	{
		public V LoadConfig<V>(string content)
		{
			byte[] byteArray =File.ReadAllBytes(content);

			if (byteArray.Length == 0)  return default(V);

			using (var ms = new MemoryStream(byteArray))
			{
				return Serializer.Deserialize<V>(ms);
			}
		}
		
		public object LoadConfig(Type t, string content)
		{
			byte[] byteArray = File.ReadAllBytes(content);
            
			if (byteArray.Length == 0)  return null;

			using (var ms = new MemoryStream(byteArray))
			{
				return RuntimeTypeModel.Default.Deserialize(ms, null, t);
			}
		}


		public virtual bool SaveToDisk(string path, object target)
		{
			return false;
		}

		public override bool DeleteFromDisk(string path)
		{
			return false;
		}

	}
}
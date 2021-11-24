using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SmartDataViewer
{
	public class RuntimeUnityJsonContainer : ConfigContainerBase, IConfigContainer
	{  
		public async Task<T> LoadConfigAsync<T>(string path) 
		{
			var loadTextHandle =  Addressables.LoadAssetAsync<TextAsset>(path).Task;
			TextAsset textAsset = await loadTextHandle;

			if (!textAsset)
				return default(T);
			
			return JsonUtility.FromJson<T>(textAsset.text);
		}
		


		public object LoadConfig(Type t, string path)
		{
			throw new NotImplementedException();
		}

		public T LoadConfig<T>(string path)
		{
			throw new NotImplementedException();
		}

		public async Task<object> LoadConfigAsync(Type t, string path)
		{
			var loadTextHandle =  Addressables.LoadAssetAsync<TextAsset>(path).Task;
			TextAsset textAsset = await loadTextHandle;

			if (!textAsset)
				return null;

			return JsonUtility.FromJson(textAsset.text, t);
		}


		/// <summary>
		/// 保存配置到本地
		/// </summary>
		/// <param name="path"></param>
		public bool SaveToDisk(string path, object target)
		{
			//Modified by keyle 2016.11.29 缩减配置尺寸
			string content = JsonUtility.ToJson(target, false);
			return  SaveText(path, content);
		}
	}
}
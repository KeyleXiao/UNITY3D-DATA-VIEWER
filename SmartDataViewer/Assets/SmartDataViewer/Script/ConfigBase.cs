using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SmartDataViewer
{
	[Serializable]
	public class ConfigBase<T> where T : IModel
	{

		public ConfigBase()
		{
			Configs = new Dictionary<int, T>();
			ConfigList = new List<T>();
		}

		public Dictionary<int, T> Configs { get; set; }

		public List<T> ConfigList;

		public virtual void InitConfigsFromChache()
		{
			if (Configs == null)
				Configs = new Dictionary<int, T>();

			Configs.Clear();

			if (ConfigList == null)
			{
#if UNITY_EDITOR
				UnityEngine.Debug.Log(string.Format("{0} Can't loading", typeof(T).Name));
#endif
				return;
			}

			foreach (var item in ConfigList)
			{
				if (Configs.ContainsKey(item.ID))
				{
					//Debug.LogError(typeof(T).Name + " Same ID：" + item.ID);
					continue;
				}
				Configs.Add(item.ID, item);
			}
		}

		public virtual T SearchByID(int id)
		{
			if (Configs.ContainsKey(id))
			{
				return Configs[id];
			}

			for (int i = 0; i < ConfigList.Count; i++)
			{
				if (ConfigList[i].ID == id)
				{
					return ConfigList[i];
				}
			}
			return null;
		}

		public virtual T SearchByNickName(string nickname)
		{
			for (int i = 0; i < ConfigList.Count; i++)
			{
				if (ConfigList[i].NickName == nickname)
				{
					return ConfigList[i];
				}
			}

			return null;
		}

		public void UpdateConfig(T item)
		{
			if (Configs.ContainsKey(item.ID))
			{
				Configs[item.ID] = item;

				for (int i = 0; i < ConfigList.Count; i++)
				{
					if (ConfigList[i].ID == item.ID)
					{
						ConfigList[i] = item;
						break;
					}
				}
			}
			else
			{
				Configs.Add(item.ID, item);
				ConfigList.Add(item);
			}
		}

		public virtual void Delete(int id)
		{
			int index = 0;
			for (int i = 0; i < ConfigList.Count; i++)
			{
				if (ConfigList[i].ID == id)
				{
					index = i;
					break;
				}
			}
			ConfigList.RemoveAt(index);
			Configs.Remove(id);
		}

		string PathCombine(string pathA, string pathB)
		{
			return Path.Combine(pathA, pathB).Replace("\\", "/");
		}



		public virtual void DeleteFromDisk(string fileWithNoExcension = "", bool absolute = false)
		{
			absolute = GetAbsolutePath(ref fileWithNoExcension);

			if (!absolute)
			{
				string configPath = string.Format("{0}/Resources/Config/{1}.txt", Application.dataPath, fileWithNoExcension);
				if (File.Exists(configPath))
					File.Delete(configPath);
			}
			else
			{
				if (File.Exists(fileWithNoExcension))
					File.Delete(fileWithNoExcension);
			}



		}

		public static bool GetAbsolutePath(ref string fileWithNoExcension)
		{
			if (fileWithNoExcension.Contains(RootSymbol))
			{
				fileWithNoExcension = fileWithNoExcension.Replace(RootSymbol, Application.dataPath);
				if (!fileWithNoExcension.EndsWith(".txt")) fileWithNoExcension += ".txt";
			}
			else if (fileWithNoExcension.Contains(EditorSymbol))
			{
				fileWithNoExcension = fileWithNoExcension.Replace(EditorSymbol, Application.dataPath + "SmartDataViewer/");
				if (!fileWithNoExcension.EndsWith(".txt")) fileWithNoExcension += ".txt";
			}
			else
			{
				if (string.IsNullOrEmpty(fileWithNoExcension))
				{
					fileWithNoExcension = typeof(T).Name;
				}
				return false;
			}
			return true;
		}

		public virtual void SaveToDisk(string fileWithNoExcension = "", bool absolute = false)
		{
			absolute = GetAbsolutePath(ref fileWithNoExcension);

			//Modified by keyle 2016.11.29 缩减配置尺寸
			string cg = JsonUtility.ToJson(this, false);
			string p = string.Empty;


			DeleteFromDisk(fileWithNoExcension, absolute);

			if (!absolute)
			{
				p = string.Format(@"{0}/Resources/Config/{1}.txt", Application.dataPath, fileWithNoExcension);
				File.WriteAllText(p, cg);
			}
			else
			{
				File.WriteAllText(fileWithNoExcension, cg);
			}
		}

		static string RootSymbol = "{ROOT}";
		static string EditorSymbol = "{EDITOR}";

		public static V LoadConfig<V>(string fileWithNoExcension = "", bool absolute = false) where V : ConfigBase<T>, new()
		{
			absolute = GetAbsolutePath(ref fileWithNoExcension);
			//Check Is Null

			string str = string.Empty;
			if (!absolute)
			{
				TextAsset temp = Resources.Load<TextAsset>(string.Format("Config/{0}", fileWithNoExcension));

				if (temp == null)
				{
#if UNITY_EDITOR
					Debug.Log(string.Format("Can't Find {0} , FileName {1}", typeof(V).Name, fileWithNoExcension));
#endif
					return new V();
				}
				str = temp.text;
			}
			else
			{
				string path = fileWithNoExcension;
				if (!File.Exists(path))
					return new V();
				else
					str = File.ReadAllText(path);
			}

			return JsonUtility.FromJson<V>(str);
		}
	}
}
//
//   		Copyright 2017 KeyleXiao.
//     		Contact : Keyle_xiao@hotmail.com 
//
//     		Licensed under the Apache License, Version 2.0 (the "License");
//     		you may not use this file except in compliance with the License.
//     		You may obtain a copy of the License at
//
//     		http://www.apache.org/licenses/LICENSE-2.0
//
//     		Unless required by applicable law or agreed to in writing, software
//     		distributed under the License is distributed on an "AS IS" BASIS,
//     		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     		See the License for the specific language governing permissions and
//     		limitations under the License.
//

using System;
using System.Collections.Generic;
using SmartDataViewer.Helpers;
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

        [NonSerialized] public Dictionary<int, T> Configs;

        public List<T> ConfigList;

        public virtual void InitConfigsFromChache()
        {
            if (Configs == null)
                Configs = new Dictionary<int, T>();
            else
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
                    continue;

                Configs.Add(item.ID, item);
            }
        }


        #region Option : CRUD

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

        #endregion


        #region 编辑器状态下资源读取，如果是实际项目中使用还请根据实际情况作出修改

        public virtual void DeleteFromDisk(string path)
        {
            path = PathMapping.GetInstance().DecodeURL(path);

            if (File.Exists(path))
                File.Delete(path);
        }


        public virtual void SaveToDisk(string path)
        {
            //Modified by keyle 2016.11.29 缩减配置尺寸
            string content = JsonUtility.ToJson(this, false);

            DeleteFromDisk(path);

            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(path, content);
        }


        public static V LoadConfig<V>(string path) where V : ConfigBase<T>, new()
        {
            string content = string.Empty;
            if (!PathMapping.GetInstance().DecodeURL(ref path))
            {
                TextAsset temp = Resources.Load<TextAsset>(path);

                if (temp == null)
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("Can't Find {0} , FileName {1}", typeof(V).Name, path));
#endif
                    return new V();
                }

                content = temp.text;
            }
            else
            {
                if (!File.Exists(path)) return new V();
                content = File.ReadAllText(path);
            }

            var result = JsonUtility.FromJson<V>(content);
            result.InitConfigsFromChache();
            return result;
        }


        public static object LoadRawConfig(Type t, string path)
        {
            string content = string.Empty;

            if (!PathMapping.GetInstance().DecodeURL(ref path))
            {
                TextAsset temp = Resources.Load<TextAsset>(path);

                if (temp == null)
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("Can't Find {0} , FileName {1}", t.Name, path));
#endif
                    return null;
                }

                content = temp.text;
            }
            else
            {
                if (!File.Exists(path)) return null;
                content = File.ReadAllText(path);
            }

            return JsonUtility.FromJson(content, t);
        }

        #endregion
    }
}
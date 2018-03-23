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


#if UNITY_EDITOR

        #region 编辑器状态下资源读取，如果是实际项目中使用还请根据实际情况作出修改

        public virtual void DeleteFromDisk(string fileWithNoExcension = "", bool absolute = false)
        {
            absolute = PathHelper.GetAbsolutePath<T>(ref fileWithNoExcension);
            string configPath = fileWithNoExcension;

            if (!absolute)
            {
                configPath = PathHelper.GetConfigAbsoluteFilePath(configPath);
            }

            if (File.Exists(configPath))
                File.Delete(configPath);
        }


        public virtual void SaveToDisk(string fileWithNoExcension = "", bool absolute = false)
        {
            absolute = PathHelper.GetAbsolutePath<T>(ref fileWithNoExcension);
            string configPath = fileWithNoExcension;

            //Modified by keyle 2016.11.29 缩减配置尺寸
            string content = JsonUtility.ToJson(this, false);

            DeleteFromDisk(configPath, absolute);

            if (!absolute)
            {
                configPath = PathHelper.GetConfigAbsoluteFilePath(configPath);
            }

            File.WriteAllText(configPath, content);
        }


        public static V LoadConfig<V>(string fileWithNoExcension = "", bool absolute = false)
            where V : ConfigBase<T>, new()
        {
            absolute = PathHelper.GetAbsolutePath<T>(ref fileWithNoExcension);
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

            var result = JsonUtility.FromJson<V>(str);
            result.InitConfigsFromChache();
            return result;
        }


        public static object LoadRawConfig(Type t, string fileWithNoExcension, bool absolute = false)
        {
            absolute = PathHelper.GetAbsolutePath(t, ref fileWithNoExcension);
            //Check Is Null

            string str = string.Empty;
            if (!absolute)
            {
                TextAsset temp = Resources.Load<TextAsset>(string.Format("Config/{0}", fileWithNoExcension));

                if (temp == null)
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("Can't Find {0} , FileName {1}", t.Name, fileWithNoExcension));
#endif
                    return null;
                }

                str = temp.text;
            }
            else
            {
                string path = fileWithNoExcension;
                if (!File.Exists(path))
                    return null;
                else
                    str = File.ReadAllText(path);
            }

            return JsonUtility.FromJson(str, t);
        }

        #endregion

#endif
    }
}
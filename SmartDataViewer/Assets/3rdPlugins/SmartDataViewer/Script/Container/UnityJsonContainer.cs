using System;
using System.IO;
using SmartDataViewer.Helpers;
using UnityEngine;

namespace SmartDataViewer
{
    public class UnityJsonContainer : IConfigContainer
    {
        /// <summary>
        /// 加载配置(静态)
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public V LoadConfig<V>(string path) where V : new()
        {
            string content = string.Empty;

            if (!PathMapping.GetInstance().DecodePath(ref path))
            {
                var temp = Resources.Load<TextAsset>(path);

                if (temp == null)
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("Can't Find {0} ", path));
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
            return result;
        }


        public object LoadConfig(Type t, string path)
        {
            var content = string.Empty;

            if (!PathMapping.GetInstance().DecodePath(ref path))
            {
                var temp = Resources.Load<TextAsset>(path);

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


        /// <summary>
        /// 从本地删除配置
        /// </summary>
        /// <param name="path"></param>
        public bool DeleteFromDisk(string path)
        {
            path = PathMapping.GetInstance().DecodePath(path);

            if (File.Exists(path))
                File.Delete(path);

            return true;
        }


        /// <summary>
        /// 保存配置到本地
        /// </summary>
        /// <param name="path"></param>
        public bool SaveToDisk(string path,object target)
        {
            //Modified by keyle 2016.11.29 缩减配置尺寸
            string content = JsonUtility.ToJson(target, false);

            DeleteFromDisk(path);

            path = PathMapping.GetInstance().DecodePath(path);

            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(path, content);

            return true;
        }
    }
}
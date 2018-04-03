using System;
using System.IO;
using SmartDataViewer.Helpers;
using UnityEngine;

namespace SmartDataViewer
{
    public class UnityJsonLoader:IConfigLoader
    {
        /// <summary>
        /// 加载配置(静态)
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public  V LoadConfig<V>(string path) where V : new()
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


        public  object LoadConfig(Type t, string path)
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
    }
}
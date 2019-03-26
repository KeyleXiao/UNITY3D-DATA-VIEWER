using System;
using System.IO;
using SmartDataViewer.Helpers;
using UnityEngine;

namespace SmartDataViewer
{
    public class UnityJsonContainer : ConfigContainerBase, IConfigContainer
    {
        public T LoadConfig<T>(string path) 
        {
            var content = string.Empty;

            if (!LoadText(path, ref content)) return default(T);

            return JsonUtility.FromJson<T>(content);
        }

        
        public object LoadConfig(Type t, string path)
        {
            var content = string.Empty;

            if (!LoadText(path, ref content)) return null;

            return JsonUtility.FromJson(content, t);
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
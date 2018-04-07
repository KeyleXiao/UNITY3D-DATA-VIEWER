using System;
using SmartDataViewer.Helpers;
using System.IO;
using UnityEngine;

namespace SmartDataViewer
{
    public class ConfigContainerBase 
    {

        public virtual bool LoadText(string path,ref string content)
        {
            if (!PathMapping.GetInstance().DecodePath(ref path))
            {
                var temp = Resources.Load<TextAsset>(path);

                if (temp == null)
                {
#if UNITY_EDITOR
                    Debug.Log(string.Format("Can't Find {0} ", path));
#endif
                    return false;
                }

                content = temp.text;
            }
            else
            {
                if (!File.Exists(path)) return false;
                content = File.ReadAllText(path);
            }

            return true;
        }


        public virtual bool SaveText(string path, string content)
        {
            DeleteFromDisk(path);

            path = PathMapping.GetInstance().DecodePath(path);

            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(path, content);
            
            return true;
        }



        /// <summary>
        /// 从本地删除配置
        /// </summary>
        /// <param name="path"></param>
        public virtual bool DeleteFromDisk(string path)
        {
            path = PathMapping.GetInstance().DecodePath(path);

            if (File.Exists(path))
                File.Delete(path);

            return true;
        }

    }
}
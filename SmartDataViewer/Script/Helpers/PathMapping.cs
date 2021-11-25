//
//    Copyright 2018 KeyleXiao.
//    Contact to Me : Keyle_xiao@hotmail.com 
//
//      Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at
//
//          http://www.apache.org/licenses/LICENSE-2.0
//
//          Unless required by applicable law or agreed to in writing, software
//          distributed under the License is distributed on an "AS IS" BASIS,
//          WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//          See the License for the specific language governing permissions and
//          limitations under the License.
//

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SmartDataViewer.Helpers
{
    /// <summary>
    /// TODO : 减小复杂度 新增适配逻辑
    /// </summary>
    public class PathMapping
    {
        private Dictionary<string, string> PathCache;

        private PathMapping()
        {
            PathCache = new Dictionary<string, string>();

#if UNITY_EDITOR
            PathCache.Add("{EDITOR}", GetDirectorieRootInUnity("SmartDataViewer"));
#endif
            PathCache.Add("{ROOT}", Application.dataPath);
            
            //---- 添加用户自定义的路径映射 ---- 
            
            
            
            //---- 添加用户自定义的路径映射 ---- 
        }

        private static PathMapping instance;

        public static PathMapping GetInstance()
        {
            return instance ?? (instance = new PathMapping());
        }

        public string DecodePath(string url)
        {
            foreach (var mapping in PathCache)
            {
                if (url.Contains(mapping.Key))
                {
                    url = url.Replace(mapping.Key, mapping.Value);
                }
            }
            return url;
        }
        
        
        /// <summary>
        /// 补全路径信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns>如果返回false 则走默认路径</returns>
        public bool DecodePath(ref string url)
        {
            bool isAbs = false;
            
            foreach (var mapping in PathCache)
            {
                if (url.Contains(mapping.Key))
                {
                    url = url.Replace(mapping.Key, mapping.Value);
                    isAbs = true;
                }
            }
            return isAbs;
        }
        

#if UNITY_EDITOR
        /// <summary>
        /// 获取根目录方法(不要用在会有重名的地方)
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public string GetDirectorieRootInUnity(string folderName)
        {
            string[] res =
                Directory.GetDirectories(Application.dataPath, folderName , SearchOption.AllDirectories);
            return res[0].Replace("\\", "/");
        }
#endif

        static public string PathCombine(string pathA, string pathB)
        {
            return Path.Combine(pathA, pathB).Replace("\\", "/");
        }
    }
}
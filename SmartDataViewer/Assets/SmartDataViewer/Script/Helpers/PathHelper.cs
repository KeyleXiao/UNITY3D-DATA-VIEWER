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

using System;
using System.IO;
using System.Runtime.Remoting;
using UnityEngine;

namespace SmartDataViewer
{
    public static class PathHelper
    {
        static public string GetConfigAbsoluteFilePath(string filename,string extionsion = ".txt")
        {
            #if UNITY_EDITOR
            return string.Format(@"{0}/Resources/Config/{1}{2}", Application.dataPath, filename,extionsion);
            #endif
            
            
            return string.Empty;
        }

        static string RootSymbol = "{ROOT}";
        static string EditorSymbol = "{EDITOR}";
        
        static  public  bool GetAbsolutePath(Type t,ref string fileNameOrPath)
        {
            if (fileNameOrPath.Contains(RootSymbol))
            {
                fileNameOrPath = fileNameOrPath.Replace(RootSymbol, Application.dataPath);
                if (!fileNameOrPath.EndsWith(".txt"))
                    fileNameOrPath += ".txt";
            }
            else if (fileNameOrPath.Contains(EditorSymbol))
            {
                fileNameOrPath =
                    fileNameOrPath.Replace(EditorSymbol, Application.dataPath + "/SmartDataViewer");
                if (!fileNameOrPath.EndsWith(".txt"))
                    fileNameOrPath += ".txt";
            }
            else
            {
                if (string.IsNullOrEmpty(fileNameOrPath))
                {
                    fileNameOrPath = t.Name;
                }

                return false;
            }
            return true;
        }
        
        
        static  public  bool GetAbsolutePath<T>(ref string fileNameOrPath)
        {
            return GetAbsolutePath(typeof(T),ref fileNameOrPath);
        }
        
        static public string PathCombine(string pathA, string pathB)
        {
            return Path.Combine(pathA, pathB).Replace("\\", "/");
        }

        public static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                case RuntimePlatform.WindowsEditor:
                    return "Android";
                case RuntimePlatform.OSXEditor:
                    return "iOS";
                // Add more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return "Android";
            }
        }

        public static string GetPlatformName()
        {
#if UNITY_EDITOR && UNITY_IOS
		return GetPlatformForAssetBundles(RuntimePlatform.IPhonePlayer);
#endif

#if UNITY_EDITOR && UNITY_ANDROID
		return GetPlatformForAssetBundles(RuntimePlatform.Android);
#endif
            return GetPlatformForAssetBundles(Application.platform);
        }

        static public string UrlEncode(string str)
        {
            return Uri.EscapeDataString(str);
        }

        static public string UrlDecode(string str)
        {
            return Uri.UnescapeDataString(str);
        }

        static public string GetFileURL(string path)
        {
            return (new Uri(path)).AbsoluteUri;
        }

        static public string GetTemplateTxtFolder()
        {
            string path = Application.dataPath + "/SmartDataViewer/CTS/";
            return path;
        }

        static public string GetRelativePath(string content = "")
        {
            string path = Application.dataPath + "/SmartDataViewer/CTS/";

            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            return PathCombine(path, content);
        }


        static public string GetLocalAssetBundleFolder()
        {
            return PathCombine(Application.persistentDataPath, GetPlatformName());
        }

        /**
         *  随机数函数
         */
        static char[] constant = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

        static public string GenerateRandomNumber(int length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder();
            System.Random rd = new System.Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }

            return newRandom.ToString();
        }

#if UNITY_EDITOR
        public static string GetEditorOutPutPath(RuntimePlatform platform, bool isRoot = false)
        {
            return string.Format("{0}/StreamingAssets/{1}", Application.dataPath, GetPlatformForAssetBundles(platform));
        }


        public static void DebugWrite(string currentLog)
        {
            string logFilePath = string.Format("{0}/Logs", Application.dataPath);

            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }

            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            if (!string.IsNullOrEmpty(currentLog))
            {
                File.WriteAllText(string.Format("{0}/{1}.txt", logFilePath, System.DateTime.Now.ToLongDateString()),
                    currentLog);
            }
        }
#endif
    }
}
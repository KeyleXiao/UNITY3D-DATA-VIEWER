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
using SmartDataViewer.Helpers;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace SmartDataViewer.Helpers
{
    public static class PathHelper
    {
        public enum PATH_TYPE
        {
            ASSETDATABASE,
            ABSOLUTE,
            RESOURCES,
            PERSISTENT
        }

        // ## 当前代码只在编辑器下使用
#if UNITY_EDITOR


        static string EditorResourcesPath = "{EDITOR}/EditorResources/";
        static string EditorClassTemplate = "{EDITOR}/CTS/EditorClassTemplate.txt";
        static string EditorSymbol = "{EDITOR}";
        static string RealEditorPath = "";
        static string RealEditorExportPath = "";
        static string RealEditorClassTemplatePath = "";
        static string RealEditorResourcesPath = "";


        /// <summary>
        /// 默认获取SmartDataViewer的 EditorResources 绝对路径 使用枚举来控制是否使用Assetdatabase路径
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="pathType"></param>
        /// <returns></returns>
        static public string GetEditorResourcePath(string resourceName = "", PATH_TYPE pathType = PATH_TYPE.ABSOLUTE)
        {
            if (string.IsNullOrEmpty(RealEditorResourcesPath))
            {
                RealEditorResourcesPath = EditorResourcesPath.Replace(EditorSymbol, GetRootEditorPath());
            }


            var result = RealEditorResourcesPath;

            if (!string.IsNullOrEmpty(resourceName))
            {
                result = PathCombine(RealEditorResourcesPath, resourceName);
            }

            //处理多种路径情况
            if (pathType == PATH_TYPE.ASSETDATABASE)
            {
                var splitIndex = result.IndexOf("Assets/");
                result = result.Substring(splitIndex);
            }

            return result;
        }

        static public string GetTemplatetEditorClassPath()
        {
            if (!string.IsNullOrEmpty(RealEditorClassTemplatePath))
                return RealEditorClassTemplatePath;

            if (string.IsNullOrEmpty(RealEditorPath))
                RealEditorPath = GetRootEditorPath();

            return RealEditorClassTemplatePath = EditorClassTemplate.Replace("{EDITOR}", RealEditorPath);
        }

        static public string GetRootEditorPath()
        {
            if (!string.IsNullOrEmpty(RealEditorPath))
                return RealEditorPath;

            string[] res =
                Directory.GetDirectories(Application.dataPath, "SmartDataViewer", SearchOption.AllDirectories);
            return RealEditorPath = res[0];
        }

        static public string GetEditorExportPath()
        {
            if (!string.IsNullOrEmpty(RealEditorExportPath))
            {
                return RealEditorExportPath;
            }

            RealEditorExportPath = string.Format(@"{0}/Editor/Export/", Application.dataPath);

            if (!Directory.Exists(RealEditorExportPath))
            {
                Directory.CreateDirectory(RealEditorExportPath);
            }

            return RealEditorExportPath;
        }

#endif


        static string RootSymbol = "{ROOT}";


        static public string GetConfigAbsoluteFilePath(string filename, string extionsion = ".txt")
        {
#if UNITY_EDITOR
            return string.Format(@"{0}/Resources/Config/{1}{2}", Application.dataPath, filename, extionsion);
#else
            return string.Empty;
#endif
        }


        static public bool GetAbsolutePath(Type t, ref string fileNameOrPath, string extension = ".txt")
        {
            if (fileNameOrPath.Contains(RootSymbol))
            {
                fileNameOrPath = fileNameOrPath.Replace(RootSymbol, Application.dataPath);
                if (!fileNameOrPath.EndsWith(extension))
                    fileNameOrPath += extension;
            }
#if UNITY_EDITOR
            else if (fileNameOrPath.Contains(EditorSymbol))
            {
#if UNITY_EDITOR
                //这里的逻辑只会在编辑器状态下被使用
                fileNameOrPath =
                    fileNameOrPath.Replace(EditorSymbol, GetRootEditorPath());
                if (!fileNameOrPath.EndsWith(extension))
                    fileNameOrPath += extension;
#endif
            }
#endif
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


        static public bool GetAbsolutePath<T>(ref string fileNameOrPath, string extension = ".txt")
        {
            return GetAbsolutePath(typeof(T), ref fileNameOrPath, extension);
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
#else
            return GetPlatformForAssetBundles(Application.platform);
#endif
        }

        static public string UrlEncode(string str)
        {
            return Uri.EscapeDataString(str);
        }

        static public string UrlDecode(string str)
        {
            return Uri.UnescapeDataString(str);
        }
    }
}
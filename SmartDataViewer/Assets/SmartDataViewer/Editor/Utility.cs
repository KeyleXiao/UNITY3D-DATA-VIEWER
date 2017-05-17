//
//    Copyright 2017 KeyleXiao.
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

using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartDataViewer.Editor
{

	public enum FieldType
	{
		INT,
		STRING,
		FLOAT,
		BOOL,
		VECTOR2,
		VECTOR3,
		VECTOR4,
		COLOR,
		CURVE,
		BOUNDS,

		GEN_INT,
		GEN_STRING,
		GEN_FLOAT,
		GEN_BOOL,
		GEN_VECTOR2,
		GEN_VECTOR3,
		GEN_VECTOR4,
		GEN_COLOR,
		GEN_CURVE,
		GEN_BOUNDS,

		ANIMATIONCURVE,
		GEN_ANIMATIONCURVE,
		ENUM,
		GEN_ENUM,

	}


	public class Utility
	{
		public static DefaultControlConfig ControlConfig { get; set; }

		public static DefaultControlConfig GetDefaultControlConfig()
		{
			if (ControlConfig == null)
			{
				ControlConfig = DefaultControlConfig.LoadConfig<DefaultControlConfig>("{ROOT}/SmartDataViewer/Config/DefaultControlPropertity");
			}
			return ControlConfig;
		}

		public static T DeepClone<T>(T a)
		{
			var content = JsonUtility.ToJson(a);
			return JsonUtility.FromJson<T>(content);
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

		static public string PathCombine(string pathA, string pathB)
		{
			return Path.Combine(pathA, pathB).Replace("\\", "/");
		}

		/**
		 *  随机数函数
		 */
		static char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

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
				File.WriteAllText(string.Format("{0}/{1}.txt", logFilePath, System.DateTime.Now.ToLongDateString()), currentLog);
			}
		}
#endif

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
				case RuntimePlatform.OSXWebPlayer:
				case RuntimePlatform.WindowsWebPlayer:
					return "WebPlayer";
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

	}
}
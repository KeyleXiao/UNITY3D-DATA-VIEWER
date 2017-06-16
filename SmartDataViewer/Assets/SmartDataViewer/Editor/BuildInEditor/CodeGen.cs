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
using UnityEngine;
using UnityEditor;
using System.IO;

namespace SmartDataViewer.Editor
{
	public class CodeGenConfigEditor : ConfigEditorSchema<CodeGen>
	{
		string RootSymbol = "{ROOT}";
		string EditorSymbol = "{EDITOR}";


		public string GetAbsolutePath(string fileWithNoExcension, bool isDir = false)
		{
			if (fileWithNoExcension.Contains(RootSymbol))
			{
				fileWithNoExcension = fileWithNoExcension.Replace(RootSymbol, Application.dataPath);
				if (!isDir && !fileWithNoExcension.EndsWith(".txt")) fileWithNoExcension += ".txt";
			}
			else if (fileWithNoExcension.Contains(EditorSymbol))
			{
				fileWithNoExcension = fileWithNoExcension.Replace(EditorSymbol, Application.dataPath + "/SmartDataViewer");
				if (!isDir && !fileWithNoExcension.EndsWith(".txt")) fileWithNoExcension += ".txt";
			}
			return fileWithNoExcension;
		}

		//{ROOT}/Editor/SceneEditor/NewEventEditor
		[MenuItem("SmartDataViewer/CodeGen")]
		static public void OpenView()
		{
			CodeGenConfigEditor w = CreateInstance<CodeGenConfigEditor>();
			w.ShowUtility();
		}


		protected override void RenderExtensionButton(CodeGen item)
		{
			//string filePath = GetAbsolutePath(item.CodeFilePath);
			string dirPath = GetAbsolutePath(item.CodeFileFolder, true);

			if (Directory.Exists(dirPath))
			{
				if (GUILayout.Button(Language.Build, new GUILayoutOption[] { GUILayout.Width(90) }))
					WriteFile(item);
			}
			else
			{
				GUILayout.Label("", GUI.skin.GetStyle("CN EntryWarn"), new GUILayoutOption[] { GUILayout.Width(50) });
			}
			//base.RenderExtensionButton(item);
		}

		string templateFile { get; set; }


		public override void Initialize()
		{
			SetConfigType(new CodeGenConfig());


			string path = GetAbsolutePath("{EDITOR}/CTS/EditorClassTemplate");
			Debug.Log(path);
			if (File.Exists(path))
			{
				templateFile = File.ReadAllText(path);
			}
		}

		public void WriteFile(CodeGen item)
		{
			if (string.IsNullOrEmpty(templateFile)) return;
			string temp = templateFile;
			temp = temp.Replace("$[EditorName]", item.EditorName);
			temp = temp.Replace("$[ClassType]", item.ClassType);
			temp = temp.Replace("$[SubType]", item.SubType);
			string path = GetAbsolutePath(item.CodeFileFolder, true) + "/" + item.EditorName + ".cs";
			if (File.Exists(path)) File.Delete(path);
			File.WriteAllText(path, temp, System.Text.Encoding.UTF8);
			AssetDatabase.Refresh();
			ShowNotification(new GUIContent("Success"));
		}


	}

	[System.Serializable]
	[ConfigEditor(disableSearch: true, load_path: "{EDITOR}/Config/CodeGen")]
	public class CodeGenConfig : ConfigBase<CodeGen> { }

	[System.Serializable]
	public class CodeGen : IModel
	{
		public CodeGen()
		{
			NickName = string.Empty;
			CodeFileFolder = string.Empty;
			EditorName = string.Empty;
			ClassType = string.Empty;
			SubType = string.Empty;
		}
		public string CodeFileFolder;
		public string EditorName;
		public string ClassType;
		public string SubType;
	}

}
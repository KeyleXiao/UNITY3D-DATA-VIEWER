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
		//{ROOT}/Editor/SceneEditor/NewEventEditor
		[MenuItem("SmartDataViewer/CodeGen")]
		static public void OpenView()
		{
			CodeGenConfigEditor w = CreateInstance<CodeGenConfigEditor>();
			w.ShowUtility();
		}


		protected override void RenderExtensionButton(CodeGen item)
		{
			base.RenderExtensionButton(item);
			
			string errorInfo = string.Empty;

			if (string.IsNullOrEmpty(item.EditorName))
			{
				errorInfo = "Editor Name Is Empty";
			}
			else if (string.IsNullOrEmpty(item.SubType))
			{
				errorInfo = "SubType Is Empty";
			}

			if (string.IsNullOrEmpty(errorInfo))
			{
				if (GUILayout.Button(Language.Build, new GUILayoutOption[] { GUILayout.Width(90) }))
					WriteFile(item);
			}
			else
			{
				if (GUILayout.Button("", GUI.skin.GetStyle("CN EntryWarn"), new GUILayoutOption[] { GUILayout.Width(50) }))
				{
					ShowNotification(new GUIContent(errorInfo));
				}
			}
		}

		string templateFile { get; set; }

		public override void Initialize()
		{
			base.Initialize();
			
			SetConfigType(new CodeGenConfig());


			string path = PathHelper.GetTemplatetEditorClassPath();
			
			if (File.Exists(path))
				templateFile = File.ReadAllText(path);
			
		}

		public void WriteFile(CodeGen item)
		{
			if (string.IsNullOrEmpty(templateFile)) return;
			string temp = templateFile;
			temp = temp.Replace("$[EditorName]", item.EditorName);
			temp = temp.Replace("$[ClassType]", item.ClassType);
			temp = temp.Replace("$[SubType]", item.SubType);
			temp = temp.Replace("$[EditorPath]", item.EditorPath);

			//Gen file path
			string path = string.Format("{0}/{1}.cs", PathHelper.GetEditorExportPath(), item.EditorName);
			
			if (File.Exists(path)) File.Delete(path);
			File.WriteAllText(path, temp, System.Text.Encoding.UTF8);
			Debug.LogWarning(string.Format("Success Build !\n{0}",path));
			AssetDatabase.Refresh();
			Close();
		}


	}

	[Serializable]
	[ConfigEditor(disableSearch: true, load_path: "{EDITOR}/Config/CodeGen")]
	public class CodeGenConfig : ConfigBase<CodeGen> { }

	[Serializable]
	public class CodeGen : IModel
	{
		public CodeGen()
		{
			EditorName = string.Empty;
			ClassType = string.Empty;
			SubType = string.Empty;
			EditorPath = "CustomEditor";
		}
		public string EditorPath;
		public string EditorName;
		public string ClassType;
		public string SubType;
	}

}
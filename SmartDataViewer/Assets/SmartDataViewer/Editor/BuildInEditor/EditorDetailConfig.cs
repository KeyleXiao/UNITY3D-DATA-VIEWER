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
using SmartDataViewer;
using UnityEditor;

namespace SmartDataViewer.Editor
{
	[ConfigEditor(LoadPath = @"{ROOT}/Config/EditorDetailConfig")]
	[Serializable]
	public class EditorDetailConfig : ConfigBase<EditorDetail>
	{

	}


	[Serializable]
	public class EditorDetail : IModel
	{
		public EditorDetail()
		{
			FieldName = string.Empty;
			Description = string.Empty;
		}
		[ConfigEditorField(can_editor: true)]
		public FieldType CurrentType;
		[ConfigEditorField(can_editor: true)]
		public string FieldName;
		[ConfigEditorField(can_editor: true)]
		public bool CanEditor;
		[ConfigEditorField(can_editor: true)]
		public string Description;
	}

	public class EditorDetailConfigEditor : ConfigEditorSchema<EditorDetail>
	{
		[MenuItem("SmartDataVier/EditorDetailConfig")]
		static public void OpenView()
		{
			EditorDetailConfigEditor w = CreateInstance<EditorDetailConfigEditor>();
			w.ShowUtility();
		}

		public override EditorDetail CreateValue()
		{
			EditorDetail r = base.CreateValue();
			return r;
		}

		public override void Initialize()
		{
			SetConfigType(new EditorDetailConfig());
		}
	}
}



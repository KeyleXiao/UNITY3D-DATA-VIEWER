//
//    Copyright 2017 KeyleXiao.
//    Contact to Me : Keyle_xiao@hotmail.com 
//
//   	Licensed under the Apache License, Version 2.0 (the "License");
//   	you may not use this file except in compliance with the License.
//   	You may obtain a copy of the License at
//
//   		http://www.apache.org/licenses/LICENSE-2.0
//
//   		Unless required by applicable law or agreed to in writing, software
//   		distributed under the License is distributed on an "AS IS" BASIS,
//   		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   		See the License for the specific language governing permissions and
//   		limitations under the License.
//

using System;

namespace SmartDataViewer
{
	public class BaseConfigFiedEditorAttribute : Attribute
	{
		public static T GetCurrentAttribute<T>(object obj, bool inherited = true)
		{
			object[] o = obj.GetType().GetCustomAttributes(typeof(T), inherited);
			if (o == null || o.Length < 1)
			{
				return default(T);
			}
			return ((T)o[0]);
		}
	}

	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public sealed class ConfigEditorFieldAttribute : BaseConfigFiedEditorAttribute
	{
		public int Order { get; set; }
		public string Display { get; set; }
		public bool CanEditor { get; set; }
		public int Width { get; set; }

		public ConfigEditorFieldAttribute(int order = 0, bool can_editor = true, string display = "",
										  int width = 100, bool isPrimarykey = false, Type outLinkClass = null,
										  string outLinkField = null
										 )
		{
			Order = order;
			Display = display;
			CanEditor = can_editor;
			Width = width;

		}
	}

	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public sealed class ConfigEditorAttribute : BaseConfigFiedEditorAttribute
	{
		public string LoadPath { get; set; }
		public string OutputPath { get; set; }
		public string EditorTitle { get; set; }
		public bool DisableSearch { get; set; }
		public bool DisableSave { get; set; }
		public bool DisableCreate { get; set; }

		public ConfigEditorAttribute(string editor_title = "",
									 string load_path = "",
									 string output_path = "",
									 bool disableSearch = false,
									 bool disableSave = false,
									 bool disableCreate = false)
		{
			EditorTitle = editor_title;
			OutputPath = output_path;
			LoadPath = load_path;
			DisableSave = disableSave;
			DisableCreate = disableCreate;
			DisableSearch = disableSearch;
		}
	}
}
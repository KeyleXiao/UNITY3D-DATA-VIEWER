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

using System.Collections.Generic;
using SmartDataViewer.Editor.BuildInEditor;

namespace SmartDataViewer.Editor
{
	public enum WindowType
	{
		INPUT,
		CALLBACK
	}
	

	
	public class EditorConfig
	{
		public static DefaultControlPropertityConfig ControlPropertyConfig { private get; set; }
		public static DefaultEditorPropertyConfig EditorPropertyConfig { private get; set; }
		
		public static CustomControlPropertityConfig CustomControlPropertyConfig { private get; set; }
		public static CustomEditorPropertyConfig CustomEditorPropertyConfig { private get; set; }
		

		public static CustomControlPropertityConfig GetCustomControlConfig(bool reload = false)
		{
			if (CustomControlPropertyConfig == null || reload)
			{
				CustomControlPropertyConfig = CustomControlPropertityConfig.LoadConfig<CustomControlPropertityConfig>("{EDITOR}/Config/CustomControlProperty");
			}
			return CustomControlPropertyConfig;
		}
		
		public static CustomEditorPropertyConfig GetCustomEditorConfig(bool reload = false)
		{
			if (CustomEditorPropertyConfig == null || reload)
			{
				CustomEditorPropertyConfig = DefaultEditorPropertyConfig.LoadConfig<CustomEditorPropertyConfig>("{EDITOR}/Config/CustomEditorPropertyConfig");
			}
			return CustomEditorPropertyConfig;
		}
		
		public static DefaultControlPropertityConfig GetDefaultControlConfig(bool reload = false)
		{
			if (ControlPropertyConfig == null || reload)
			{
				ControlPropertyConfig = DefaultControlPropertityConfig.LoadConfig<DefaultControlPropertityConfig>("{EDITOR}/Config/DefaultControlProperty");
			}
			return ControlPropertyConfig;
		}
		
		public static DefaultEditorPropertyConfig GetDefaultEditorConfig(bool reload = false)
		{
			if (EditorPropertyConfig == null || reload)
			{
				EditorPropertyConfig = DefaultEditorPropertyConfig.LoadConfig<DefaultEditorPropertyConfig>("{EDITOR}/Config/DefaultEditorPropertyConfig");
			}
			return EditorPropertyConfig;
		}
		
	}		

	public class Language
	{
		//public static readonly string Build = "生成";
		//public static readonly string Select = "选择";
		//public static readonly string Previous = "前页";
		//public static readonly string Next = "后页";
		//public static readonly string Add = "添加";
		//public static readonly string OutLinkIsNull = "请设置外链编辑器";
		//public static readonly string Success = "成功..";
		//public static readonly string SuccessAdd = "成功添加 {0}";
		//public static readonly string NickName = @"别名";
		//public static readonly string Delete = @"X";
		//public static readonly string Copy = @"C";
		//public static readonly string Paste = @"P";
		//public static readonly string Operation = @"操作";
		//public static readonly string Contract = @"Version 1.2 Beta   --Keyle";
		//public static readonly string OnePageMaxNumber = "单页最大数量";
		//public static readonly string PageInfoFormate = @"|{0}|页-共|{1}|页";

		public static readonly string Jump = "Jump:";
		public static readonly string NewLine = "NewLine";
		public static readonly string DiscardChange = "Discard";
		public static readonly string Menu = "Menu";
		public static readonly string Build = "Build";
		public static readonly string ReBuild = "Rebuild";
		public static readonly string Select = "Select";
		public static readonly string Previous = "Previous";
		public static readonly string Next = "Next";
		public static readonly string Add = "Add";
		public static readonly string OutLinkIsNull = "Out link editor field is null";
		public static readonly string Success = "Success ..";
		public static readonly string SuccessAdd = "Success add {0}";
		public static readonly string NickName = @"NickName";
		public static readonly string Delete = @"X";
		public static readonly string Copy = @"C";
		public static readonly string Paste = @"P";
		public static readonly string Verfiy = @"V";
		public static readonly string VerfiyMessageSuccess = @"ID: {0} 当前数据有效！";
		public static readonly string Operation = @"Operation";
		public static readonly string Contract = @"Version 1.2 Beta   --Keyle";
		public static readonly string OnePageMaxNumber = "Max In Page";
		public static readonly string PageInfoFormate = @"Page |{0}|-|{1}|";
	}
}
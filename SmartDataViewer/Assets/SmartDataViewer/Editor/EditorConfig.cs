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

namespace SmartDataViewer.Editor
{
	public enum WindowType
	{
		INPUT,
		CALLBACK
	}
	
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

	
	public class EditorConfig
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
	}		

	public class Language
	{
		//public static string Build = "生成";
		//public static string Select = "选择";
		//public static string Previous = "前页";
		//public static string Next = "后页";
		//public static string Add = "添加";
		//public static string OutLinkIsNull = "请设置外链编辑器";
		//public static string Success = "成功..";
		//public static string SuccessAdd = "成功添加 {0}";
		//public static string NickName = @"别名";
		//public static string Delete = @"X";
		//public static string Copy = @"C";
		//public static string Paste = @"P";
		//public static string Operation = @"操作";
		//public static string Contract = @"Version 1.2 Beta   --Keyle";
		//public static string OnePageMaxNumber = "单页最大数量";
		//public static string PageInfoFormate = @"|{0}|页-共|{1}|页";

		public static string Build = "Build";
		public static string Select = "Select";
		public static string Previous = "Previous";
		public static string Next = "Next";
		public static string Add = "Add";
		public static string OutLinkIsNull = "Out link editor field is null";
		public static string Success = "Success ..";
		public static string SuccessAdd = "Success add {0}";
		public static string NickName = @"NickName";
		public static string Delete = @"X";
		public static string Copy = @"C";
		public static string Paste = @"P";
		public static string Operation = @"Operation";
		public static string Contract = @"Version 1.2 Beta   --Keyle";
		public static string OnePageMaxNumber = "Max In Page";
		public static string PageInfoFormate = @"Page |{0}|-|{1}|";
	}
}
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
	[ConfigEditor("通用编辑")]
	[Serializable]
	public class FilterOptionConfig : ConfigBase<FilterOption>
	{

	}

	[Serializable]
	public class FilterOption : IModel
	{
		[ConfigEditorField(2, true)]
		public FilterModel filter_mode;

		[ConfigEditorField(3, true)]
		public EnumLevel currentLevel;

		[ConfigEditorField(4, true)]
		public EnumStatus currentStatus;


		[ConfigEditorField(5, true, "条件(1-10)")]
		public int extionsion_condition;

		[ConfigEditorField(4, true)]
		public bool Enable;
	}

	public enum EnumStatus
	{
		A,
		B,
		C
	}


	public enum EnumLevel
	{
		VIP1,
		VIP2,
		VIP3
	}


	public enum FilterModel
	{
		DEFAULT,
		STATUS,
		LEVEL
	}
}
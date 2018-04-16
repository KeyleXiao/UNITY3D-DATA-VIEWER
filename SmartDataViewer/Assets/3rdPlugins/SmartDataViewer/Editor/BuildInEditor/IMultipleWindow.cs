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
using System.Collections.Generic;
using System.Reflection;

namespace SmartDataViewer.Editor.BuildInEditor
{

	public class IMultipleWindow : UnityEditor.EditorWindow
	{
		protected enum SelectModel
		{
			SINGLE,
			MUTI
		}
		
		protected WindowType current_windowType = WindowType.INPUT;
		protected Action<List<int>, IModel> select_callback { get; set; }
		protected Action<object,IModel,FieldInfo> single_select_callback { get; set; }
		protected SelectModel select_model { get; set; }

		/// <summary>
		/// 保存界面打开者的处理数据，在选择回调的时候还回去
		/// </summary>
		protected List<int> selector_raw_list { get; set; }
		protected object selector_raw_single { get; set; }
		protected FieldInfo selector_raw_field_single { get; set; }


		public virtual void UpdateSelectModel(List<int> curren_raw_list, Action<List<int>, IModel> callback)
		{
			select_callback = callback;
			select_model = SelectModel.MUTI;
			current_windowType = WindowType.CALLBACK;
			selector_raw_list = curren_raw_list;
		}
		
		public virtual void UpdateSelectModel(object raw,FieldInfo field,Action<object,IModel,FieldInfo> callback)
		{
			single_select_callback = callback;
			select_model = SelectModel.SINGLE;
			current_windowType = WindowType.CALLBACK;
			selector_raw_single = raw;
			selector_raw_field_single = field;
		}
	}
}

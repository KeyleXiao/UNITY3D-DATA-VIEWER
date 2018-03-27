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

namespace SmartDataViewer.Editor.BuildInEditor
{

	public class IMultipleWindow : UnityEditor.EditorWindow
	{
		protected WindowType current_windowType = WindowType.INPUT;
		protected Action<object, object> select_callback { get; set; }
		protected object current_list { get; set; }

		//Single item selcted
		protected List<int> SingleTempSelect { get; set; }
		protected Dictionary<int, string> SingleSelectInfo { get; set; }

		public virtual void SetListItemValue(object item, object addValue)
		{
			var temp = item as List<int>;
			var model = addValue as IModel;
			temp.Add(model.ID);
		}

		public virtual void AddSingleSelectFlag(int id, string name)
		{
			if (SingleTempSelect == null) SingleTempSelect = new List<int>();
			if (SingleSelectInfo == null) SingleSelectInfo = new Dictionary<int, string>();
			if (!SingleSelectInfo.ContainsKey(id)) SingleSelectInfo.Add(id, name);
		}

		public int GetSingleSelectValueByFlag(int id, string filed, int rawVlaue)
		{
			if (SingleTempSelect == null) SingleTempSelect = new List<int>();
			if (SingleSelectInfo == null) SingleSelectInfo = new Dictionary<int, string>();

			if (SingleTempSelect.Count == 0 ||
				!SingleSelectInfo.ContainsKey(id) ||
				SingleSelectInfo[id] != filed) return rawVlaue;


			int result = SingleTempSelect[SingleTempSelect.Count - 1];
			SingleSelectInfo.Clear();
			SingleTempSelect.Clear();
			return result;
		}

		public virtual void UpdateSelectModel(object curren_list, Action<object, object> callback) { }
	}
}

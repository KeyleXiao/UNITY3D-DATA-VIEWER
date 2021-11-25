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
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SmartDataViewer.Helpers;
using SmartDataViewer;
using SmartDataViewer.Editor;
using SmartDataViewer.Editor.BuildInEditor;

namespace SmartDataViewer.Editor.BuildInEditor
{
	public class ConfigEditorLineCache<T> where T:new()
	{
		public ConfigEditorLineCache()
		{
			ColumnInfo = new List<ConfigEditorLineFieldCache>();
			SearchKey = string.Empty;
		}
		
		public int HashCode { get; set; }

		public string SearchKey { get; set; }

		public T RawData { get; set; }

		public List<ConfigEditorLineFieldCache> ColumnInfo { get; set; }
	}

	/// <summary>
	/// 存储反射的行的列数据
	/// </summary>
	public class ConfigEditorLineFieldCache
	{
		public ConfigEditorSchemaChache CurrentSchema { get; set; }
		public int HashCode { get; set; }
		public bool IsGenericType { get; set; }
		public MethodInfo Add { get; set; }
		public MethodInfo RemoveAt { get; set; }
		public PropertyInfo Count { get; set; }
		public PropertyInfo Item { get; set; }
		public Type AttributeType { get; set; }

		public object RawData { get; set; }

		public void Invoke_Add(object obj)
		{
			if (RawData == null) return;
			Add.Invoke(RawData, new object[] {obj});
		}

		public void Invoke_RemoveAt(int removeIndex)
		{
			if (RawData == null) return;
			RemoveAt.Invoke(RawData, new object[] {removeIndex});
		}


		public int Invoke_Count()
		{
			if (RawData == null) return 0;
			return Convert.ToInt32(Count.GetValue(RawData, null));
		}

		public object Invoke_GetItem(int index)
		{
			if (RawData == null) return 0;
			return Item.GetValue(RawData, new object[] {index});
		}

		public void Invoke_SetItem(object obj, int index)
		{
			if (RawData == null) return;
			Item.SetValue(RawData, obj, new object[] {index});
		}
	}
}
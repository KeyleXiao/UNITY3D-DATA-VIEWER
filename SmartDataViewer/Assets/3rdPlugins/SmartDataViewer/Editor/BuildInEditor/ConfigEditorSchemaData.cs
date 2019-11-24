//
//    Copyright 2017 KeyleXiao.
//    Contact to Me : Keyle_xiao@hotmail.com 
//
//      Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at
//
//          http://www.apache.org/licenses/LICENSE-2.0
//
//          Unless required by applicable law or agreed to in writing, software
//          distributed under the License is distributed on an "AS IS" BASIS,
//          WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//          See the License for the specific language governing permissions and
//          limitations under the License.
//


using System.Collections.Generic;
using System.Reflection;

namespace SmartDataViewer.Editor.BuildInEditor
{
	public class ConfigEditorSchemaData<T> where T :new()
	{
		public ConfigEditorSchemaData()
		{
			CurrentClassFiledsChache = new List<ConfigEditorSchemaChache>();
			LineFieldChache = new List<ConfigEditorLineFieldChache>();
			LineChaches = new List<ConfigEditorLineChache>();
		}
		
		/// <summary>
		/// 当前编辑器程序集
		/// </summary>
		public Assembly CurrentAssembly { get; set; }
		
		/// <summary>
		/// 当前编辑对象
		/// </summary>
		public ConfigBase<T> config_current { get; set; }
		
		/// <summary>
		/// 当前编辑器配置
		/// </summary>
		public EditorProperty currentEditorSetting;
        
		/// <summary>
		/// 缓存当前类反射字段信息
		/// </summary>
		public List<ConfigEditorSchemaChache> CurrentClassFiledsChache { get; set; }

		/// <summary>
		/// 缓存行级别字段结构
		/// </summary>
		public List<ConfigEditorLineFieldChache> LineFieldChache { get; set; }

		/// <summary>
		/// 缓存行列表
		/// </summary>
		public List<ConfigEditorLineChache> LineChaches { get; set; }
	}
}
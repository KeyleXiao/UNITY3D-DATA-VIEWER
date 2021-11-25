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
using UnityEngine;

namespace SmartDataViewer.Editor.BuildInEditor
{
	public class ConfigEditorSchemaData<T> where T :new()
	{
		public ConfigEditorSchemaData()
		{
			CurrentClassFieldsCache = new List<ConfigEditorSchemaChache>();
			LineFieldsCache = new List<ConfigEditorLineFieldCache>();
			LinesCache = new List<ConfigEditorLineCache<T>>();
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
		public List<ConfigEditorSchemaChache> CurrentClassFieldsCache { get; set; }
		
		/// <summary>
		/// 缓存行级别字段结构
		/// </summary>
		public List<ConfigEditorLineFieldCache> LineFieldsCache { get; set; }

		/// <summary>
		/// 缓存行列表
		/// </summary>
		public List<ConfigEditorLineCache<T>> LinesCache { get; set; }

		public bool LineFieldCacheContains(int hashcode)
		{
			for (int i = 0; i < LineFieldsCache.Count; i++)
			{
				if (LineFieldsCache[i].HashCode == hashcode)
				{
					return true;
				}
			}

			return false;
		}
		
		public bool LineCacheContains(int hashcode)
		{
			for (int i = 0; i < LinesCache.Count; i++)
			{
				if (LinesCache[i].HashCode == hashcode)
				{
					return true;
				}
			}
			return false;
		}



		public void SyncData(T lineData)
		{
			var hash = lineData.GetHashCode();
			if (LineCacheContains(hash)) return;
				
			var lineCache = new ConfigEditorLineCache<T>();
			lineCache.HashCode = hash;
			lineCache.RawData = lineData;
				
			foreach (var schema in CurrentClassFieldsCache)
			{
				var columnData = schema.field_info.GetValue(lineData);

				if (columnData == null)
				{
					Debug.Log($"Please Check {lineData.GetType().Name}'s Filed ,should be set a default value !!!");
					continue;
				}
					
				var column = new ConfigEditorLineFieldCache();
				column.RawData = columnData;
				column.CurrentSchema = schema;
				column.HashCode = columnData.GetHashCode();
				column.IsGenericType = columnData.GetType().IsGenericType;
				column.Add = columnData.GetType().GetMethod("Add");
				column.RemoveAt = columnData.GetType().GetMethod("RemoveAt");
				column.Count = columnData.GetType().GetProperty("Count");
				column.Item = columnData.GetType().GetProperty("Item");
				
				if (column.IsGenericType && schema.config_editor_setting.OutCodeGenEditorID == 0)
					column.AttributeType = columnData.GetType().GetGenericArguments()[0];
					
				lineCache.ColumnInfo.Add(column);
			}
			
			LinesCache.Add(lineCache);
		}

		public void SyncData()
		{
			LinesCache.Clear();
			for (var i = 0; i < config_current.ConfigList.Count; i++)
			{
				SyncData(config_current.ConfigList[i]);
			}
		}
	}
}
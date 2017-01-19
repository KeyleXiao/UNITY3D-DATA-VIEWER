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
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SmartDataViewerV1.Editor
{
	public class ConfigEditorSchemaChache
	{
		public FieldInfo field_info { get; set; }
		public ConfigEditorFieldAttribute config_editor_field { get; set; }
		public int order { get; set; }
	}


	/// TODO: 抽象T 增加非继承自Model的类型
	public class ConfigEditorSchema<T> : EditorWindow where T : Model, new()
	{
		protected ConfigEditorAttribute configSetting { get; set; }

		/// TODO: 抽象 ConfigBase 增加对 NewstoneJson ， Protobuff的支持
		protected ConfigBase<T> config_current { get; set; }

		protected int Index { get; set; }
		protected Vector2 posv { get; set; }
		protected List<int> deleteList = new List<int>();
		protected string SearchResourceName { get; set; }
		protected int PageAmount = 50;
		protected int PageIndex = 0;
		protected bool initialized { get; set; }
		protected List<FieldInfo> fieldinfos { get; set; }


		/// TODO: 抽象Chache作为一个接口
		protected List<ConfigEditorSchemaChache> Chache { get; set; }

		public void SetConfigType(ConfigBase<T> tp)
		{
			this.config_current = tp;

			configSetting = ConfigEditorAttribute.GetCurrentAttribute<ConfigEditorAttribute>(tp);

			this.titleContent = new GUIContent(configSetting.EditorTitle);

			fieldinfos = typeof(T).GetFields().ToList();

			Chache = new List<ConfigEditorSchemaChache>();

			foreach (var item in fieldinfos)
			{
				var infos = item.GetCustomAttributes(typeof(ConfigEditorFieldAttribute), true);
				ConfigEditorSchemaChache f = new ConfigEditorSchemaChache();
				f.field_info = item;
				f.order = 0;

				if (infos.Length != 0)
				{
					ConfigEditorFieldAttribute cefa = (ConfigEditorFieldAttribute)infos[0];
					f.order = cefa.Order;
					f.config_editor_field = cefa;
				}
				Chache.Add(f);
			}
			if (Chache.Count > 0)
			{
				Chache = Chache.OrderByDescending(x => x.order).ToList();
			}

			initialized = true;
		}

		public virtual void Initialize() { }

		public virtual T AddValue()
		{
			T t = new T();
			t.ID = (config_current.ConfigList.Count == 0) ? 1 : (config_current.ConfigList.Max(i => i.ID) + 1);
			t.NickName = string.Empty;
			return t;
		}


		/// TODO: 这里需要重构 将类型 与 控件的关系抽象
		public virtual void RenderRawLine(ConfigEditorSchemaChache data, object value, T raw)
		{
			if (data.config_editor_field == null)
			{
				EditorGUILayout.LabelField(((bool)value).ToString(), new GUILayoutOption[] { GUILayout.Width(80) });
				return;
			}

			if (value == null)
			{
				Debug.LogError("you must impleted the default construct class : " + raw.GetType().Name);
				return;
			}

			if (value is Enum)
			{
				if (data.config_editor_field.CanEditor)
				{
					value = EditorGUILayout.EnumPopup(value as Enum, new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
					data.field_info.SetValue(raw, value);
				}
				else
				{
					EditorGUILayout.LabelField((value as Enum).ToString(), new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
				}
			}
			if (value is string)
			{
				if (data.config_editor_field.CanEditor)
				{
					value = EditorGUILayout.TextField(value as string, new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
					data.field_info.SetValue(raw, value);
				}
				else
				{
					EditorGUILayout.LabelField(value as string, new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
				}
			}
			if (value is float)
			{
				if (data.config_editor_field.CanEditor)
				{
					value = EditorGUILayout.FloatField((float)value, new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
					data.field_info.SetValue(raw, value);
				}
				else
				{
					EditorGUILayout.LabelField(value as string, new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
				}
			}
			if (value is int)
			{
				if (data.config_editor_field.CanEditor)
				{
					value = EditorGUILayout.IntField((int)value, new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
					data.field_info.SetValue(raw, value);
				}
				else
				{
					EditorGUILayout.LabelField(value as string, new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
				}
			}
			if (value is bool)
			{
				if (data.config_editor_field.CanEditor)
				{
					value = EditorGUILayout.Toggle((bool)value, new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
					data.field_info.SetValue(raw, value);
				}
				else
				{
					EditorGUILayout.LabelField(((bool)value).ToString(), new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });
				}
			}
		}

		public void OnGUI()
		{
			if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
			{
				Close();
				return;
			}

			if (!initialized)
			{
				Initialize();
			}

			if (config_current == null)
			{
				config_current = ConfigBase<T>.LoadConfig<ConfigBase<T>>(configSetting.LoadPath);

				if (config_current == null)
					config_current = new ConfigBase<T>();
			}

			EditorGUILayout.Space();
			GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
			if (GUILayout.Button("Refresh", GUI.skin.GetStyle("ButtonLeft"), new GUILayoutOption[] { GUILayout.Height(30) }))
			{
				config_current = ConfigBase<T>.LoadConfig<ConfigBase<T>>(configSetting.LoadPath);
				deleteList.Clear();
			}

			if (GUILayout.Button("New Line", GUI.skin.GetStyle("ButtonMid"), new GUILayoutOption[] { GUILayout.Height(30) }))
			{
				config_current.ConfigList.Add(AddValue());
				Debug.Log(config_current.ConfigList.Count);
			}

			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("Save", GUI.skin.GetStyle("ButtonRight"), new GUILayoutOption[] { GUILayout.Height(30) }))
			{
				for (int i = 0; i < deleteList.Count; i++)
				{
					config_current.Delete(deleteList[i]);
				}
				config_current.SaveToDisk(configSetting.OutputPath);
				AssetDatabase.Refresh();
				ShowNotification(new GUIContent("SUCCESS !!!"));
			}
			GUI.backgroundColor = Color.white;
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(EditorConfig.NickName, GUILayout.Width(100));
			SearchResourceName = EditorGUILayout.TextField(SearchResourceName, GUI.skin.GetStyle("ToolbarSeachTextField"));
			GUILayout.EndHorizontal();

			GUILayout.BeginScrollView(posv, false, false, GUIStyle.none, GUIStyle.none, new GUILayoutOption[] { GUILayout.Height(45) });
			GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
			GUILayout.Space(20);


			foreach (var item in Chache)
			{
				if (item.config_editor_field == null)
					EditorGUILayout.LabelField(new GUIContent(item.field_info.Name), GUILayout.Width(80));
				else
					EditorGUILayout.LabelField(new GUIContent(item.config_editor_field.Display == "" ? item.field_info.Name : item.config_editor_field.Display), GUILayout.Width(item.config_editor_field.Width));

				GUILayout.Space(20);
			}
			EditorGUILayout.LabelField(new GUIContent(EditorConfig.Delete), GUILayout.Width(80));


			GUILayout.EndHorizontal();
			GUILayout.EndScrollView();


			GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
			posv = GUILayout.BeginScrollView(posv, false, false);
			GUILayout.BeginVertical();

			List<T> Finallylist = null;

			int ItemMaxCount = 0;
			if (!string.IsNullOrEmpty(SearchResourceName))
			{
				ItemMaxCount = config_current.ConfigList.Count(x => x.NickName.ToLower().Contains(SearchResourceName.ToLower().Trim()));
				Finallylist = config_current.ConfigList.Where(x => x.NickName.ToLower().Contains(SearchResourceName.ToLower().Trim())).Skip(PageIndex * PageAmount).Take(PageAmount).ToList();
			}
			else {
				ItemMaxCount = config_current.ConfigList.Count;
				Finallylist = config_current.ConfigList.Skip(PageIndex * PageAmount).Take(PageAmount).ToList();
			}

			foreach (var item in Finallylist)
			{
				if (deleteList.Contains(item.ID))
				{
					continue;
				}
				GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));

				foreach (var schema in Chache)
				{
					var rawData = schema.field_info.GetValue(item);

					RenderRawLine(schema, rawData, item);
					GUILayout.Space(20);
				}

				if (GUILayout.Button("Delete", new GUILayoutOption[] { GUILayout.Width(80) }))
				{
					deleteList.Add(item.ID);
					ShowNotification(new GUIContent("SUCCESS !!!"));
				}
				GUILayout.Space(20);
				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
			GUILayout.EndScrollView();
			GUILayout.EndHorizontal();




			GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));
			int maxIndex = Mathf.FloorToInt((ItemMaxCount - 1) / (float)PageAmount);
			if (maxIndex < PageIndex)
				PageIndex = 0;

			GUILayout.Label(string.Format(EditorConfig.PageInfoFormate, PageIndex + 1, maxIndex + 1), GUILayout.Width(80));
			GUILayout.Label(EditorConfig.OnePageMaxNumber, GUILayout.Width(80));
			int.TryParse(GUILayout.TextField(PageAmount.ToString(), GUILayout.Width(80)), out PageAmount);

			if (GUILayout.Button("<<<", GUI.skin.GetStyle("ButtonLeft"), GUILayout.Height(16)))
			{
				if (PageIndex - 1 < 0)
				{
					PageIndex = 0;
				}
				else {
					PageIndex -= 1;
				}
			}
			if (GUILayout.Button(">>>", GUI.skin.GetStyle("ButtonRight"), GUILayout.Height(16)))
			{
				if (PageIndex + 1 > maxIndex)
				{
					PageIndex = maxIndex;
				}
				else {
					PageIndex++;
				}
			}
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			GUILayout.Label(EditorConfig.Contract);
			GUILayout.EndHorizontal();
		}
	}
}
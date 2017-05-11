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

namespace SmartDataViewer.Editor
{
	public class ConfigEditorSchemaChache
	{
		public FieldInfo field_info { get; set; }
		public ConfigEditorFieldAttribute config_editor_field { get; set; }
		public int order { get; set; }
	}

	public class ConfigEditorSchema<T> : EditorWindow where T : IModel, new()
	{
		protected ConfigEditorAttribute configSetting { get; set; }
		protected ConfigBase<T> config_current { get; set; }
		protected int Index { get; set; }
		protected Vector2 posv { get; set; }
		protected List<int> deleteList = new List<int>();
		protected string SearchResourceName { get; set; }
		protected int PageAmount = 50;
		protected int PageIndex = 0;
		protected bool initialized { get; set; }
		protected List<FieldInfo> fieldinfos { get; set; }
		protected List<ConfigEditorSchemaChache> Chache { get; set; }
		protected bool FirstLoadFlag { get; set; }

		protected List<T> Finallylist { get; set; }
		protected int ItemMaxCount { get; set; }

		public void SetConfigType(ConfigBase<T> tp)
		{
			this.config_current = tp;

			configSetting = ConfigEditorAttribute.GetCurrentAttribute<ConfigEditorAttribute>(tp);

			this.titleContent = new GUIContent(string.IsNullOrEmpty(configSetting.EditorTitle) ? typeof(T).Name : configSetting.EditorTitle);

			fieldinfos = typeof(T).GetFields().ToList();

			Chache = new List<ConfigEditorSchemaChache>();

			foreach (var item in fieldinfos)
			{
				var infos = item.GetCustomAttributes(typeof(ConfigEditorFieldAttribute), true);
				ConfigEditorSchemaChache f = new ConfigEditorSchemaChache();
				f.field_info = item;
				f.order = 0;

				if (infos.Length == 0)
					continue;
				else
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

		public virtual T CreateValue()
		{
			T t = new T();
			t.ID = (config_current.ConfigList.Count == 0) ? 1 : (config_current.ConfigList.Max(i => i.ID) + 1);
			t.NickName = string.Empty;

			Debug.Log(t.ID);
			return t;
		}

		public virtual void RenderRawLine(ConfigEditorSchemaChache data, object value, T raw)
		{

		}

		protected virtual void Reload()
		{
			config_current = ConfigBase<T>.LoadConfig<ConfigBase<T>>(configSetting.LoadPath);
			Debug.Log(configSetting.LoadPath);
			if (config_current == null)
				config_current = new ConfigBase<T>();
			deleteList.Clear();
		}

		protected virtual void SaveConfig()
		{
			for (int i = 0; i < deleteList.Count; i++)
			{
				config_current.Delete(deleteList[i]);
			}
			config_current.SaveToDisk(configSetting.OutputPath);
			AssetDatabase.Refresh();
			ShowNotification(new GUIContent("SUCCESS !!!"));
		}

		protected virtual void SaveButton()
		{
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("Save", GUI.skin.GetStyle("ButtonRight"), new GUILayoutOption[] { GUILayout.Height(30) }))
				SaveConfig();
			GUI.backgroundColor = Color.white;
		}

		protected virtual void SearchField()
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(EditorConfig.NickName, GUILayout.Width(100));
			SearchResourceName = EditorGUILayout.TextField(SearchResourceName, GUI.skin.GetStyle("ToolbarSeachTextField"));
			GUILayout.EndHorizontal();
		}
		protected virtual void NewLineButton()
		{
			if (GUILayout.Button("New Line", GUI.skin.GetStyle("ButtonMid"), new GUILayoutOption[] { GUILayout.Height(30) }))
				config_current.ConfigList.Add(CreateValue());
		}

		public void OnGUI()
		{
			if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
			{
				Close();
				return;
			}

			if (!initialized)
				Initialize();

			if (!FirstLoadFlag)
			{
				FirstLoadFlag = true;
				Reload();
			}



			EditorGUILayout.Space();
			GUILayout.BeginHorizontal(GUI.skin.GetStyle("GroupBox"));

			if (GUILayout.Button("Refresh", GUI.skin.GetStyle("ButtonLeft"), new GUILayoutOption[] { GUILayout.Height(30) }))
				Reload();

			if (!configSetting.DisableCreate)
				NewLineButton();

			if (!configSetting.DisableSave)
				SaveButton();

			GUILayout.EndHorizontal();


			if (!configSetting.DisableSearch)
				SearchField();


			GUILayout.BeginScrollView(new Vector2(posv.x, 0), false, false, GUIStyle.none, GUIStyle.none, new GUILayoutOption[] { GUILayout.Height(45) });
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
			posv = GUILayout.BeginScrollView(posv, true, false, GUI.skin.GetStyle("horizontalScrollbar"), GUIStyle.none, GUI.skin.GetStyle("GroupBox"));
			GUILayout.BeginVertical();



			if (!string.IsNullOrEmpty(SearchResourceName))
			{
				ItemMaxCount = config_current.ConfigList.Count(x => x.NickName.ToLower().Contains(SearchResourceName.ToLower().Trim()));
				Finallylist = config_current.ConfigList.Where(x => x.NickName.ToLower().Contains(SearchResourceName.ToLower().Trim())).Skip(PageIndex * PageAmount).Take(PageAmount).ToList();
			}
			else
			{
				ItemMaxCount = config_current.ConfigList.Count;
				Finallylist = config_current.ConfigList.Skip(PageIndex * PageAmount).Take(PageAmount).ToList();
			}

			foreach (var item in Finallylist)
			{
				if (deleteList.Contains(item.ID))
					continue;

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

			Page();

			GUILayout.BeginHorizontal();
			GUILayout.Label(EditorConfig.Contract);
			GUILayout.EndHorizontal();
		}
		protected virtual void Page()
		{
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
				else
				{
					PageIndex -= 1;
				}
			}
			if (GUILayout.Button(">>>", GUI.skin.GetStyle("ButtonRight"), GUILayout.Height(16)))
			{
				if (PageIndex + 1 > maxIndex)
				{
					PageIndex = maxIndex;
				}
				else
				{
					PageIndex++;
				}
			}
			GUILayout.EndHorizontal();
		}
	}
}
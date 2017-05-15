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

	public class ConfigEditorSchema<T> : IMultipleWindow where T : IModel, new()
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

		protected Dictionary<string, ConfigBase<IModel>> outLinkChache { get; set; }


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
			if (data.config_editor_field == null)
			{
				//EditorGUILayout.LabelField(((bool)value).ToString(), new GUILayoutOption[] { GUILayout.Width(80) });
				EditorGUILayout.LabelField("", new GUILayoutOption[] { GUILayout.Width(80) });
				return;
			}

			if (value.GetType().IsGenericType)
			{
				GUILayout.BeginVertical(GUIStyle.none, new GUILayoutOption[] { GUILayout.Width(data.config_editor_field.Width) });

				int deleteIndex = -1;

				//Open Editor
				if (!string.IsNullOrEmpty(data.config_editor_field.OutLinkEditor))
				{


					if (GUILayout.Button(EditorConfig.Add))
					{
						Assembly assembly = Assembly.GetExecutingAssembly();
						IMultipleWindow e = assembly.CreateInstance(data.config_editor_field.OutLinkEditor) as IMultipleWindow;
						if (e == null)
							ShowNotification(new GUIContent(EditorConfig.OutLinkIsNull));
						else
						{
							e.UpdateSelectModel(value, SetListItemValue);
							e.ShowUtility();
						}
					}

					var temp = value as List<int>;

					for (int i = 0; i < temp.Count; i++)
					{
						GUILayout.BeginHorizontal();

						if (!outLinkChache.ContainsKey(data.config_editor_field.OutLinkClass))
						{
							GUILayout.Label("NickName");
						}
						else
						{
							var info = outLinkChache[data.config_editor_field.OutLinkClass].SearchByID(temp[i]);
							if (info != null)
							{
								GUILayout.Label(info.NickName);
							}
							else { GUILayout.Label("NickName"); }
						}

						if (GUILayout.Button("X", GUILayout.Width(18)))
							deleteIndex = i;

						GUILayout.EndHorizontal();
					}
					if (deleteIndex != -1)
					{
						temp.RemoveAt(deleteIndex);
					}
				}
				else
				{
					Type t = value.GetType().GetGenericArguments()[0];

					if (value == null)
					{
						value = Activator.CreateInstance(t);
						data.field_info.SetValue(raw, value);
					}

					var addMethod = value.GetType().GetMethod("Add");
					var removeMethod = value.GetType().GetMethod("RemoveAt");


					if (GUILayout.Button(EditorConfig.Add))
					{
						addMethod.Invoke(value, new object[] { Activator.CreateInstance(t) });
					}


					int count = Convert.ToInt32(value.GetType().GetProperty("Count").GetValue(value, null));

					int removeIndex = -1;

					for (int i = 0; i < count; i++)
					{
						object listItem = value.GetType().GetProperty("Item").GetValue(value, new object[] { i });


						GUILayout.BeginHorizontal();
						RenderBaseControl(data.config_editor_field.Width - 18, data.config_editor_field.CanEditor, listItem, v =>
						{
							value.GetType().GetProperty("Item").SetValue(value, v, new object[] { i });
						});

						if (GUILayout.Button("X", new GUILayoutOption[] { GUILayout.Width(18) }))
						{
							removeIndex = i;
						}
						GUILayout.EndHorizontal();
					}

					if (removeIndex != -1)
					{
						removeMethod.Invoke(value, new object[] { removeIndex });
					}

				}
				GUILayout.EndVertical();

			}
			else
			{
				RenderBaseControl(data.config_editor_field.Width, data.config_editor_field.CanEditor, value, v => { data.field_info.SetValue(raw, v); });
			}

		}
		public virtual void RenderBaseControl(int width, bool enable, object value, Action<object> setValue)
		{
			if (value is Enum)
			{
				if (enable)
				{
					value = EditorGUILayout.EnumPopup(value as Enum, new GUILayoutOption[] { GUILayout.Width(width) });
					setValue(value);
				}
				else
				{
					EditorGUILayout.LabelField((value as Enum).ToString(), new GUILayoutOption[] { GUILayout.Width(width) });
				}
			}
			else if (value is string)
			{
				if (enable)
				{
					value = EditorGUILayout.TextField(value as string, new GUILayoutOption[] { GUILayout.Width(width) });
					setValue(value);
				}
				else
				{
					EditorGUILayout.LabelField(value as string, new GUILayoutOption[] { GUILayout.Width(width) });
				}
			}
			else if (value is float)
			{
				if (enable)
				{
					value = EditorGUILayout.FloatField((float)value, new GUILayoutOption[] { GUILayout.Width(width) });
					setValue(value);
				}
				else
				{
					EditorGUILayout.LabelField(value as string, new GUILayoutOption[] { GUILayout.Width(width) });
				}
			}
			else if (value is int)
			{
				if (enable)
				{
					value = EditorGUILayout.IntField((int)value, new GUILayoutOption[] { GUILayout.Width(width) });
					setValue(value);
				}
				else
				{
					EditorGUILayout.LabelField(value as string, new GUILayoutOption[] { GUILayout.Width(width) });
				}
			}
			else if (value is bool)
			{
				if (enable)
				{
					value = EditorGUILayout.Toggle((bool)value, new GUILayoutOption[] { GUILayout.Width(width) });
					setValue(value);
				}
				else
				{
					EditorGUILayout.LabelField(((bool)value).ToString(), new GUILayoutOption[] { GUILayout.Width(width) });
				}
			}
			else if (value is Vector2)
			{
				if (enable)
				{
					value = EditorGUILayout.Vector2Field("", (Vector2)value, new GUILayoutOption[] { GUILayout.Width(width) });
					setValue(value);
				}
				else
				{
					EditorGUILayout.Vector2Field("", (Vector2)value, new GUILayoutOption[] { GUILayout.Width(width) });
				}
			}
			else if (value is Vector3)
			{
				if (enable)
				{
					value = EditorGUILayout.Vector3Field("", (Vector3)value, new GUILayoutOption[] { GUILayout.Width(width) });
					setValue(value);
				}
				else
				{
					EditorGUILayout.Vector3Field("", (Vector3)value, new GUILayoutOption[] { GUILayout.Width(width) });
				}
			}
			else if (value is Vector4)
			{
				if (enable)
				{
					value = EditorGUILayout.Vector3Field("", (Vector4)value, new GUILayoutOption[] { GUILayout.Width(width) });
					setValue(value);
				}
				else
				{
					EditorGUILayout.Vector3Field("", (Vector4)value, new GUILayoutOption[] { GUILayout.Width(width) });
				}
			}
		}

		public virtual void SetListItemValue(object item, object addValue)
		{
			var temp = item as List<int>;
			var model = addValue as IModel;
			temp.Add(model.ID);
		}

		protected virtual void Reload()
		{
			config_current = ConfigBase<T>.LoadConfig<ConfigBase<T>>(configSetting.LoadPath);

			if (config_current == null)
				config_current = new ConfigBase<T>();
			deleteList.Clear();

			ReloadOutLinkChache();
		}

		protected virtual void ReloadOutLinkChache()
		{
			outLinkChache = new Dictionary<string, ConfigBase<IModel>>();

			for (int i = 0; i < Chache.Count; i++)
			{
				if (Chache[i].config_editor_field == null)
					continue;

				if (!string.IsNullOrEmpty(Chache[i].config_editor_field.OutLinkEditor) && !string.IsNullOrEmpty(Chache[i].config_editor_field.OutLinkClass))
				{
					ConfigBase<IModel> model = ConfigBase<IModel>.LoadConfig<ConfigBase<IModel>>(Chache[i].config_editor_field.OutLinkClass);

					if (model != null)
					{
						outLinkChache.Add(Chache[i].config_editor_field.OutLinkClass, model);
					}
				}
			}
		}

		protected virtual void SaveConfig()
		{
			for (int i = 0; i < deleteList.Count; i++)
			{
				config_current.Delete(deleteList[i]);
			}
			config_current.SaveToDisk(configSetting.OutputPath);
			AssetDatabase.Refresh();
			ShowNotification(new GUIContent(EditorConfig.Success));
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

			if (!configSetting.DisableCreate && current_windowType != WindowType.CALLBACK)
				NewLineButton();

			if (!configSetting.DisableSave && current_windowType != WindowType.CALLBACK)
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

			if (current_windowType != WindowType.CALLBACK)
				EditorGUILayout.LabelField(new GUIContent(EditorConfig.Delete), GUILayout.Width(80));
			if (current_windowType == WindowType.CALLBACK)
				EditorGUILayout.LabelField(new GUIContent(EditorConfig.Select), GUILayout.Width(80));


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

				if (current_windowType != WindowType.CALLBACK)
				{
					if (GUILayout.Button(EditorConfig.Delete, new GUILayoutOption[] { GUILayout.Width(80) }))
					{
						deleteList.Add(item.ID);
						ShowNotification(new GUIContent(EditorConfig.Success));
					}
				}

				if (current_windowType == WindowType.CALLBACK)
				{
					if (GUILayout.Button(EditorConfig.Select, new GUILayoutOption[] { GUILayout.Width(80) }))
					{
						select_callback(current_list, item);
						Close();
					}
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

			if (GUILayout.Button(EditorConfig.Previous, GUI.skin.GetStyle("ButtonLeft"), GUILayout.Height(16)))
			{
				if (PageIndex - 1 < 0)
					PageIndex = 0;
				else
					PageIndex -= 1;
			}
			if (GUILayout.Button(EditorConfig.Next, GUI.skin.GetStyle("ButtonRight"), GUILayout.Height(16)))
			{
				if (PageIndex + 1 > maxIndex)
					PageIndex = maxIndex;
				else
					PageIndex++;
			}
			GUILayout.EndHorizontal();
		}

		public override void UpdateSelectModel(object current_list, Action<object, object> callback)
		{
			current_windowType = WindowType.CALLBACK;
			select_callback = callback;
			this.current_list = current_list;
		}
	}
}
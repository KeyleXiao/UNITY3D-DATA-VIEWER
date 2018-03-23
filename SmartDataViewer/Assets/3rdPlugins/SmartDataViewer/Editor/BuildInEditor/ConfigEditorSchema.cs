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

//TODO bug fix: 删除文件之后加载CodeGen直接报错 然而直接加载detail编辑 再切回来就正常了 主要是由于生成脚本编译引起的 现在CodeGen之后直接把界面关闭

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using SmartDataViewer.Helpers;

namespace SmartDataViewer.Editor
{
    /// <summary>
    /// 存储反射字段信息的容器
    /// </summary>
    public class ConfigEditorSchemaChache
    {
        public FieldInfo field_info { get; set; }

        //public ConfigEditorFieldAttribute config_editor_field { get; set; }
        public DefaultControlPropertity config_editor_setting { get; set; }
        public int order { get; set; }
    }

    /// <summary>
    /// 泛化数据编辑器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigEditorSchema<T> : IMultipleWindow where T : IModel, new()
    {
        /// <summary>
        /// 测试工具
        /// </summary>
        protected bool isLog
        {
            get { return false; }
        }

        /// <summary>
        /// 设置列间间隔
        /// </summary>
        protected int ColumnSpan = 3;

        /// <summary>
        /// 表头高度
        /// </summary>
        protected int TableHeadHeight = 26;
        
        /// <summary>
        /// 缩放
        /// </summary>
        protected float Resize = 1;

        /// <summary>
        /// 编辑器通用配置
        /// </summary>
        protected ConfigEditorAttribute configSetting { get; set; }
        
        /// <summary>
        /// 当前编辑对象
        /// </summary>
        protected ConfigBase<T> config_current { get; set; }
        
        /// <summary>
        /// 纵向拖动
        /// </summary>
        protected Vector2 posv { get; set; }
        
        /// <summary>
        /// 待删除列表
        /// </summary>
        protected List<int> deleteList = new List<int>();
        
        /// <summary>
        /// 搜索字段
        /// </summary>
        protected string SearchResourceName { get; set; }
        
        /// <summary>
        /// 默认一页数量
        /// </summary>
        protected int PageAmount = 50;
        
        /// <summary>
        /// 当前页
        /// </summary>
        protected int PageIndex = 0;
        
        /// <summary>
        /// 是否初始化完毕
        /// </summary>
        protected bool initialized { get; set; }
        
        /// <summary>
        /// 缓存反射字段
        /// </summary>
        protected List<FieldInfo> fieldinfos { get; set; }
        
        /// <summary>
        /// 缓存反射字段信息
        /// </summary>
        protected List<ConfigEditorSchemaChache> Chache { get; set; }
        
        /// <summary>
        /// 首次加载标示
        /// </summary>
        protected bool FirstLoadFlag { get; set; }

        /// <summary>
        /// 搜索结果
        /// </summary>
        protected List<T> Finallylist { get; set; }
        
        /// <summary>
        /// 页上限
        /// </summary>
        protected int ItemMaxCount { get; set; }
        
        /// <summary>
        /// 字段排序
        /// </summary>
        protected Dictionary<string, bool> FieldsOrder { get; set; }

      

        
        //TODO 2.0 外联表原始数据
        /// <summary>
        /// 外链表数据
        /// </summary>
        protected Dictionary<string, object> outLinkRawData { get; set; }


        /// <summary>
        /// 选择模式下 选中的列ID
        /// </summary>
        public List<int> SelctList { get; set; }


        /// <summary>
        /// 粘贴行数据
        /// </summary>
        protected T PasteItem { get; set; }


        /// <summary>
        /// 设置编辑器字段排序规则
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual bool GetFieldsOrder(string key)
        {
            if (!FieldsOrder.ContainsKey(key))
            {
                FieldsOrder.Add(key, true);
                return true;
            }

            FieldsOrder[key] = !FieldsOrder[key];
            return FieldsOrder[key];
        }


        /// <summary>
        /// 设置想要编辑的类
        /// </summary>
        /// <param name="tp"></param>
        public void SetConfigType(ConfigBase<T> tp)
        {
            config_current = tp;

            configSetting = ConfigEditorAttribute.GetCurrentAttribute<ConfigEditorAttribute>(tp) ??
                            new ConfigEditorAttribute();

            titleContent = new GUIContent(string.IsNullOrEmpty(configSetting.Setting.EditorTitle)
                ? typeof(T).Name
                : configSetting.Setting.EditorTitle);

            fieldinfos = typeof(T).GetFields().ToList();

            Chache = new List<ConfigEditorSchemaChache>();

            foreach (var item in fieldinfos)
            {
                var infos = item.GetCustomAttributes(typeof(ConfigEditorFieldAttribute), true);
                ConfigEditorSchemaChache f = new ConfigEditorSchemaChache();
                f.field_info = item;
                f.order = 0;

                if (infos.Length == 0)
                {
                    int id = (int) ReflectionHelper.GetCurrentFieldType(item.FieldType);
                    f.config_editor_setting = EditorConfig.GetDefaultControlConfig().SearchByID(id);


                    if (f.config_editor_setting == null)
                    {
                        Log("Can't find default control id :" + id);
                    }
                    else
                    {
                        if (!f.config_editor_setting.Visibility)
                            continue;
                    }
                }
                else
                {
                    ConfigEditorFieldAttribute cefa = (ConfigEditorFieldAttribute) infos[0];
                    f.order = cefa.Setting.Order;
                    f.config_editor_setting = cefa.Setting;


                    if (f.config_editor_setting.Width == 0)
                    {
                        int id = (int) ReflectionHelper.GetCurrentFieldType(item.FieldType);
                        var setting = EditorConfig.GetDefaultControlConfig().SearchByID(id);
                        f.config_editor_setting.Width = setting.Width;
                    }

                    if (!cefa.Setting.Visibility)
                        continue;
                }

                Chache.Add(f);
            }

            if (Chache.Count > 0)
            {
                Chache = Chache.OrderByDescending(x => x.order).ToList();
            }

            initialized = true;
        }


        /// <summary>
        /// 初始化构造
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 创建一条新数据
        /// </summary>
        /// <returns></returns>
        public virtual T CreateValue()
        {
            T t = new T();
            t.ID = (config_current.ConfigList.Count == 0) ? 1 : (config_current.ConfigList.Max(i => i.ID) + 1);
            t.NickName = string.Empty;

            Log(t.ID.ToString());
            return t;
        }


        /// <summary>
        /// 反射原始数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <param name="raw"></param>
        public virtual void RenderRawLine(ConfigEditorSchemaChache data, object value, T raw)
        {
            if (value == null) return;

            if (value.GetType().IsGenericType)
            {
                GUILayout.BeginVertical(GUIStyle.none,
                    new GUILayoutOption[] {GUILayout.Width(data.config_editor_setting.Width * Resize)});

                int deleteIndex = -1;

                //Open Editor
                if (!string.IsNullOrEmpty(data.config_editor_setting.OutLinkEditor))
                {
                    if (GUILayout.Button(Language.Add))
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        IMultipleWindow e =
                            assembly.CreateInstance(data.config_editor_setting.OutLinkEditor) as IMultipleWindow;
                        if (e == null)
                            ShowNotification(new GUIContent(Language.OutLinkIsNull));
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
                        GUILayout.Label(GetOutLinkDisplayField(temp[i], data.config_editor_setting.OutLinkClass,
                            data.config_editor_setting.OutLinkDisplay));
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

                    var addMethod = value.GetType().GetMethod("Add");
                    var removeMethod = value.GetType().GetMethod("RemoveAt");

                    if (GUILayout.Button(Language.Add))
                    {
                        addMethod.Invoke(value,
                            new object[] {t == typeof(string) ? string.Empty : Activator.CreateInstance(t)});
                    }


                    int count = Convert.ToInt32(value.GetType().GetProperty("Count").GetValue(value, null));

                    int removeIndex = -1;

                    for (int i = 0; i < count; i++)
                    {
                        object listItem = value.GetType().GetProperty("Item").GetValue(value, new object[] {i});


                        GUILayout.BeginHorizontal();
                        //alignment
                        RenderBaseControl(data.config_editor_setting.Width - 22, data.config_editor_setting.CanEditor,
                            listItem,
                            v => { value.GetType().GetProperty("Item").SetValue(value, v, new object[] {i}); });

                        if (GUILayout.Button("X", new GUILayoutOption[] {GUILayout.Width(18)}))
                        {
                            removeIndex = i;
                        }

                        GUILayout.EndHorizontal();
                    }

                    if (removeIndex != -1)
                    {
                        removeMethod.Invoke(value, new object[] {removeIndex});
                    }
                }

                GUILayout.EndVertical();
            }
            else
            {
                //Open Editor
                if (!string.IsNullOrEmpty(data.config_editor_setting.OutLinkEditor))
                {
                    data.field_info.SetValue(raw,
                        GetSingleSelectValueByFlag(raw.ID, data.field_info.Name, (int) value));
                    string buttonName = GetOutLinkDisplayField((int) value, data.config_editor_setting.OutLinkClass,
                        data.config_editor_setting.OutLinkDisplay);

                    if (GUILayout.Button(buttonName,
                        new GUILayoutOption[] {GUILayout.Width(data.config_editor_setting.Width * Resize)}))
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        IMultipleWindow e =
                            assembly.CreateInstance(data.config_editor_setting.OutLinkEditor) as IMultipleWindow;
                        if (e == null)
                            ShowNotification(new GUIContent(Language.OutLinkIsNull));
                        else
                        {
                            AddSingleSelectFlag(raw.ID, data.field_info.Name);
                            e.UpdateSelectModel(SingleTempSelect, SetListItemValue);
                            e.ShowUtility();
                        }
                    }
                }
                else
                {
                    RenderBaseControl(data.config_editor_setting.Width, data.config_editor_setting.CanEditor, value,
                        v => { data.field_info.SetValue(raw, v); });
                }
            }
        }


        /// <summary>
        /// 使用isLog控制输出
        /// </summary>
        /// <param name="str"></param>
        public virtual void Log(string str)
        {
            if (isLog) Debug.Log(str);
        }


        /// <summary>
        /// 获取外链编辑器的按钮上显示的字符，未指定则显示ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="outLinkChacheKey"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string GetOutLinkDisplayField(int id, string outLinkChacheKey, string field)
        {
            if (!outLinkRawData.ContainsKey(outLinkChacheKey))
                return string.Empty;

            if (string.IsNullOrEmpty(field)) field = "NickName";

            object rawData = outLinkRawData[outLinkChacheKey];

            var searchMethod = rawData.GetType().GetMethod("SearchByID");

            var subData = searchMethod.Invoke(rawData, new object[] {id});

            Log(string.Format("is Null ={0}  Field ={1}  id ={2}", subData == null, field, id.ToString()));

            if (subData == null) return Language.Select;

            var tempDisplayObj = subData.GetType().GetField(field).GetValue(subData);

            if (tempDisplayObj == null || string.IsNullOrEmpty(tempDisplayObj.ToString()))
            {
                field = "ID";
                tempDisplayObj = subData.GetType().GetField(field).GetValue(subData);
            }

            string v = tempDisplayObj.ToString();

            if (string.IsNullOrEmpty(v)) return Language.Select;

            return v;
        }


        /// <summary>
        /// 设置基础的控件
        /// </summary>
        /// <param name="width"></param>
        /// <param name="enable"></param>
        /// <param name="value"></param>
        /// <param name="setValue"></param>
        public virtual void RenderBaseControl(int width, bool enable, object value, Action<object> setValue)
        {
            if (value is Enum)
            {
                if (enable)
                {
                    value = EditorGUILayout.EnumPopup(value as Enum, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField((value as Enum).ToString(),
                        new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is Bounds)
            {
                if (enable)
                {
                    value = EditorGUILayout.BoundsField((Bounds) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.BoundsField((Bounds) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is Color)
            {
                if (enable)
                {
                    value = EditorGUILayout.ColorField((Color) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.ColorField((Color) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is AnimationCurve)
            {
                if (enable)
                {
                    value = EditorGUILayout.CurveField((AnimationCurve) value,
                        new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.CurveField((AnimationCurve) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is string)
            {
                if (enable)
                {
                    value = EditorGUILayout.TextField(value as string, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is float)
            {
                if (enable)
                {
                    value = EditorGUILayout.FloatField((float) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is int)
            {
                if (enable)
                {
                    value = EditorGUILayout.IntField((int) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is bool)
            {
                if (enable)
                {
                    value = EditorGUILayout.Toggle((bool) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(((bool) value).ToString(),
                        new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is Vector2)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector2Field("", (Vector2) value,
                        new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector2Field("", (Vector2) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is Vector3)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector3Field("", (Vector3) value,
                        new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector3Field("", (Vector3) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
            else if (value is Vector4)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector4Field("", (Vector4) value,
                        new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector4Field("", (Vector4) value, new GUILayoutOption[] {GUILayout.Width(width*Resize)});
                }
            }
        }


        /// <summary>
        /// Reload 但不包括 SetConfigType 与 Initialize
        /// </summary>
        protected virtual void Reload()
        {
            AssetDatabase.Refresh();
            FieldsOrder = new Dictionary<string, bool>();
            config_current = ConfigBase<T>.LoadConfig<ConfigBase<T>>(configSetting.Setting.LoadPath);

            if (config_current == null)
                config_current = new ConfigBase<T>();
            deleteList.Clear();
            ReloadOutLinkChache();
        }


        /// <summary>
        /// 重新加载外链类缓存
        /// </summary>
        protected virtual void ReloadOutLinkChache()
        {
            if (outLinkRawData == null)
                outLinkRawData = new Dictionary<string, object>();
            else
                outLinkRawData.Clear();

            for (int i = 0; i < Chache.Count; i++)
            {
                //Bug fix: 自动做默认参数初始化 
                if (Chache[i].config_editor_setting == null)
                    Chache[i].config_editor_setting = new DefaultControlPropertity();

                if (string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkEditor))
                {
                    if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkClass))
                    {
                        Chache[i].config_editor_setting.OutLinkEditor =
                            Chache[i].config_editor_setting.OutLinkClass + "Editor";
                    }
                    else if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkSubClass))
                    {
                        Chache[i].config_editor_setting.OutLinkEditor =
                            Chache[i].config_editor_setting.OutLinkSubClass + "ConfigEditor";
                    }
                    else
                    {
                        Log("Out link info is null ...");
                        continue;
                    }
                }

                if (string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkClass))
                {
                    if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkSubClass))
                        Chache[i].config_editor_setting.OutLinkClass =
                            Chache[i].config_editor_setting.OutLinkSubClass + "Config";
                    else
                        Chache[i].config_editor_setting.OutLinkClass =
                            Chache[i].config_editor_setting.OutLinkEditor.Replace("Editor", "");
                }

                if (string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkFilePath))
                {
                    if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkSubClass))
                        Chache[i].config_editor_setting.OutLinkFilePath =
                            Chache[i].config_editor_setting.OutLinkSubClass;
                    else
                        Chache[i].config_editor_setting.OutLinkFilePath = Chache[i].config_editor_setting.OutLinkClass
                            .Replace("ConfigEditor", "");
                }


                //TODO VERSION 2.0 Load Raw Data
                if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkEditor) &&
                    !string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkClass) &&
                    !string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkFilePath)
                )
                {
                    string rawClass = Chache[i].config_editor_setting.OutLinkClass;
                    if (string.IsNullOrEmpty(rawClass))
                        rawClass = Chache[i].config_editor_setting.OutLinkSubClass + "Config";
                    Type classType = ReflectionHelper.GetCurrnetAssemblyType(rawClass);
                    if (classType == null)
                    {
                        Log("Can't find calss " + rawClass);
                        continue;
                    }

                    var modelRaw =
                        ConfigBase<IModel>.LoadRawConfig(classType, Chache[i].config_editor_setting.OutLinkFilePath);
                    if (modelRaw != null)
                    {
                        if (!outLinkRawData.ContainsKey(Chache[i].config_editor_setting.OutLinkClass))
                            outLinkRawData.Add(Chache[i].config_editor_setting.OutLinkClass, modelRaw);

                        Log(string.Format("Loading OutLink Class [{0}] Data",
                            Chache[i].config_editor_setting.OutLinkClass));
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

            deleteList.Clear();

            string outPutPath = configSetting.Setting.OutputPath;
            if (string.IsNullOrEmpty(outPutPath) && !string.IsNullOrEmpty(configSetting.Setting.LoadPath))
                outPutPath = configSetting.Setting.LoadPath;

            config_current.SaveToDisk(outPutPath);
            AssetDatabase.Refresh();
            ShowNotification(new GUIContent(Language.Success));
        }

        protected virtual void SaveButton()
        {
            if (GUILayout.Button("Save", GUI.skin.GetStyle("ButtonRight")))
                SaveConfig();
        }

        protected virtual void SearchField()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Language.NickName, GUILayout.Width(70));
            SearchResourceName =
                EditorGUILayout.TextField(SearchResourceName, GUI.skin.GetStyle("ToolbarSeachTextField"));
            GUILayout.EndHorizontal();
        }


        /// <summary>
        /// 创建新行
        /// </summary>
        protected virtual void NewLineButton()
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(Language.NewLine, GUI.skin.GetStyle("ButtonMid")))
                config_current.ConfigList.Add(CreateValue());
            GUI.backgroundColor = Color.white;
        }

        /// <summary>
        /// 页头按钮
        /// </summary>
        /// <param name="field_name"></param>
        protected virtual void HeadButton_Click(string field_name)
        {
            if (field_name == "ID")
            {
                if (GetFieldsOrder(field_name))
                    config_current.ConfigList = config_current.ConfigList.OrderBy(x => x.ID).ToList();
                else
                    config_current.ConfigList = config_current.ConfigList.OrderByDescending(x => x.ID).ToList();
            }

            if (field_name == "NickName")
            {
                if (GetFieldsOrder(field_name))
                    config_current.ConfigList = config_current.ConfigList.OrderBy(x => x.NickName).ToList();
                else
                    config_current.ConfigList = config_current.ConfigList.OrderByDescending(x => x.NickName).ToList();
            }
        }

        /// <summary>
        /// 处理表头
        /// </summary>
        protected virtual void RenderHead()
        {
            GUILayout.BeginScrollView(new Vector2(posv.x, 0), false, false, GUIStyle.none, GUIStyle.none,
                new GUILayoutOption[] {GUILayout.Height(TableHeadHeight)});

            GUILayout.BeginHorizontal(EditorGUIStyle.GetGroupBoxStyle(),
                new GUILayoutOption[] {GUILayout.Width(position.width)});

            //TODO Set Order
            foreach (var item in Chache)
            {
                if (GUILayout.Button(
                    string.IsNullOrEmpty(item.config_editor_setting.Display)
                        ? item.field_info.Name
                        : item.config_editor_setting.Display, EditorGUIStyle.GetTagButtonStyle(),
                    new GUILayoutOption[] {GUILayout.Width(item.config_editor_setting.Width * Resize)}))
                    HeadButton_Click(item.field_info.Name);

                GUILayout.Space(ColumnSpan);
            }

            RenderExtensionHead();

            if (current_windowType != WindowType.CALLBACK)
                EditorGUILayout.LabelField(new GUIContent(Language.Operation), GUILayout.Width(80));
            if (current_windowType == WindowType.CALLBACK)
                EditorGUILayout.LabelField(new GUIContent(Language.Select), GUILayout.Width(80));


            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }


        /// <summary>
        /// 用户可扩展表头
        /// </summary>
        protected virtual void RenderExtensionHead()
        {
        }

        /// <summary>
        /// 用户可扩展行按钮
        /// </summary>
        /// <param name="item">当前行的数据</param>
        protected virtual void RenderExtensionButton(T item)
        {
        }

        
        /// <summary>
        /// 处理表主体
        /// </summary>
        protected virtual void RenderTable()
        {
            posv = GUILayout.BeginScrollView(posv, false, false, EditorGUIStyle.GetHorizontalScrollbarStyle(),
                GUIStyle.none, EditorGUIStyle.GetTableGroupBoxStyle());

            if (!string.IsNullOrEmpty(SearchResourceName))
            {
                ItemMaxCount = config_current.ConfigList.Count(x =>
                    x.NickName.ToLower().Contains(SearchResourceName.ToLower().Trim()));
                Finallylist = config_current.ConfigList
                    .Where(x => x.NickName.ToLower().Contains(SearchResourceName.ToLower().Trim()))
                    .Skip(PageIndex * PageAmount).Take(PageAmount).ToList();
            }
            else
            {
                ItemMaxCount = config_current.ConfigList.Count;
                Finallylist = config_current.ConfigList.Skip(PageIndex * PageAmount).Take(PageAmount).ToList();
            }

            //遍历搜索结果
            for (int i = 0; i < Finallylist.Count; i++)
            {
                T item = Finallylist[i];

                if (deleteList.Contains(item.ID))
                    continue;

                //Select effect diaplay
                if (current_windowType == WindowType.CALLBACK && SelctList.Contains(item.ID))
                    GUI.backgroundColor = Color.green;
                else GUI.backgroundColor = Color.white;


                GUILayout.BeginHorizontal(EditorGUIStyle.GetGroupBoxStyle());

                foreach (var schema in Chache)
                {
                    var rawData = schema.field_info.GetValue(item);
                    //一条一条加载
                    RenderRawLine(schema, rawData, item);
                    GUILayout.Space(ColumnSpan);
                }

                RenderExtensionButton(item);
                GUILayout.Space(ColumnSpan);
                RenderFunctionButton(item);

                GUILayout.Space(ColumnSpan);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(15);
            GUILayout.EndScrollView();
        }


        /// <summary>
        /// 处理顶部按钮
        /// </summary>
        protected virtual void RenderMenu()
        {
            GUILayout.BeginHorizontal(GUIStyle.none, new GUILayoutOption[] {GUILayout.Height(25)});

            if (GUILayout.Button(Language.DiscardChange, GUI.skin.GetStyle("ButtonLeft")))
                Reload();


            if (!configSetting.Setting.DisableCreate && current_windowType != WindowType.CALLBACK)
                NewLineButton();

            if (!configSetting.Setting.DisableSave && current_windowType != WindowType.CALLBACK)
                SaveButton();

            GUILayout.EndHorizontal();


            if (!configSetting.Setting.DisableSearch)
                SearchField();
        }

        /// <summary>
        /// 主逻辑
        /// </summary>
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
                if (current_windowType == WindowType.CALLBACK) SelctList = new List<int>(); //init select display
            }


            if (!FirstLoadFlag)
            {
                FirstLoadFlag = true;
                Reload();
            }

            //TODO: 有一种情况 生成需要unity编译的脚本 这时候打开编辑器会报错(存留在内存的数据被clear) 这时候需要先关闭编辑器
            if (configSetting == null)
            {
                Close();
                return;
            }

            EditorGUILayout.Space();

            //--- 顶部 功能按钮 ---
            RenderMenu();
            //--- 顶部 功能按钮 ---

            //--- 表头 ---
            RenderHead();
            //--- 表头 ---

            //--- 主表 ---
            RenderTable();
            //--- 主表 ---

            //--- 分页 ---
            Page();
            //--- 分页 ---

            GUILayout.BeginHorizontal();
            GUILayout.Label(Language.Contract);
            GUILayout.EndHorizontal();
        }


        /// <summary>
        /// 扩展按钮
        /// </summary>
        /// <param name="item"></param>
        protected virtual void RenderFunctionButton(T item)
        {
            if (current_windowType == WindowType.CALLBACK)
            {
                if (GUILayout.Button(Language.Select, new GUILayoutOption[] {GUILayout.Width(80)}))
                {
                    SelctList.Add(item.ID);
                    select_callback(current_list, item);
                    ShowNotification(new GUIContent(string.Format(Language.SuccessAdd, item.NickName)));
                }
            }

            if (current_windowType != WindowType.CALLBACK)
            {
                if (GUILayout.Button(Language.Copy, GUI.skin.GetStyle("ButtonLeft"),
                    new GUILayoutOption[] {GUILayout.Width(19)}))
                {
                    PasteItem = DeepClone(item);
                }

                if (GUILayout.Button(Language.Delete, GUI.skin.GetStyle("ButtonMid"),
                    new GUILayoutOption[] {GUILayout.Width(19)}))
                {
                    deleteList.Add(item.ID);
                    ShowNotification(new GUIContent(Language.Success));
                }

                if (GUILayout.Button(Language.Paste, GUI.skin.GetStyle("ButtonRight"),
                    new GUILayoutOption[] {GUILayout.Width(19)}))
                {
                    if (PasteItem != null)
                    {
                        config_current.ConfigList.Remove(item);
                        PasteItem.ID = item.ID;
                        config_current.ConfigList.Add(DeepClone<T>(PasteItem));
                    }
                }
            }
        }


        protected int jumpTo { get; set; }

        /// <summary>
        /// 分页算法
        /// </summary>
        protected virtual void Page()
        {
            GUILayout.BeginHorizontal(EditorGUIStyle.GetGroupBoxStyle(),
                new GUILayoutOption[] {GUILayout.Width(position.width)});
            int maxIndex = Mathf.FloorToInt((ItemMaxCount - 1) / (float) PageAmount);
            if (maxIndex < PageIndex)
                PageIndex = 0;

            GUILayout.Label(string.Format(Language.PageInfoFormate, PageIndex + 1, maxIndex + 1), GUILayout.Width(100));
            GUILayout.Label(Language.OnePageMaxNumber, EditorGUIStyle.GetPageLabelGuiStyle(), GUILayout.Width(80));
            int.TryParse(GUILayout.TextField(PageAmount.ToString(), GUILayout.Width(40)), out PageAmount);


            if (GUILayout.Button("Jump:",EditorGUIStyle.GetJumpButtonGuiStyle(),GUILayout.Width(38)))
            {
                if (jumpTo-1 <0)
                    jumpTo = 0;

                if (jumpTo-1 >maxIndex)
                    jumpTo = maxIndex;

                PageIndex = jumpTo -1;
            }
            jumpTo = EditorGUILayout.IntField(jumpTo,GUILayout.Width(40));
            
            if (GUILayout.Button(Language.Previous, GUI.skin.GetStyle("ButtonLeft"), GUILayout.Height(16)))
            {
                if (PageIndex - 1 < 0)
                    PageIndex = 0;
                else
                    PageIndex -= 1;
            }

            if (GUILayout.Button(Language.Next, GUI.skin.GetStyle("ButtonRight"), GUILayout.Height(16)))
            {
                if (PageIndex + 1 > maxIndex)
                    PageIndex = maxIndex;
                else
                    PageIndex++;
            }

            Resize = GUILayout.HorizontalSlider(Resize, 1, 10, GUILayout.Width(70));

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 当前界面设置 Select 回调
        /// </summary>
        /// <param name="current_list"></param>
        /// <param name="callback"></param>
        public override void UpdateSelectModel(object current_list, Action<object, object> callback)
        {
            current_windowType = WindowType.CALLBACK;
            select_callback = callback;
            this.current_list = current_list;
        }


        /// <summary>
        /// 深拷贝 复制数据
        /// </summary>
        /// <param name="a"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public K DeepClone<K>(K a)
        {
            var content = JsonUtility.ToJson(a);
            return JsonUtility.FromJson<K>(content);
        }

//        /// <summary>
//        /// 在当前程序集总获取Type 不能放在其他程序集
//        /// </summary>
//        /// <param name="type_name"></param>
//        /// <returns></returns>
//        protected virtual Type GetType(string type_name)
//        {
//            return Assembly.GetExecutingAssembly().GetType(type_name);
//        }
    }
}
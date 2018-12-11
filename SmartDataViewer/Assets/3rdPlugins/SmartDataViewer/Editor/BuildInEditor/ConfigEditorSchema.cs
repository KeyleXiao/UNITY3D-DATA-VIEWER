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
using System.Text;
using SmartDataViewer.Helpers;
using UnityEngine.Profiling;

namespace SmartDataViewer.Editor.BuildInEditor
{
    /// <summary>
    /// 存储反射字段信息的容器
    /// </summary>
    public class ConfigEditorSchemaChache
    {
        public FieldInfo field_info { get; set; }
        public ControlProperty config_editor_setting { get; set; }
    }

    /// <summary>
    /// 存储反射的行数据
    /// </summary>
    public class ConfigEditorLineChache
    {
        public int HashCode { get; set; }
        public bool IsGenericType { get; set; }
        public MethodInfo Add { get; set; }
        public MethodInfo RemoveAt { get; set; }
        public PropertyInfo Count { get; set; }
        public PropertyInfo Item { get; set; }
        public Type AttributeType { get; set; }
    }

    /// <summary>
    /// 泛化数据编辑器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigEditorSchema<T> : IMultipleWindow where T : IModel, new()
    {
        /// <summary>
        /// 当前编辑器配置
        /// </summary>
        protected EditorProperty currentEditorSetting;


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
        /// 当前页转跳位置
        /// </summary>
        protected int jumpTo = 1;

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
        /// 缓存行级别结构
        /// </summary>
        public Dictionary<int, ConfigEditorLineChache> LineChache { get; set; }

        /// <summary>
        /// 首次加载标示
        /// </summary>
        protected bool FirstLoadFlag { get; set; }


        /// <summary>
        /// 搜索结果
        /// </summary>
        protected List<T> Finallylist { get; set; }


        /// <summary>
        /// 字段排序
        /// </summary>
        protected Dictionary<string, bool> FieldsOrder { get; set; }


        //TODO 3.0 外联表原始数据
        /// <summary>
        /// 外链表数据
        /// </summary>
        protected Dictionary<int, object> outLinkRawData { get; set; }


        /// <summary>
        /// 选择模式下 选中的列ID 只做为一种表现 没有数据意义
        /// </summary>
        public List<int> SelectDisplayEffectList { get; set; }


        /// <summary>
        /// 粘贴行数据
        /// </summary>
        protected T PasteItem { get; set; }

        /// <summary>
        /// 当前编辑器程序集
        /// </summary>
        public Assembly CurrentAssembly { get; set; }

        /// <summary>
        /// 临时变量
        /// </summary>
        public int TempHashCode { get; set; }

        /// <summary>
        /// 临时变量
        /// </summary>
        public ConfigEditorLineChache TempLineChache { get; set; }

        /// <summary>
        /// 报错显示
        /// </summary>
        public List<int> ErrorLine { get; set; }

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
        /// 获取代码生成器关联信息
        /// </summary>
        /// <returns></returns>
        public virtual CodeGen GetCodeGenInfo()
        {
            return null;
        }


        /// <summary>
        /// 设置想要编辑的类
        /// </summary>
        /// <param name="tp"></param>
        public void SetConfigType(ConfigBase<T> tp)
        {
            config_current = tp;

            var configSetting = ConfigEditorAttribute.GetCurrentAttribute<ConfigEditorAttribute>(tp) ??
                                new ConfigEditorAttribute();

            //先读取用户定义的 如果没有配置直接读取默认的
            currentEditorSetting =
                EditorConfig.GetCustomEditorPropertyConfig().SearchByID(configSetting.EditorConfigID) ??
                EditorConfig.GetDefaultEditorPropertyConfig().SearchByID(configSetting.EditorConfigID);


            titleContent = new GUIContent(string.IsNullOrEmpty(currentEditorSetting.EditorTitle)
                ? typeof(T).Name
                : currentEditorSetting.EditorTitle);

            fieldinfos = typeof(T).GetFields().ToList();

            //-- Chache --
            Chache = new List<ConfigEditorSchemaChache>();
            LineChache = new Dictionary<int, ConfigEditorLineChache>();
            CurrentAssembly = Assembly.GetExecutingAssembly();
            //-- Chache --

            //-- Error --
            ErrorLine = new List<int>();
            //-- Error --

            foreach (var item in fieldinfos)
            {
                var infos = item.GetCustomAttributes(typeof(ConfigEditorFieldAttribute), true);
                ConfigEditorSchemaChache f = new ConfigEditorSchemaChache { field_info = item };

                if (infos.Length == 0)
                {
                    int id = (int) ReflectionHelper.GetFieldTypeMapping(item.FieldType);
                    f.config_editor_setting = EditorConfig.GetDefaultControlPropertyConfig().SearchByID(id);


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

                    //先查Custom配置
                    f.config_editor_setting =
                        EditorConfig.GetCustomControlPropertyConfig().SearchByID(cefa.ControlPropertyID);

                    //走属性默认配置
                    if (f.config_editor_setting == null && cefa.ControlPropertyID == 0)
                    {
                        int id = (int) ReflectionHelper.GetFieldTypeMapping(item.FieldType);
                        f.config_editor_setting = EditorConfig.GetDefaultControlPropertyConfig().SearchByID(id);
                        f.config_editor_setting = EditorConfig.GetDefaultControlPropertyConfig().SearchByID(id);
                    }
                    

                    //走默认配置
                    if (f.config_editor_setting == null && cefa.ControlPropertyID != 0)
                    {
                        f.config_editor_setting = EditorConfig.GetDefaultControlPropertyConfig()
                            .SearchByID(cefa.ControlPropertyID);
                    }

                    //如果默认配置被删除 为了防止报错 
                    if (f.config_editor_setting == null)
                    {
                        Log("Can't find default control id :" + cefa.ControlPropertyID);
                        continue;
                    }

                    if (!f.config_editor_setting.Visibility)
                        continue;
                    
                }

                Chache.Add(f); 
            }
             

            if (Chache.Count > 0)
            {
                Chache = Chache.OrderByDescending(x => x.config_editor_setting.Order).ToList();
            }

            initialized = true;
        }


        public string GetFieldDisplayFromChache(string name)
        {
            if (Chache == null)
            {
                return name;
            }
            for (int i = 0; i < Chache.Count; i++)
            {
                if (Chache[i].field_info.Name == name)
                {
                    return string.IsNullOrEmpty(Chache[i].config_editor_setting.Display) ? name : Chache[i].config_editor_setting.Display;
                }                
            }
            return name;
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
        /// 根据CodeGenID找到关联信息打开编辑器，如果是内建编辑器会找不到CodeGen信息 这时候再重写一下GetCodeGen函数即可
        /// </summary>
        /// <param name="codeGenID"></param>
        /// <returns></returns>
        public virtual IMultipleWindow OpenOutlinkWindow(int codeGenID)
        {
            //TODO: 这里先暂时这样处理
            var editorName = EditorConfig.GetCodeGenConfig().SearchByID(codeGenID).EditorName;

            IMultipleWindow e = CurrentAssembly.CreateInstance(editorName) as IMultipleWindow;
            return e;
        }

        /// <summary>
        /// 反射原始数据 TODO: 加入缓存来优化反射  --Complete
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <param name="raw"></param>
        public virtual void RenderRawLine(ConfigEditorSchemaChache data, object value, T raw)
        {
            if (value == null) return;

            // ---- Chache ----
            TempHashCode = value.GetHashCode();

            if (LineChache.ContainsKey(TempHashCode))
            {
                TempLineChache = LineChache[TempHashCode];
            }
            else
            {
                var currentLineChache = new ConfigEditorLineChache();
                currentLineChache.HashCode = value.GetHashCode();
                currentLineChache.IsGenericType = value.GetType().IsGenericType;
                currentLineChache.Add = value.GetType().GetMethod("Add");
                currentLineChache.RemoveAt = value.GetType().GetMethod("RemoveAt");
                currentLineChache.Count = value.GetType().GetProperty("Count");
                currentLineChache.Item = value.GetType().GetProperty("Item");

                if (currentLineChache.IsGenericType && data.config_editor_setting.OutCodeGenEditorID == 0)
                    currentLineChache.AttributeType = value.GetType().GetGenericArguments()[0];

                LineChache.Add(TempHashCode, currentLineChache);
                TempLineChache = currentLineChache;
            }
            // -----------------


            if (TempLineChache.IsGenericType)
            {
                GUILayout.BeginVertical(GUIStyle.none,
                    new GUILayoutOption[]
                    {
                        GUILayout.Width(GetResizeWidth(data.config_editor_setting.Width,
                            data.config_editor_setting.MaxWidth))
                    });

                int deleteIndex = -1;

                //Open Editor
                if (data.config_editor_setting.OutCodeGenEditorID != 0)
                {
                    //处理外联添加逻辑
                    var temp = value as List<int>;

                    for (int i = 0; i < temp.Count; i++)
                    {
                        GUILayout.BeginHorizontal();

                        //显示外联数据
                        GUILayout.Label(GetOutLinkDisplayField(
                                temp[i],
                                data.config_editor_setting.OutCodeGenEditorID,
                                data.config_editor_setting.OutLinkDisplay),
                            GUILayout.Width(
                                GetResizeWidth(
                                    data.config_editor_setting.Width,
                                    data.config_editor_setting.MaxWidth,
                                    currentEditorSetting.KitButtonWidth + currentEditorSetting.ColumnSpan
                                )
                            )
                        );

                        //删除子类型数据
                        if (GUILayout.Button(Language.Delete, GUILayout.Width(currentEditorSetting.KitButtonWidth)))
                            deleteIndex = i;

                        GUILayout.EndHorizontal();
                    }
                    //处理外联添加逻辑

                    // -- 打开面板 --
                    if (GUILayout.Button(Language.Add))
                    {
                        var e = OpenOutlinkWindow(data.config_editor_setting.OutCodeGenEditorID);
                        if (e == null)
                        {
                            ShowNotification(new GUIContent(Language.OutLinkIsNull));
                        }
                        else
                        {
                            e.UpdateSelectModel(value as List<int>, SetListItemValue);
                            e.ShowUtility();
                        }
                    }
                    // -- 打开面板 --
                    
                    

                    if (deleteIndex != -1)
                    {
                        temp.RemoveAt(deleteIndex);
                        //TODO 此处应该处理联动列需求
                        
                    }
                }
                else
                {
                    //处理数组逻辑
                    int count = Convert.ToInt32(TempLineChache.Count.GetValue(value, null));

                    int removeIndex = -1;

                    for (int i = 0; i < count; i++)
                    {
                        object listItem = TempLineChache.Item.GetValue(value, new object[] {i});


                        GUILayout.BeginHorizontal();
                        //alignment
                        RenderBaseControl(
                            GetResizeWidth(data.config_editor_setting.Width, data.config_editor_setting.MaxWidth,
                                currentEditorSetting.KitButtonWidth + currentEditorSetting.ColumnSpan),
                            data.config_editor_setting.CanEditor,
                            listItem,
                            v => { TempLineChache.Item.SetValue(value, v, new object[] {i}); });

                        if (GUILayout.Button(Language.Delete,
                            new GUILayoutOption[] {GUILayout.Width(currentEditorSetting.KitButtonWidth)}))
                        {
                            removeIndex = i;
                        }

                        GUILayout.EndHorizontal();
                    }

                    if (removeIndex != -1)
                    {
                        TempLineChache.RemoveAt.Invoke(value, new object[] {removeIndex});
                    }
                    //处理数组逻辑


                    //添加数据
                    if (GUILayout.Button(Language.Add))
                    {
                        TempLineChache.Add.Invoke(value,
                            new object[]
                            {
                                TempLineChache.AttributeType == typeof(string)
                                    ? string.Empty
                                    : Activator.CreateInstance(TempLineChache.AttributeType)
                            });
                    }

                    //添加数据
                }

                GUILayout.EndVertical();
            }
            else
            {
                //Open Editor
                if (data.config_editor_setting.OutCodeGenEditorID != 0)
                {
//                    data.field_info.SetValue(raw,GetSelectValueByFlag(raw.ID, data.field_info.Name, (int) value));
                    
                    string buttonName = GetOutLinkDisplayField((int) value,
                        data.config_editor_setting.OutCodeGenEditorID,
                        data.config_editor_setting.OutLinkDisplay);

                    if (GUILayout.Button(buttonName,
                        new GUILayoutOption[]
                        {
                            GUILayout.Width(GetResizeWidth(data.config_editor_setting.Width,
                                data.config_editor_setting.MaxWidth))
                        }))
                    {
                        var e = OpenOutlinkWindow(data.config_editor_setting.OutCodeGenEditorID);

                        if (e == null)
                            ShowNotification(new GUIContent(Language.OutLinkIsNull));
                        else
                        {
                            e.UpdateSelectModel(raw,data.field_info, SetListItemValue);
                            e.ShowUtility();
                        }
                    }
                }
                else
                {
                    RenderBaseControl(
                        GetResizeWidth(data.config_editor_setting.Width, data.config_editor_setting.MaxWidth),
                        data.config_editor_setting.CanEditor, value,
                        v => { data.field_info.SetValue(raw, v); });
                }
            }
        }

        
        /// <summary>
        /// 这个函数应该是当前界面内的，当打开的界面有信息修改会调用此界面的回调函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="addValue"></param>
        public virtual void SetListItemValue(List<int> item, IModel addValue)
        {
//            var temp = item as List<int>;
//            var model = addValue as IModel;
//            temp.Add(model.ID);
            item.Add(addValue.ID);
        }


        public virtual void SetListItemValue(object item,IModel addValue,FieldInfo field)
        {
            field.SetValue(item,addValue.ID);
            ShowNotification(new GUIContent(string.Format(Language.SuccessAdd, addValue.NickName)));//弹出消息提示  
        }

        /// <summary>
        /// 使用currentEditorSetting.isLog控制输出
        /// </summary>
        /// <param name="str"></param>
        public virtual void Log(string str)
        {
            if (currentEditorSetting.isLog) Debug.Log(str);
        }


        //TODO: 增加Chache深度
        /// <summary>
        /// 获取外链编辑器的按钮上显示的字符，未指定则显示ID 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="outLinkChacheKey"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string GetOutLinkDisplayField(int id, int outLinkChacheKey, string field)
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
                    value = EditorGUILayout.EnumPopup(value as Enum, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField((value as Enum).ToString(),
                        new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Bounds)
            {
                if (enable)
                {
                    value = EditorGUILayout.BoundsField((Bounds) value, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.BoundsField((Bounds) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Color)
            {
                if (enable)
                {
                    value = EditorGUILayout.ColorField((Color) value, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.ColorField((Color) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is AnimationCurve)
            {
                if (enable)
                {
                    value = EditorGUILayout.CurveField((AnimationCurve) value,
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.CurveField((AnimationCurve) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is string)
            {
                if (enable)
                {
                    value = EditorGUILayout.TextField(value as string, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is float)
            {
                if (enable)
                {
                    value = EditorGUILayout.FloatField((float) value, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is int)
            {
                if (enable)
                {
                    value = EditorGUILayout.IntField((int) value, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is bool)
            {
                if (enable)
                {
                    value = EditorGUILayout.Toggle((bool) value, EditorGUIStyle.GetTogleStyle(),
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(((bool) value).ToString(),
                        new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Vector2)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector2Field("", (Vector2) value,
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector2Field("", (Vector2) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Vector3)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector3Field("", (Vector3) value,
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector3Field("", (Vector3) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Vector4)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector4Field("", (Vector4) value,
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector4Field("", (Vector4) value, new GUILayoutOption[] {GUILayout.Width(width)});
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

            config_current = ConfigContainerFactory.GetInstance(GetCodeGenInfo().ContainerType)
                .LoadConfig<ConfigBase<T>>(GetCodeGenInfo().InOutPath);

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
                outLinkRawData = new Dictionary<int, object>();
            else
                outLinkRawData.Clear();

            for (int i = 0; i < Chache.Count; i++)
            {
                //Bug fix: 自动做默认参数初始化 
                if (Chache[i].config_editor_setting == null)
                {
                    Chache[i].config_editor_setting = new ControlProperty();
                    continue;
                }

                if (outLinkRawData.ContainsKey(Chache[i].config_editor_setting.OutCodeGenEditorID))
                    continue;

                if (Chache[i].config_editor_setting.OutCodeGenEditorID == 0)
                    continue;


                //重写外联编辑器逻辑


                var codeGenInfo = EditorConfig.GetCodeGenConfig()
                    .SearchByID(Chache[i].config_editor_setting.OutCodeGenEditorID);

                if (codeGenInfo == null)
                {
                    Log("Can't find id In CodeGen Table" + Chache[i].config_editor_setting.OutCodeGenEditorID);
                    continue;
                }

                //查找非编辑器命名控件下
                Type classType = ReflectionHelper.GetCurrentAssemblyType(codeGenInfo.ClassType);

                //查找编辑器命名控件下
                if (classType == null) classType = GetType(codeGenInfo.ClassType);


                if (classType == null)
                {
                    Log("Can't find calss " + codeGenInfo.ClassType);
                    continue;
                }

                var modelRaw = ConfigContainerFactory.GetInstance(codeGenInfo.ContainerType)
                    .LoadConfig(classType, codeGenInfo.InOutPath);

                if (modelRaw == null)
                {
                    Log(string.Format("Loading OutLink Class [{0}] Data failed !", codeGenInfo.ClassType));
                    return;
                }

                outLinkRawData.Add(Chache[i].config_editor_setting.OutCodeGenEditorID, modelRaw);

                Log(string.Format("Loading OutLink Class [{0}] Data Success !", codeGenInfo.ClassType));

//                if (string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkEditor))
//                {
//                    if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkClass))
//                    {
//                        Chache[i].config_editor_setting.OutLinkEditor =
//                            Chache[i].config_editor_setting.OutLinkClass + "Editor";
//                    }
//                    else if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkSubClass))
//                    {
//                        Chache[i].config_editor_setting.OutLinkEditor =
//                            Chache[i].config_editor_setting.OutLinkSubClass + "ConfigEditor";
//                    }
//                    else
//                    {
//                        Log("Out link info is null ...");
//                        continue;
//                    }
//                }
//
//                if (string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkClass))
//                {
//                    if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkSubClass))
//                        Chache[i].config_editor_setting.OutLinkClass =
//                            Chache[i].config_editor_setting.OutLinkSubClass + "Config";
//                    else
//                        Chache[i].config_editor_setting.OutLinkClass =
//                            Chache[i].config_editor_setting.OutLinkEditor.Replace("Editor", "");
//                }
//
//                if (string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkFilePath))
//                {
//                    if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkSubClass))
//                        Chache[i].config_editor_setting.OutLinkFilePath =
//                            Chache[i].config_editor_setting.OutLinkSubClass;
//                    else
//                        Chache[i].config_editor_setting.OutLinkFilePath = Chache[i].config_editor_setting.OutLinkClass
//                            .Replace("ConfigEditor", "");
//                }
//
//
//                //TODO VERSION 2.0 Load Raw Data
//                if (!string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkEditor) &&
//                    !string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkClass) &&
//                    !string.IsNullOrEmpty(Chache[i].config_editor_setting.OutLinkFilePath)
//                )
//                {
//                    string rawClass = Chache[i].config_editor_setting.OutLinkClass;
//                    if (string.IsNullOrEmpty(rawClass))
//                        rawClass = Chache[i].config_editor_setting.OutLinkSubClass + "Config";
//                    Type classType = ReflectionHelper.GetCurrentAssemblyType(rawClass);
//                    if (classType == null)
//                    {
//                        Log("Can't find calss " + rawClass);
//                        continue;
//                    }
//
//                    var modelRaw =
//                        ConfigBase<IModel>.LoadConfig(classType, Chache[i].config_editor_setting.OutLinkFilePath);
//                    if (modelRaw != null)
//                    {
//                        if (!outLinkRawData.ContainsKey(Chache[i].config_editor_setting.OutLinkClass))
//                            outLinkRawData.Add(Chache[i].config_editor_setting.OutLinkClass, modelRaw);
//
//                        Log(string.Format("Loading OutLink Class [{0}] Data",
//                            Chache[i].config_editor_setting.OutLinkClass));
//                    }
//                }
            }
        }


        /// <summary>
        /// 程序预留 数据逻辑校验接口
        /// </summary>
        /// <param name="data"></param>
        protected virtual string VerfiyLineData(T data, bool showNotification = true)
        {
            string error = "";
//                    -- 测试数据 --          
//            if (string.IsNullOrEmpty(data.NickName))
//            {
//                error = string.Format("当前ID:{0} 建议NickName字段不要为空 请您补全描述信息",data.ID);
//                if (!ErrorLine.Contains(data.ID))ErrorLine.Add(data.ID); //添加报错提示
//
//                if (showNotification)
//                    ShowNotification(new GUIContent(error));    
//            }
//            else
//                    -- 测试数据 --            
            {
                if (showNotification)
                    ShowNotification(new GUIContent(string.Format(Language.VerfiyMessageSuccess, data.ID)));

                if (ErrorLine.Contains(data.ID)) ErrorLine.Remove(data.ID); //校验成功解除报错信息
            }

            return error;
        }


        /// <summary>
        /// 导出数据之前校验所有行的数据逻辑 如果重写此函数请一并重写 VerfiyLineData 函数
        /// </summary>
        /// <returns></returns>
        protected virtual bool VerfiyExportData()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < config_current.ConfigList.Count; i++)
            {
                //跳过被删除的数据
                if (deleteList.Contains(config_current.ConfigList[i].ID))
                    continue;

                //保存错误信息
                string errorInfo = VerfiyLineData(config_current.ConfigList[i], false);

                if (!string.IsNullOrEmpty(errorInfo))
                    sb.Append("\n").Append(errorInfo);
            }

            if (sb.Length == 0) return true;

            ShowNotification(new GUIContent(Language.PleaseCheckConsole));
            //格式化输出
            Debug.LogError(string.Format(Language.TableErrorInfoFormat, currentEditorSetting.EditorTitle, sb));
            //格式化输出
            return false;
        }

        /// <summary>
        /// 保存配置（逻辑）
        /// </summary>
        protected virtual void SaveConfig()
        {
            //批量调用校验逻辑
            if (!VerfiyExportData())
                return;
            //批量调用校验逻辑


            //处理删除逻辑
            for (int i = 0; i < deleteList.Count; i++)
            {
                config_current.Delete(deleteList[i]);
            }

            deleteList.Clear();
            //处理删除逻辑


            //处理路径关系

            if (GetCodeGenInfo() == null)
            {
                ShowNotification(new GUIContent(Language.CantReadOutputPath));
                return;
            }

            if (ConfigContainerFactory.GetInstance(GetCodeGenInfo().ContainerType)
                .SaveToDisk(GetCodeGenInfo().InOutPath, config_current))
            {
                AssetDatabase.Refresh();
                ShowNotification(new GUIContent(Language.Success));
            }
            else
            {
                ShowNotification(new GUIContent(Language.SaveFailed));
            }
        }

        /// <summary>
        /// 保存按钮 gui
        /// </summary>
        protected virtual void SaveButton()
        {
            if (GUILayout.Button(Language.Save, GUI.skin.GetStyle("ButtonRight")))
            {
                SaveConfig();
            }
        }

        /// <summary>
        /// 搜索 gui
        /// </summary>
        protected virtual void SearchField()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Language.NickName, GUILayout.Width(70));
            currentEditorSetting.SearchResourceName =
                EditorGUILayout.TextField(currentEditorSetting.SearchResourceName,
                    GUI.skin.GetStyle("ToolbarSeachTextField"));
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
                new GUILayoutOption[] {GUILayout.Height(currentEditorSetting.TableHeadHeight)});

            GUILayout.BeginHorizontal(EditorGUIStyle.GetGroupBoxStyle(),
                new GUILayoutOption[] {GUILayout.Width(position.width)});

            //TODO Set Order
            foreach (var item in Chache)
            {
                if (GUILayout.Button(
                    string.IsNullOrEmpty(item.config_editor_setting.Display)
                        ? item.field_info.Name
                        : item.config_editor_setting.Display, EditorGUIStyle.GetTagButtonStyle(),
                    new GUILayoutOption[]
                    {
                        GUILayout.Width(GetResizeWidth(item.config_editor_setting.Width,
                            item.config_editor_setting.MaxWidth))
                    }))
                    HeadButton_Click(item.field_info.Name);

                GUILayout.Space(currentEditorSetting.ColumnSpan);
            }

            RenderExtensionHead();

            if (current_windowType == WindowType.CALLBACK)
                GUILayout.Label(Language.Select, EditorGUIStyle.GetTagButtonStyle(),
                    GUILayout.Width(currentEditorSetting.ExtensionHeadTagWith));
            else if (current_windowType == WindowType.INPUT)
                GUILayout.Label(Language.Operation, EditorGUIStyle.GetTagButtonStyle(),
                    GUILayout.Width(currentEditorSetting.ExtensionHeadTagWith));
            else
                GUILayout.Label(Language.Operation, EditorGUIStyle.GetTagButtonStyle(),
                    GUILayout.Width(currentEditorSetting.ExtensionHeadTagWith));

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

            if (!string.IsNullOrEmpty(currentEditorSetting.SearchResourceName))
            {
                currentEditorSetting.ItemMaxCount = config_current.ConfigList.Count(x =>
                    x.NickName.ToLower().Contains(currentEditorSetting.SearchResourceName.ToLower().Trim()));
                Finallylist = config_current.ConfigList
                    .Where(x => x.NickName.ToLower().Contains(currentEditorSetting.SearchResourceName.ToLower().Trim()))
                    .Skip(currentEditorSetting.PageIndex * currentEditorSetting.PageAmount)
                    .Take(currentEditorSetting.PageAmount).ToList();
            }
            else
            {
                currentEditorSetting.ItemMaxCount = config_current.ConfigList.Count;
                Finallylist = config_current.ConfigList
                    .Skip(currentEditorSetting.PageIndex * currentEditorSetting.PageAmount)
                    .Take(currentEditorSetting.PageAmount).ToList();
            }

            //遍历搜索结果
            for (int i = 0; i < Finallylist.Count; i++)
            {
                T item = Finallylist[i];

                if (deleteList.Contains(item.ID))
                    continue;

                //Select effect diaplay
                if (current_windowType == WindowType.CALLBACK && SelectDisplayEffectList.Contains(item.ID))
                    GUI.backgroundColor = Color.green;
                else GUI.backgroundColor = Color.white;


                GUILayout.BeginHorizontal(EditorGUIStyle.GetGroupBoxStyle());

                foreach (var schema in Chache)
                {
                    var rawData = schema.field_info.GetValue(item);

                    if (ErrorLine.Contains(item.ID)) GUI.color = Color.red;
                    RenderRawLine(schema, rawData, item);
                    GUI.color = Color.white;

                    GUILayout.Space(currentEditorSetting.ColumnSpan);
                }

                RenderExtensionButton(item);
                GUILayout.Space(currentEditorSetting.ColumnSpan);

                RenderFunctionButton(item);
                GUILayout.Space(currentEditorSetting.ColumnSpan);

                GUILayout.EndHorizontal();
                GUILayout.Space(5);
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


            if (!currentEditorSetting.DisableCreate && current_windowType != WindowType.CALLBACK)
                NewLineButton();

            if (!currentEditorSetting.DisableSave && current_windowType != WindowType.CALLBACK)
                SaveButton();

            GUILayout.EndHorizontal();


            if (!currentEditorSetting.DisableSearch)
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
                if (current_windowType == WindowType.CALLBACK) SelectDisplayEffectList = new List<int>(); //init select display
            }


            if (!FirstLoadFlag)
            {
                FirstLoadFlag = true;
                Reload();
            }

            //TODO: 有一种情况 生成需要unity编译的脚本 这时候打开编辑器会报错(存留在内存的数据被clear) 这时候需要先关闭编辑器
            if (currentEditorSetting == null)
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

            //-- 版权信息 --
            GUILayout.BeginHorizontal();
            GUILayout.Label(Language.Contract);
            GUILayout.EndHorizontal();
        }


        /// <summary>
        /// 扩展功能按钮栏
        /// </summary>
        /// <param name="item"></param>
        protected virtual void RenderFunctionButton(T item)
        {
            if (current_windowType == WindowType.CALLBACK)
            {
                if (GUILayout.Button(Language.Select, GUI.skin.GetStyle("ButtonLeft"),
                    GUILayout.Width(currentEditorSetting.KitButtonWidth * 2)))
                {
                    //要确认当前是何种模式
                    if (select_model == SelectModel.MUTI)
                    {
                        SelectDisplayEffectList.Add(item.ID);
                        select_callback(selector_raw_list, item);
                        ShowNotification(new GUIContent(string.Format(Language.SuccessAdd, item.NickName)));
                    }
                    else
                    {
                        single_select_callback(selector_raw_single, item , selector_raw_field_single);
                        Close();
                    }
                }

                if (GUILayout.Button(Language.Close, GUI.skin.GetStyle("ButtonRight"),
                    GUILayout.Width(currentEditorSetting.KitButtonWidth * 2)))
                {
                    Close();
                    return;
                }
            }

            if (current_windowType != WindowType.CALLBACK)
            {
                if (GUILayout.Button(Language.Copy, GUI.skin.GetStyle("ButtonLeft"),
                    new GUILayoutOption[] {GUILayout.Width(currentEditorSetting.KitButtonWidth)}))
                {
                    PasteItem = DeepClone(item);
                }

                if (GUILayout.Button(Language.Delete, GUI.skin.GetStyle("ButtonMid"),
                    new GUILayoutOption[] {GUILayout.Width(currentEditorSetting.KitButtonWidth)}))
                {
                    deleteList.Add(item.ID);
                    ShowNotification(new GUIContent(Language.Success));
                }

                if (GUILayout.Button(Language.Verfiy, GUI.skin.GetStyle("ButtonMid"),
                    new GUILayoutOption[] {GUILayout.Width(currentEditorSetting.KitButtonWidth)}))
                {
                    VerfiyLineData(item);
                }

                if (GUILayout.Button(Language.Paste, GUI.skin.GetStyle("ButtonRight"),
                    new GUILayoutOption[] {GUILayout.Width(currentEditorSetting.KitButtonWidth)}))
                {
                    if (PasteItem != null)
                    {
                        //Bug fix : 从指定位置删除插入
                        var insertPos = config_current.ConfigList.IndexOf(item);
                        config_current.ConfigList.Remove(item);
                        PasteItem.ID = item.ID;
                        config_current.ConfigList.Insert(insertPos, DeepClone(PasteItem));
                    }
                }
            }
        }


        /// <summary>
        /// 分页算法
        /// </summary>
        protected virtual void Page()
        {
            GUILayout.BeginHorizontal(EditorGUIStyle.GetGroupBoxStyle(),
                new GUILayoutOption[] {GUILayout.Width(position.width)});
            int maxIndex =
                Mathf.FloorToInt((currentEditorSetting.ItemMaxCount - 1) / (float) currentEditorSetting.PageAmount);
            if (maxIndex < currentEditorSetting.PageIndex)
                currentEditorSetting.PageIndex = 0;

            GUILayout.Label(string.Format(Language.PageInfoFormate, currentEditorSetting.PageIndex + 1, maxIndex + 1),
                GUILayout.Width(100));
            GUILayout.Label(Language.OnePageMaxNumber, EditorGUIStyle.GetPageLabelGuiStyle(), GUILayout.Width(80));
            int.TryParse(GUILayout.TextField(currentEditorSetting.PageAmount.ToString(), GUILayout.Width(40)),
                out currentEditorSetting.PageAmount);


            if (GUILayout.Button(Language.Jump, EditorGUIStyle.GetJumpButtonGuiStyle(), GUILayout.Width(40)))
            {
                if (jumpTo - 1 < 0)
                    jumpTo = 0;

                if (jumpTo - 1 > maxIndex)
                    jumpTo = maxIndex;

                currentEditorSetting.PageIndex = jumpTo - 1;
            }

            jumpTo = EditorGUILayout.IntField(jumpTo, GUILayout.Width(40));

            if (GUILayout.Button(Language.Previous, GUI.skin.GetStyle("ButtonLeft"), GUILayout.Height(16)))
            {
                if (currentEditorSetting.PageIndex - 1 < 0)
                    currentEditorSetting.PageIndex = 0;
                else
                    currentEditorSetting.PageIndex -= 1;
            }

            if (GUILayout.Button(Language.Next, GUI.skin.GetStyle("ButtonRight"), GUILayout.Height(16)))
            {
                if (currentEditorSetting.PageIndex + 1 > maxIndex)
                    currentEditorSetting.PageIndex = maxIndex;
                else
                    currentEditorSetting.PageIndex++;
            }

            //处理缩放
            if (!currentEditorSetting.HideResizeSlider)
                currentEditorSetting.Resize =
                    (int) GUILayout.HorizontalSlider(currentEditorSetting.Resize, 1, 5, GUILayout.Width(70));

            GUILayout.EndHorizontal();
        }


        /// <summary>
        /// 计算缩放尺寸 目前只有两处调用  如果后面再有其他地方调用  需要重写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual int GetResizeWidth(int value, int max, int offset = 0)
        {
            if (currentEditorSetting.HideResizeSlider)
            {
                return value;
            }
            else
            {
                return Mathf.Clamp(value * currentEditorSetting.Resize - offset, 0, max - offset);
            }
        }

//        /// <summary>
//        /// 当前界面设置 Select 回调
//        /// </summary>
//        /// <param name="current_list"></param>
//        /// <param name="callback"></param>
//        public override void UpdateSelectModel(List<int> current_list, Action<List<int>, IModel> callback)
//        {
//            base.UpdateSelectModel(current_list,callback);
//            current_windowType = WindowType.CALLBACK;
//            this.selector_raw_list = current_list;
//        }
//
//        public override void UpdateSelectModel(IModel model, Action<IModel, IModel> callback)
//        {
//            base.UpdateSelectModel(model,callback);
//            current_windowType = WindowType.CALLBACK;
//            this.selector_raw_single = model;
//        }


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
        protected virtual Type GetType(string type_name)
        {
            if (string.IsNullOrEmpty(type_name))
            {
                return null;
            }
            return Assembly.GetExecutingAssembly().GetType(type_name);
        }
    }
}

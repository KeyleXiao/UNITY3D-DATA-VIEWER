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

namespace SmartDataViewer.Editor.BuildInEditor
{
    /// <summary>
    /// 泛化数据编辑器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ConfigEditorSchema<T> : IMultipleWindow where T : new()
    {
        protected ConfigEditorSchemaData<T> Data { get; set; }
        
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
        /// 首次加载标示
        /// </summary>
        protected bool FirstLoadFlag { get; set; }

        /// <summary>
        /// 搜索结果
        /// </summary>
        protected List<ConfigEditorLineCache<T>> Finallylist { get; set; }

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
        /// 临时变量
        /// </summary>
        public int TempHashCode { get; set; }

        /// <summary>
        /// 临时变量
        /// </summary>
        public ConfigEditorLineFieldCache TempLineFieldFieldCache { get; set; }

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
            Data = new ConfigEditorSchemaData<T>();
            Data.config_current = tp;

            var configSetting = ConfigEditorAttribute.GetFirstAttribute<ConfigEditorAttribute>(tp) ??
                                new ConfigEditorAttribute();

            //先读取用户定义的 如果没有配置直接读取默认的
            Data.currentEditorSetting =
                EditorConfig.GetDefaultEditorPropertyConfig().SearchByOrderKey(configSetting.EditorConfigID);


            titleContent = new GUIContent(string.IsNullOrEmpty(Data.currentEditorSetting.EditorTitle)
                ? typeof(T).Name
                : Data.currentEditorSetting.EditorTitle);

            fieldinfos = typeof(T).GetFields().ToList();

            //-- Chache --

            Data.CurrentAssembly = Assembly.GetExecutingAssembly();
            //-- Chache --

            //-- Error --
            ErrorLine = new List<int>();
            //-- Error --

            foreach (var item in fieldinfos)
            {
                var infos = item.GetCustomAttributes(typeof(ConfigEditorFieldAttribute), true);
                var f = new ConfigEditorSchemaChache { field_info = item };

                if (infos.Length == 0)
                {
                    int id = (int) ReflectionHelper.GetFieldTypeMapping(item.FieldType);
                    f.config_editor_setting = EditorConfig.GetDefaultControlPropertyConfig().SearchByOrderKey(id);


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
                    var cefa = (ConfigEditorFieldAttribute) infos[0];

                    f.CurrentFiledProperty = cefa.CurrentFiledProperty;
                    //先查Custom配置
                    f.config_editor_setting =
                        EditorConfig.GetDefaultControlPropertyConfig().SearchByOrderKey(cefa.ControlPropertyID);

                    //走属性默认配置
                    if (f.config_editor_setting == null && cefa.ControlPropertyID == 0)
                    {
                        int id = (int) ReflectionHelper.GetFieldTypeMapping(item.FieldType);
                        f.config_editor_setting = EditorConfig.GetDefaultControlPropertyConfig().SearchByOrderKey(id);
                        // f.config_editor_setting = EditorConfig.GetDefaultControlPropertyConfig().SearchByOrderKey(id);
                    }
                    

                    //走默认配置
                    if (f.config_editor_setting == null && cefa.ControlPropertyID != 0)
                    {
                        f.config_editor_setting = EditorConfig.GetDefaultControlPropertyConfig()
                            .SearchByOrderKey(cefa.ControlPropertyID);
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

                Data.CurrentClassFieldsCache.Add(f); 
            }
             

            if (Data.CurrentClassFieldsCache.Count > 0)
            {
                Data.CurrentClassFieldsCache = Data.CurrentClassFieldsCache.OrderByDescending(x => x.config_editor_setting.Order).ToList();
            }
            
            initialized = true;
        }


        public string GetFieldDisplayFromChache(string name)
        {
            if (Data.CurrentClassFieldsCache == null) return name;
            
            for (var i = 0; i < Data.CurrentClassFieldsCache.Count; i++)
            {
                if (Data.CurrentClassFieldsCache[i].field_info.Name == name)
                {
                    return string.IsNullOrEmpty(Data.CurrentClassFieldsCache[i].config_editor_setting.Display) ? name : Data.CurrentClassFieldsCache[i].config_editor_setting.Display;
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
//            t.SetID((config_current.ConfigList.Count == 0) ? 1 : (config_current.ConfigList.Max(i => i.GetID()) + 1));
//            Log(t.GetID().ToString());
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
            var editorName = EditorConfig.GetCodeGenConfig().SearchByOrderKey(codeGenID).EditorName;

            IMultipleWindow e = Data.CurrentAssembly.CreateInstance(editorName) as IMultipleWindow;
            return e;
        }

        /// <summary>
        /// 反射原始数据 TODO: 加入缓存来优化反射  --Complete
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <param name="raw"></param>
        public virtual void RenderRawColumn(ConfigEditorLineFieldCache column,ConfigEditorLineCache<T> lineData)
        {
            if (column.RawData == null) return;

            var columnSchema = column.CurrentSchema;
            var lineRawData = lineData.RawData;
            var columnRawData = column.RawData = columnSchema.field_info.GetValue(lineRawData);
          
            // ---- Chache ----

            if (column.IsGenericType)
            {
                GUILayout.BeginVertical(GUIStyle.none,
                    new GUILayoutOption[]
                    {
                        GUILayout.Width(GetResizeWidth(columnSchema.config_editor_setting.Width,
                            columnSchema.config_editor_setting.MaxWidth))
                    });

                int deleteIndex = -1;

                //Open Editor
                if (columnSchema.config_editor_setting.OutCodeGenEditorID != 0)
                {
                    //处理外联添加逻辑
                    var temp = columnRawData as List<int>;

                    for (int i = 0; i < temp.Count; i++)
                    {
                        GUILayout.BeginHorizontal();

                        //显示外联数据
                        GUILayout.Label(GetOutLinkDisplayField(
                                temp[i],
                                columnSchema.config_editor_setting.OutCodeGenEditorID,
                                columnSchema.config_editor_setting.OutLinkDisplay),
                            GUILayout.Width(
                                GetResizeWidth(
                                    columnSchema.config_editor_setting.Width,
                                    columnSchema.config_editor_setting.MaxWidth,
                                    Data.currentEditorSetting.KitButtonWidth + Data.currentEditorSetting.ColumnSpan
                                )
                            )
                        );

                        //删除子类型数据
                        if (GUILayout.Button(Language.Delete, GUILayout.Width(Data.currentEditorSetting.KitButtonWidth)))
                            deleteIndex = i;

                        GUILayout.EndHorizontal();
                    }
                    //处理外联添加逻辑

                    // -- 打开面板 --
                    if (GUILayout.Button(Language.Add))
                    {
                        //var test = data.config_editor_setting;
                        var e = OpenOutlinkWindow(columnSchema.config_editor_setting.OutCodeGenEditorID);
                        if (e == null)
                        {
                            ShowNotification(new GUIContent(Language.OutLinkIsNull));
                        }
                        else
                        {
                            e.UpdateSelectModel(columnRawData as List<int>, SetListItemValue);
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
                    //添加数据
                    if (GUILayout.Button(Language.Add))
                    {
                        column.Invoke_Add(
                            column.AttributeType == typeof(string)
                                ? string.Empty
                                : Activator.CreateInstance(column.AttributeType));
                    }

                    //添加数据
                    
                    
                    //处理数组逻辑
                    int count = column.Invoke_Count();

                    int removeIndex = -1;

                    for (int i = 0; i < count; i++)
                    {
                        object listItem = column.Invoke_GetItem(i); //.Item.GetValue(value, new object[] {i});


                        GUILayout.BeginHorizontal();
                        //alignment
                        RenderBaseControl(
                            GetResizeWidth(columnSchema.config_editor_setting.Width, columnSchema.config_editor_setting.MaxWidth,
                                Data.currentEditorSetting.KitButtonWidth + Data.currentEditorSetting.ColumnSpan),
                            columnSchema.config_editor_setting.CanEditor,
                            listItem,
                            v => { column.Invoke_SetItem(v,i); });

                        if (GUILayout.Button(Language.Delete,
                            new GUILayoutOption[] {GUILayout.Width(Data.currentEditorSetting.KitButtonWidth)}))
                        {
                            removeIndex = i;
                        }

                        GUILayout.EndHorizontal();
                    }

                    if (removeIndex != -1)
                    {
                        column.Invoke_RemoveAt(removeIndex);
                    }
                    //处理数组逻辑

                }

                GUILayout.EndVertical();
            }
            else
            {
                //Open Editor
                if (columnSchema.config_editor_setting.OutCodeGenEditorID != 0)
                {
//                    data.field_info.SetValue(raw,GetSelectValueByFlag(raw.ID, data.field_info.Name, (int) value));
                    
                    string buttonName = GetOutLinkDisplayField((int) columnRawData,
                        columnSchema.config_editor_setting.OutCodeGenEditorID,
                        columnSchema.config_editor_setting.OutLinkDisplay);

                    if (GUILayout.Button(buttonName,
                        new GUILayoutOption[]
                        {
                            GUILayout.Width(GetResizeWidth(columnSchema.config_editor_setting.Width,
                                columnSchema.config_editor_setting.MaxWidth))
                        }))
                    {
                        var e = OpenOutlinkWindow(columnSchema.config_editor_setting.OutCodeGenEditorID);

                        if (e == null)
                            ShowNotification(new GUIContent(Language.OutLinkIsNull));
                        else
                        {
                            e.UpdateSelectModel(lineRawData,columnSchema.field_info, SetListItemValue);
                            e.ShowUtility();
                        }
                    }
                }
                else
                {
                    RenderBaseControl(
                        GetResizeWidth(columnSchema.config_editor_setting.Width, columnSchema.config_editor_setting.MaxWidth),
                        columnSchema.config_editor_setting.CanEditor, columnRawData,
                        v => { columnSchema.field_info.SetValue(lineRawData, v);  });
                }
            }

            //更新search key
            if (column.CurrentSchema.CurrentFiledProperty == FiledProperty.SEARCH_KEY)
            {
                lineData.SearchKey = columnRawData.ToString();
            }
        }

        
        /// <summary>
        /// 这个函数应该是当前界面内的，当打开的界面有信息修改会调用此界面的回调函数
        /// </summary>
        /// <param name="item"></param>
        /// <param name="addValue"></param>
        public virtual void SetListItemValue(List<int> item, object addValue)
        {
//            var temp = item as List<int>;
//            var model = addValue as IModel;
//            temp.Add(model.ID);
//            item.Add(addValue.GetHashCode());
        }


        public virtual void SetListItemValue(object item,object addValue,FieldInfo field)
        {
//            field.SetValue(item,addValue.GetID());
//            ShowNotification(new GUIContent(string.Format(Language.SuccessAdd, addValue.GetComments())));//弹出消息提示  
        }

        /// <summary>
        /// 使用currentEditorSetting.isLog控制输出
        /// </summary>
        /// <param name="str"></param>
        public virtual void Log(string str)
        {
            if (Data.currentEditorSetting.isLog) Debug.Log(str);
        }


        //TODO: 增加Chache深度
        //TODO: 修改为依赖函数查找KEY
        /// <summary>
        /// 获取外链编辑器的按钮上显示的字符，未指定则显示ID 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="outLinkChacheKey"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public string GetOutLinkDisplayField(int id, int outLinkChacheKey, string field)
        {
            return string.Empty;
//            if (!outLinkRawData.ContainsKey(outLinkChacheKey))
//                return string.Empty;
//
//            if (string.IsNullOrEmpty(field)) field = "Comments";
//
//            object rawData = outLinkRawData[outLinkChacheKey];
//
//            var searchMethod = rawData.GetType().GetMethod("SearchByOrderKey");
//
//            var subData = searchMethod.Invoke(rawData, new object[] {id});
//
//            Log(string.Format("is Null ={0}  Field ={1}  id ={2}", subData == null, field, id.ToString()));
//
//            if (subData == null) return Language.Select;
//
//            //var tempDisplayObj = subData.GetType().GetField(field).GetValue(subData);
//            var tempDisplayObj = subData.GetType().GetField(field).GetValue(subData);
//
//            if (tempDisplayObj == null || string.IsNullOrEmpty(tempDisplayObj.ToString()))
//            {
//                field = "ID";
//                tempDisplayObj = subData.GetType().GetField(field).GetValue(subData);
//            }
//
//            string v = tempDisplayObj.ToString();
//
//            if (string.IsNullOrEmpty(v)) return Language.Select;
//
//            return v;
        }


        /// <summary>
        /// Reload 但不包括 SetConfigType 与 Initialize
        /// </summary>
        protected virtual void Reload()
        {
            AssetDatabase.Refresh();
            FieldsOrder = new Dictionary<string, bool>();

            var code_info = GetCodeGenInfo();//先获取加载配置信息 不然会debug很久的
            Data.config_current = ConfigContainerFactory.GetInstance(code_info.ContainerType).LoadConfig<ConfigBase<T>>(code_info.InOutPath);

            if (Data.config_current == null)
                Data.config_current = new ConfigBase<T>();
            
            
            deleteList.Clear();
            ReloadOutLinkChache();
            Data.SyncData();
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

            for (int i = 0; i < Data.CurrentClassFieldsCache.Count; i++)
            {
                //Bug fix: 自动做默认参数初始化 
                if (Data.CurrentClassFieldsCache[i].config_editor_setting == null)
                {
                    Data.CurrentClassFieldsCache[i].config_editor_setting = new ControlProperty();
                    continue;
                }

                if (outLinkRawData.ContainsKey(Data.CurrentClassFieldsCache[i].config_editor_setting.OutCodeGenEditorID))
                    continue;

                if (Data.CurrentClassFieldsCache[i].config_editor_setting.OutCodeGenEditorID == 0)
                    continue;


                //重写外联编辑器逻辑
                var codeGenInfo = EditorConfig.GetCodeGenConfig()
                    .SearchByOrderKey(Data.CurrentClassFieldsCache[i].config_editor_setting.OutCodeGenEditorID);

                if (codeGenInfo == null)
                {
                    Log("Can't find id In CodeGen Table" + Data.CurrentClassFieldsCache[i].config_editor_setting.OutCodeGenEditorID);
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

                outLinkRawData.Add(Data.CurrentClassFieldsCache[i].config_editor_setting.OutCodeGenEditorID, modelRaw);

                Log(string.Format("Loading OutLink Class [{0}] Data Success !", codeGenInfo.ClassType));

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
                    ShowNotification(new GUIContent(string.Format(Language.VerfiyMessageSuccess, data.GetHashCode())));

                if (ErrorLine.Contains(data.GetHashCode())) ErrorLine.Remove(data.GetHashCode()); //校验成功解除报错信息
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
            for (int i = 0; i < Data.config_current.ConfigList.Count; i++)
            {
                //跳过被删除的数据
                if (deleteList.Contains(Data.config_current.ConfigList[i].GetHashCode()))
                    continue;

                //保存错误信息
                string errorInfo = VerfiyLineData(Data.config_current.ConfigList[i], false);

                if (!string.IsNullOrEmpty(errorInfo))
                    sb.Append("\n").Append(errorInfo);
            }

            if (sb.Length == 0) return true;

            ShowNotification(new GUIContent(Language.PleaseCheckConsole));
            //格式化输出
            Debug.LogError(string.Format(Language.TableErrorInfoFormat, Data.currentEditorSetting.EditorTitle, sb));
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


            var temp = new List<T>();
            
            //处理删除逻辑
            for (int i = 0; i < deleteList.Count; i++)
            {
                
                foreach (var VARIABLE in Data.config_current.ConfigList)
                {
                    if (VARIABLE.GetHashCode() == deleteList[i])
                    {
                        temp.Add(VARIABLE);
                        break;
                    }
                }
            }

            for (int i = 0; i < temp.Count; i++)
            {
                Data.config_current.ConfigList.Remove(temp[i]);
            }

            deleteList.Clear();
            Data.SyncData();
            //处理删除逻辑


            //处理路径关系

            if (GetCodeGenInfo() == null)
            {
                ShowNotification(new GUIContent(Language.CantReadOutputPath));
                return;
            }

            var code_info = GetCodeGenInfo();//请注意这里 一定要先获取代码生成信息

            if (ConfigContainerFactory.GetInstance(code_info.ContainerType).SaveToDisk(code_info.InOutPath, Data.config_current))
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
            Data.currentEditorSetting.SearchResourceName =
                EditorGUILayout.TextField(Data.currentEditorSetting.SearchResourceName,
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
            {
                var lineData = CreateValue();
                Data.config_current.ConfigList.Add(lineData);
                Data.SyncData(lineData);
            }

            GUI.backgroundColor = Color.white;
            
           
        }

        /// <summary>
        /// 页头按钮
        /// </summary>
        /// <param name="field_name"></param>
        protected virtual void HeadButton_Click(string field_name)
        {
//            if (field_name == "ID")
//            {
//                if (GetFieldsOrder(field_name))
//                    config_current.ConfigList = config_current.ConfigList.OrderBy(x => x.GetID()).ToList();
//                else
//                    config_current.ConfigList = config_current.ConfigList.OrderByDescending(x => x.GetID()).ToList();
//            }
//
//            if (field_name == "NickName")
//            {
//                if (GetFieldsOrder(field_name))
//                    config_current.ConfigList = config_current.ConfigList.OrderBy(x => x.GetComments()).ToList();
//                else
//                    config_current.ConfigList = config_current.ConfigList.OrderByDescending(x => x.GetComments()).ToList();
//            }
        }

        /// <summary>
        /// 处理表头
        /// </summary>
        protected virtual void RenderHead()
        {
            GUILayout.BeginScrollView(new Vector2(posv.x, 0), false, false, GUIStyle.none, GUIStyle.none,
                new GUILayoutOption[] {GUILayout.Height(Data.currentEditorSetting.TableHeadHeight)});

            GUILayout.BeginHorizontal(EditorGUIStyle.GetGroupBoxStyle(),
                new GUILayoutOption[] {GUILayout.Width(position.width)});

            //TODO Set Order
            foreach (var item in Data.CurrentClassFieldsCache)
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

                GUILayout.Space(Data.currentEditorSetting.ColumnSpan);
            }

            RenderExtensionHead();

            if (current_windowType == WindowType.CALLBACK)
                GUILayout.Label(Language.Select, EditorGUIStyle.GetTagButtonStyle(),
                    GUILayout.Width(Data.currentEditorSetting.ExtensionHeadTagWith));
            else if (current_windowType == WindowType.INPUT)
                GUILayout.Label(Language.Operation, EditorGUIStyle.GetTagButtonStyle(),
                    GUILayout.Width(Data.currentEditorSetting.ExtensionHeadTagWith));
            else
                GUILayout.Label(Language.Operation, EditorGUIStyle.GetTagButtonStyle(),
                    GUILayout.Width(Data.currentEditorSetting.ExtensionHeadTagWith));

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

            if (!string.IsNullOrEmpty(Data.currentEditorSetting.SearchResourceName))
            {
                Data.currentEditorSetting.ItemMaxCount = Data.LinesCache.Count(x =>
                    x.SearchKey.ToLower().Contains(Data.currentEditorSetting.SearchResourceName.ToLower().Trim()));
                
                Finallylist = Data.LinesCache
                    .Where(x => x.SearchKey.ToLower().Contains(Data.currentEditorSetting.SearchResourceName.ToLower().Trim()))
                    .Skip(Data.currentEditorSetting.PageIndex * Data.currentEditorSetting.PageAmount)
                    .Take(Data.currentEditorSetting.PageAmount).ToList();
            }
            else
            {
                Data.currentEditorSetting.ItemMaxCount = Data.LinesCache.Count;
                Finallylist = Data.LinesCache
                    .Skip(Data.currentEditorSetting.PageIndex * Data.currentEditorSetting.PageAmount)
                    .Take(Data.currentEditorSetting.PageAmount).ToList();
            }

            //遍历搜索结果
            for (int i = 0; i < Finallylist.Count; i++)
            {
                var lineData = Finallylist[i];

                if (deleteList.Contains(lineData.HashCode))
                    continue;

                //Select effect diaplay
                if (current_windowType == WindowType.CALLBACK && SelectDisplayEffectList.Contains(lineData.HashCode))
                    GUI.backgroundColor = Color.green;
                else GUI.backgroundColor = Color.white;


                GUILayout.BeginHorizontal(EditorGUIStyle.GetGroupBoxStyle());

                foreach (var column in lineData.ColumnInfo)
                {
                    if (ErrorLine.Contains(lineData.HashCode)) GUI.color = Color.red;
                    RenderRawColumn(column,lineData);
                    GUI.color = Color.white;
                    
                    GUILayout.Space(Data.currentEditorSetting.ColumnSpan);
                }

                RenderExtensionButton(lineData.RawData);
                GUILayout.Space(Data.currentEditorSetting.ColumnSpan);

                RenderFunctionButton(lineData.RawData);
                GUILayout.Space(Data.currentEditorSetting.ColumnSpan);

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


            if (!Data.currentEditorSetting.DisableCreate && current_windowType != WindowType.CALLBACK)
                NewLineButton();

            if (!Data.currentEditorSetting.DisableSave && current_windowType != WindowType.CALLBACK)
                SaveButton();

            GUILayout.EndHorizontal();


            if (!Data.currentEditorSetting.DisableSearch)
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

            if (!initialized || Data == null)
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
            if (Data.currentEditorSetting == null)
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
        /// <param name="lineData"></param>
        protected virtual void RenderFunctionButton(T lineData)
        {
            if (current_windowType == WindowType.CALLBACK)
            {
                if (GUILayout.Button(Language.Select, GUI.skin.GetStyle("ButtonLeft"),
                    GUILayout.Width(Data.currentEditorSetting.KitButtonWidth * 2)))
                {
                    //要确认当前是何种模式
                    if (select_model == SelectModel.MUTI)
                    {
                        SelectDisplayEffectList.Add(lineData.GetHashCode());
                        select_callback(selector_raw_list, lineData);
                        ShowNotification(new GUIContent(string.Format(Language.SuccessAdd, JsonUtility.ToJson(lineData))));
                    }
                    else
                    {
                        single_select_callback(selector_raw_single, lineData , selector_raw_field_single);
                        Close();
                    }
                }

                if (GUILayout.Button(Language.Close, GUI.skin.GetStyle("ButtonRight"),
                    GUILayout.Width(Data.currentEditorSetting.KitButtonWidth * 2)))
                {
                    Close();
                    return;
                }
            }

            if (current_windowType != WindowType.CALLBACK)
            {
                if (GUILayout.Button(Language.Copy, GUI.skin.GetStyle("ButtonLeft"),
                    new GUILayoutOption[] {GUILayout.Width(Data.currentEditorSetting.KitButtonWidth)}))
                {
                    PasteItem = DeepClone(lineData);
                }

                if (GUILayout.Button(Language.Delete, GUI.skin.GetStyle("ButtonMid"),
                    new GUILayoutOption[] {GUILayout.Width(Data.currentEditorSetting.KitButtonWidth)}))
                {
                    deleteList.Add(lineData.GetHashCode());
                    ShowNotification(new GUIContent(Language.Success));
                }

                if (GUILayout.Button(Language.Verfiy, GUI.skin.GetStyle("ButtonMid"),
                    new GUILayoutOption[] {GUILayout.Width(Data.currentEditorSetting.KitButtonWidth)}))
                {
                    VerfiyLineData(lineData);
                }

                if (GUILayout.Button(Language.Paste, GUI.skin.GetStyle("ButtonRight"),
                    new GUILayoutOption[] {GUILayout.Width(Data.currentEditorSetting.KitButtonWidth)}))
                {
                    if (PasteItem != null)
                    {
                        //Bug fix : 从指定位置删除插入
                        var insertPos = Data.config_current.ConfigList.IndexOf(lineData);
                        Data.config_current.ConfigList.Remove(lineData);
                        //PasteItem.SetID(item.GetID());//修改为函数插入
                        Data.config_current.ConfigList.Insert(insertPos, DeepClone(PasteItem));
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
                Mathf.FloorToInt((Data.currentEditorSetting.ItemMaxCount - 1) / (float) Data.currentEditorSetting.PageAmount);
            if (maxIndex < Data.currentEditorSetting.PageIndex)
                Data.currentEditorSetting.PageIndex = 0;

            GUILayout.Label(string.Format(Language.PageInfoFormate, Data.currentEditorSetting.PageIndex + 1, maxIndex + 1),
                GUILayout.Width(100));
            GUILayout.Label(Language.OnePageMaxNumber, EditorGUIStyle.GetPageLabelGuiStyle(), GUILayout.Width(80));
            int.TryParse(GUILayout.TextField(Data.currentEditorSetting.PageAmount.ToString(), GUILayout.Width(40)),
                out Data.currentEditorSetting.PageAmount);


            if (GUILayout.Button(Language.Jump, EditorGUIStyle.GetJumpButtonGuiStyle(), GUILayout.Width(40)))
            {
                if (jumpTo - 1 < 0)
                    jumpTo = 0;

                if (jumpTo - 1 > maxIndex)
                    jumpTo = maxIndex;

                Data.currentEditorSetting.PageIndex = jumpTo - 1;
            }

            jumpTo = EditorGUILayout.IntField(jumpTo, GUILayout.Width(40));

            if (GUILayout.Button(Language.Previous, GUI.skin.GetStyle("ButtonLeft"), GUILayout.Height(16)))
            {
                if (Data.currentEditorSetting.PageIndex - 1 < 0)
                    Data.currentEditorSetting.PageIndex = 0;
                else
                    Data.currentEditorSetting.PageIndex -= 1;
            }

            if (GUILayout.Button(Language.Next, GUI.skin.GetStyle("ButtonRight"), GUILayout.Height(16)))
            {
                if (Data.currentEditorSetting.PageIndex + 1 > maxIndex)
                    Data.currentEditorSetting.PageIndex = maxIndex;
                else
                    Data.currentEditorSetting.PageIndex++;
            }

            //处理缩放
            if (!Data.currentEditorSetting.HideResizeSlider)
                Data.currentEditorSetting.Resize =
                    (int) GUILayout.HorizontalSlider(Data.currentEditorSetting.Resize, 1, 5, GUILayout.Width(70));

            GUILayout.EndHorizontal();
        }


        /// <summary>
        /// 计算缩放尺寸 目前只有两处调用  如果后面再有其他地方调用  需要重写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual int GetResizeWidth(int value, int max, int offset = 0)
        {
            if (Data.currentEditorSetting.HideResizeSlider)
            {
                return value;
            }
            else
            {
                return Mathf.Clamp(value * Data.currentEditorSetting.Resize - offset, 0, max - offset);
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
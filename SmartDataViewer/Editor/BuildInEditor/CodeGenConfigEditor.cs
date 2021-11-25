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
using UnityEngine;
using UnityEditor;
using System.IO;
using NUnit.Framework;
using SmartDataViewer.Helpers;
using SmartDataViewer;
using SmartDataViewer.Editor;
using SmartDataViewer.Editor.BuildInEditor;

namespace SmartDataViewer.Editor.BuildInEditor
{
    /// <summary>
    /// TODO: 待解决 代码生成器优化，model design之后顺便插入一条数据到model的 editor 上。进行定制
    /// </summary>
    public class CodeGenConfigEditor : ConfigEditorSchema<CodeGen>
    {
        [MenuItem("SmartDataViewer/Code Generator")]
        static public void OpenView()
        {
            CodeGenConfigEditor w = CreateInstance<CodeGenConfigEditor>();
            w.ShowUtility();
        }

        public override CodeGen GetCodeGenInfo()
        {
            return new CodeGen {InOutPath = EditorConfig.CodeGenFilePath};
        }

        protected override void RenderExtensionHead()
        {
            GUILayout.Label("GenCode", EditorGUIStyle.GetTagButtonStyle(),
                new GUILayoutOption[] {GUILayout.Width(Data.currentEditorSetting.ExtensionHeadTagWith)});
        }


        protected override string VerfiyLineData(CodeGen data, bool showNotification = true)
        {
            string error = "";
            
            
            if (string.IsNullOrEmpty(data.NickName))
            {
                error = string.Format("当前ID:{0} 建议NickName字段不要为空 请您补全描述信息便于查错", data.ID);
                if (!ErrorLine.Contains(data.ID)) ErrorLine.Add(data.ID); //添加报错提示

                if (showNotification)
                    ShowNotification(new GUIContent(error));
            }
            else if (GetType(data.ClassType) == null && ReflectionHelper.GetCurrentAssemblyType(data.ClassType) == null)
            {
                error = string.Format("当前ID:{0} {1} 找不到 请您确认代码中有这个类", data.ID, GetFieldDisplayFromChache("ClassType"));
                if (!ErrorLine.Contains(data.ID)) ErrorLine.Add(data.ID); //添加报错提示

                if (showNotification)
                    ShowNotification(new GUIContent(error));
            }
            else if (GetType(data.SubType) == null && ReflectionHelper.GetCurrentAssemblyType(data.SubType) == null)
            {
                error = string.Format("当前ID:{0} {1} 找不到 请您确认代码中有这个类", data.ID,GetFieldDisplayFromChache("SubType"));
                if (!ErrorLine.Contains(data.ID)) ErrorLine.Add(data.ID); //添加报错提示

                if (showNotification)
                    ShowNotification(new GUIContent(error));
            }
            else if (string.IsNullOrEmpty(data.CodeExportPath))
            {
                error = string.Format("当前ID:{0} {1}没写 麻烦你写下", data.ID,GetFieldDisplayFromChache("CodeExportPath"));
                if (!ErrorLine.Contains(data.ID)) ErrorLine.Add(data.ID); //添加报错提示

                if (showNotification)
                    ShowNotification(new GUIContent(error));
            }
            else if (string.IsNullOrEmpty(data.InOutPath))
            {
                error = string.Format("当前ID:{0} {1}没写 麻烦你写下", data.ID, GetFieldDisplayFromChache("InOutPath"));
                if (!ErrorLine.Contains(data.ID)) ErrorLine.Add(data.ID ); //添加报错提示

                if (showNotification)
                    ShowNotification(new GUIContent(error));
            }
            else if (string.IsNullOrEmpty(data.EditorPath))
            {
                error = string.Format("当前ID:{0} {1}没写 麻烦你写下", data.ID, GetFieldDisplayFromChache("EditorPath"));
                if (!ErrorLine.Contains(data.ID)) ErrorLine.Add(data.ID); //添加报错提示

                if (showNotification)
                    ShowNotification(new GUIContent(error));
            }
            else if (string.IsNullOrEmpty(data.EditorName))
            {
                error = string.Format("当前ID:{0} {1}没写 麻烦你写下", data.ID,GetFieldDisplayFromChache("EditorName"));
                if (!ErrorLine.Contains(data.ID)) ErrorLine.Add(data.ID); //添加报错提示

                if (showNotification)
                    ShowNotification(new GUIContent(error));
            }
            else if (data.InOutPath == Language.InOutPath)
            {
                error = string.Format("InOutPath需要文件名", data.ID,GetFieldDisplayFromChache("EditorName"));
                if (!ErrorLine.Contains(data.ID)) ErrorLine.Add(data.ID); //添加报错提示

                if (showNotification)
                    ShowNotification(new GUIContent(error));
            }
            else
            {
                if (showNotification)
                    ShowNotification(new GUIContent(string.Format(Language.VerfiyMessageSuccess, data.ID)));

                if (ErrorLine.Contains(data.ID)) ErrorLine.Remove(data.ID); //校验成功解除报错信息
            }

            return error;
        }

        protected override void RenderExtensionButton(CodeGen item)
        {
            string error = VerfiyLineData(item,false);
            if (string.IsNullOrEmpty(error))
            {
                string export_path = PathMapping.GetInstance().DecodePath(item.CodeExportPath);
                if (!Directory.Exists(export_path))
                    Directory.CreateDirectory(export_path);

                //检测代码是否已存在
                string path = Path.Combine(export_path, string.Format("{0}Export.cs", item.ClassType));
                string disPlay = Language.Build;
                if (File.Exists(path))
                {
                    GUI.color = Color.green;
                    disPlay = Language.ReBuild;
                }

                if (GUILayout.Button(disPlay,
                    new GUILayoutOption[] {GUILayout.Width(Data.currentEditorSetting.ExtensionHeadTagWith)}))
                {
                    SaveConfig();
                    WriteFile(item, path);
                }

                GUI.color = Color.white;
            }
            else
            {
                if (GUILayout.Button(EditorGUIStyle.LoadEditorResource<Texture2D>("warning.png"),
                    new GUILayoutOption[]
                        {GUILayout.Width(Data.currentEditorSetting.ExtensionHeadTagWith), GUILayout.Height(18)}))
                {
                    ShowNotification(new GUIContent(error));
                }
            }
        }

        string templateFile { get; set; }

        public override void Initialize()
        {
            base.Initialize();

            SetConfigType(new CodeGenConfig());

            string path = PathMapping.GetInstance()
                .DecodePath("{EDITOR}/Editor/CTS/EditorClassTemplate.unityjson"); // Build In 编辑器路径一律写死
            if (File.Exists(path))
                templateFile = File.ReadAllText(path);
        }

        public void WriteFile(CodeGen item, string path)
        {
            if (string.IsNullOrEmpty(templateFile)) return;
            string temp = templateFile;
            temp = temp.Replace("$[EDITOR_NAME]", item.EditorName);
            temp = temp.Replace("$[CLASS_TYPE]", item.ClassType);
            temp = temp.Replace("$[SUB_TYPE]", item.SubType);
            temp = temp.Replace("$[EDITOR_PATH]", item.EditorPath);
            temp = temp.Replace("$[CODE_GEN_ID]", item.ID.ToString());


            if (File.Exists(path)) File.Delete(path);

            File.WriteAllText(path, temp, System.Text.Encoding.UTF8);
            Debug.LogWarning(string.Format("Success Build !\n{0}", path));
            AssetDatabase.Refresh();
            Close();
        }
    }
}
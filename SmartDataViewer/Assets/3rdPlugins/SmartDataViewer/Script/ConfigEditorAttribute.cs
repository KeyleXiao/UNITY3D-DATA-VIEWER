//
//    Copyright 2017 KeyleXiao.
//    Contact to Me : Keyle_xiao@hotmail.com 
//
//   	Licensed under the Apache License, Version 2.0 (the "License");
//   	you may not use this file except in compliance with the License.
//   	You may obtain a copy of the License at
//
//   		http://www.apache.org/licenses/LICENSE-2.0
//
//   		Unless required by applicable law or agreed to in writing, software
//   		distributed under the License is distributed on an "AS IS" BASIS,
//   		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   		See the License for the specific language governing permissions and
//   		limitations under the License.
//

using System;

namespace SmartDataViewer
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class ConfigEditorAttribute : BaseConfigFiedEditorAttribute
    {
        //## 当前代码只在编辑器下使用
#if UNITY_EDITOR
        public DefaultEditorPropertity Setting { get; set; }


        public ConfigEditorAttribute()
        {
            Setting = new DefaultEditorPropertity();
            Setting.EditorTitle = string.Empty;
            Setting.OutputPath = string.Empty;
            Setting.LoadPath = string.Empty;
            Setting.DisableSave = false;
            Setting.DisableCreate = false;
            Setting.DisableSearch = false;
        }
#endif
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDataViewer.ConfigEditorAttribute"/> class.
        /// </summary>
        /// <param name="editor_title">当前编辑器显示的名词</param>
        /// <param name="load_path">当前编辑器数据文件的位置</param>
        /// <param name="output_path">编辑文件导出路径</param>
        /// <param name="disableSearch">是否禁用搜索栏</param>
        /// <param name="disableSave">是否禁用保存按钮</param>
        /// <param name="disableCreate">是否禁用添加按钮</param>
        public ConfigEditorAttribute(string editor_title = "",
            string load_path = "",
            string output_path = "",
            bool disableSearch = false,
            bool disableSave = false,
            bool disableCreate = false)
        {
            //## 当前代码只在编辑器下使用
#if UNITY_EDITOR
            Setting = new DefaultEditorPropertity();
            Setting.EditorTitle = editor_title;
            Setting.OutputPath = output_path;
            Setting.LoadPath = load_path;
            Setting.DisableSave = disableSave;
            Setting.DisableCreate = disableCreate;
            Setting.DisableSearch = disableSearch;
#endif
        }
    }
}
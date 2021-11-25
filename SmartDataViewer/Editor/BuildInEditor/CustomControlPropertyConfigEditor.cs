﻿//
//   		Copyright 2018 KeyleXiao.
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
using SmartDataViewer.Editor.BuildInEditor;
using UnityEditor;

namespace SmartDataViewer.Editor.BuildInEditor
{
    public class ControlPropertyConfigEditor : ConfigEditorSchema<ControlProperty>
    {
        [MenuItem("SmartDataViewer/Control Setting/Custom")]
        static public void OpenView()
        {
            var w = CreateInstance<ControlPropertyConfigEditor>();
            w.ShowUtility();
        }

        public override CodeGen GetCodeGenInfo()
        {
            return new CodeGen{ InOutPath = EditorConfig.CustomControlPropertyConfigPath };
        }
        
        public override ControlProperty CreateValue()
        {
            var r = base.CreateValue();
            return r;
        }

        public override void Initialize()
        {
            base.Initialize();
            SetConfigType(new CustomControlPropertyConfig());
        }

        protected override void SaveConfig()
        {
            base.SaveConfig();
            
            //刷新编辑器配置
            EditorConfig.GetCustomControlPropertyConfig(true);
            
        }
    }
}
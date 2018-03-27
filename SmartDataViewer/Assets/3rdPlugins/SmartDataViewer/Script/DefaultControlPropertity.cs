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


// ## 当前代码只在编辑器下使用
#if UNITY_EDITOR

using System;

namespace SmartDataViewer
{
    [Serializable]
    public class DefaultControlPropertity : IModel
    {
        public DefaultControlPropertity()
        {
            Display = string.Empty;
            OutLinkEditor = string.Empty;
            OutLinkSubClass = string.Empty;
            OutLinkClass = string.Empty;
            OutLinkDisplay = string.Empty;
            OutLinkFilePath = string.Empty;
            CanEditor = true;
        }

        public int Order;

        [ConfigEditorField(visibility: false)] public string Display;

        public bool CanEditor;

        public int Width;

        [ConfigEditorField(visibility: false)] public string OutLinkEditor;

        [ConfigEditorField(visibility: false)] public string OutLinkSubClass;

        [ConfigEditorField(visibility: false)] public string OutLinkClass;

        public bool Visibility;

        [ConfigEditorField(visibility: false)] public string OutLinkDisplay;

        [ConfigEditorField(visibility: false)] public string OutLinkFilePath;
    }
}

#endif
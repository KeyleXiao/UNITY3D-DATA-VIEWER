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
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public sealed class ConfigEditorFieldAttribute : BaseConfigFiedEditorAttribute
    {
        //## 当前代码只在编辑器下使用
#if UNITY_EDITOR
        public ControlPropertity Setting { get; private set; }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDataViewer.ConfigEditorFieldAttribute"/> class.
        /// </summary>
        /// <param name="order">编辑器字段显示顺序</param>
        /// <param name="can_editor">If set to <c>true</c> can editor.</param>
        /// <param name="display">编辑器中显示别名 不填为字段名</param>
        /// <param name="width">编辑器中显示的字段宽度</param>
        /// <param name="outLinkEditor">外联到新的编辑器</param>
        /// <param name="outLinkSubClass">外联到新的子类型,如果遵循编辑器默认命名规则 只需要填写此项即可</param>
        /// <param name="outLinkClass">外联到新的类型</param>
        /// <param name="visibility">是否在编辑器中隐藏此字段</param>
        /// <param name="outLinkDisplay">将显示外联数据的别名 默认显示外联数据的NickName如果没有则显示ID</param>
        /// <param name="outLinkFilePath">外联数据的文件位置</param>
        public ConfigEditorFieldAttribute(int order = 0, bool can_editor = true, string display = "",
            int width = 0, string outLinkEditor = "",
            string outLinkSubClass = "", string outLinkClass = "",
            bool visibility = true, string outLinkDisplay = "",
            string outLinkFilePath = ""
        )
        {
            //## 当前代码只在编辑器下使用
#if UNITY_EDITOR
            Setting = new ControlPropertity
            {
                Order = order,
                Display = display,
                CanEditor = can_editor,
                Width = width,
                OutLinkEditor = outLinkEditor,
                OutLinkSubClass = outLinkSubClass,
                OutLinkClass = outLinkClass,
                Visibility = visibility,
                OutLinkDisplay = outLinkDisplay,
                OutLinkFilePath = outLinkFilePath
            };

#endif
        }
    }
}
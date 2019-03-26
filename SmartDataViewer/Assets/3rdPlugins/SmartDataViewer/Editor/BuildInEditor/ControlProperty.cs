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

namespace SmartDataViewer
{
    [Serializable]
    [ConfigEditor(10005)]
    public class DefaultControlPropertyConfig : ConfigBase<ControlProperty>
    {
    }

    [Serializable]
    [ConfigEditor(10004)]
    public class CustomControlPropertyConfig : ConfigBase<ControlProperty>
    {
    }

    [Serializable]
    public class ControlProperty : IModel
    {
        public int GetOrderKey()
        {
            return ID;
        }

        public void SetOrderKey(int value)
        {
            ID = value;
        }
        public string GetComments()
        {
            return NickName;
        }
        
        [ConfigEditorField(11000)]
        public int ID;

        [ConfigEditorField(11001)] 
        public string NickName;
        
        public ControlProperty()
        {
            NickName = "";
            Display = string.Empty;
            OutLinkDisplay = string.Empty;
            CanEditor = true;
            Visibility = true;
            Width = 180;
            MaxWidth = 200;
            Linkage = string.Empty;
        }

        public string Linkage;

        public int Order;

        public string Display;

        public bool CanEditor;

        public int Width;

        public int MaxWidth;

        public bool Visibility;

        public string OutLinkDisplay;
        
        /// <summary>
        /// 直接关联到代码生成器
        /// </summary>
        [ConfigEditorField(11014)]
        public int OutCodeGenEditorID;

    }
}

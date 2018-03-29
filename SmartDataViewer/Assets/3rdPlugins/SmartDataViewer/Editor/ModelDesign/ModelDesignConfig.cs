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

using System.Collections.Generic;
using SmartDataViewer.Helpers;

namespace SmartDataViewer.Editor.ModelDesign
{
    /// <summary>
    /// 每个节点的配置文件 序列化保存到本地
    /// </summary>
    [System.Serializable] 
    public class ModelDesignConfig :ConfigBase<NodeFieldInfo>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        public ModelDesignConfig()
        {
//            EditorSetting = null;
//            Fields = new List<NodeFieldInfo>();
        }
        

//        public EditorPropertity EditorSetting { get; set; } //这个属性直接会写到配置目录里面去

    }


}
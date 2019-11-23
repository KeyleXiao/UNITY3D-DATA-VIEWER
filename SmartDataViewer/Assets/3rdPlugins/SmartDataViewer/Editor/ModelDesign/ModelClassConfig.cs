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
    /// 用于记录Class信息的实体类
    /// </summary>
    public class ModelClassConfig : ConfigBase<NodeFieldInfo>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        public ModelClassConfig()
        {
            Fields = new List<NodeFieldInfo>();
        }
        
        /// <summary>
        /// 当前实体类的字段
        /// </summary>
        public List<NodeFieldInfo> Fields { get; set; }
    }
    
    
    /// <summary>
    /// 节点内的字段信息
    /// </summary>
    [System.Serializable]
    public class NodeFieldInfo:IModel
    {
        public int GetID()
        {
            return ID;
        }
        public void SetID(int value)
        {
            ID = value;
        }

        public void SetComments(string value)
        {
            NickName = value;
        }

        public string GetComments()
        {
            return NickName;
        }
        
        [ConfigEditorField(11000)]
        public int ID;

        /// <summary>
        /// 注释
        /// </summary>
        [ConfigEditorField(11001)] 
        public string NickName;
        /// <summary>
        /// 默认配置
        /// </summary>
        public NodeFieldInfo()
        {
            IsPublic = true;
            IsSerializable = true;
            fieldType = ReflectionHelper.FieldType.STRING;
        }

        /// <summary>
        /// 生成的编辑器属性配置
        /// </summary>
        public ControlProperty controlSetting { get; set; }

        /// <summary>
        /// 当前字段属性
        /// </summary>
        public ReflectionHelper.FieldType fieldType { get; set; }

        /// <summary>
        /// 当前字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 是否可序列化
        /// </summary>
        public bool IsSerializable { get; set; }

        /// <summary>
        /// 描述文档
        /// </summary>
        public string Description { get; set; }
    }

}
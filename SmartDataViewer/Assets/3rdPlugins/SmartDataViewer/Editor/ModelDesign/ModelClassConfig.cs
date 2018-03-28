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
        public ControlPropertity controlSetting { get; set; }

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
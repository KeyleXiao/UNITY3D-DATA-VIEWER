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
            EditorSetting = null;
//            Fields = new List<NodeFieldInfo>();
        }
        

        public EditorPropertity EditorSetting { get; set; } //这个属性直接会写到配置目录里面去

    }


}
using System;

namespace SmartDataViewer.Editor.BuildInEditor
{
    /// <summary>
    /// 默认编辑器配置容器
    /// </summary>
    [System.Serializable]
    [ConfigEditor(10002)]
    public class DefaultEditorPropertyConfig : ConfigBase<EditorProperty>
    {
    }
    
    
    /// <summary>
    /// 默认编辑器配置容器
    /// </summary>
    [System.Serializable]
    [ConfigEditor(10003)]
    public class CustomEditorPropertyConfig : ConfigBase<EditorProperty>
    {
    }
    
    
    [Serializable]
    public class EditorProperty : IModel
    {
        public EditorProperty()
        {
            EditorTitle = string.Empty;
            SearchResourceName = string.Empty;
            NickName = "";
        }
        
        public void SetOrderKey(int value)
        {
            ID = value;
        }
        
        public int GetOrderKey()
        {
            return ID;
        }

        public string GetComments()
        {
            return NickName;
        }
        
        [ConfigEditorField(11000)]
        public int ID;

        [ConfigEditorField(11001)] 
        public string NickName;


        [ConfigEditorField(11008)]
        public string EditorTitle;

        [ConfigEditorField(11009)]
        public string SearchResourceName ;

        [ConfigEditorField(11002)]
        public bool DisableSearch;

        [ConfigEditorField(11003)]
        public bool DisableSave;

        [ConfigEditorField(11004)]
        public bool DisableCreate;
        
        [ConfigEditorField(11005)]
        public bool HideResizeSlider ;
        
        /// <summary>
        /// 扩展部分列宽
        /// </summary>
        
        public int ExtensionHeadTagWith = 88;

        /// <summary>
        /// 工具栏按钮统一宽度
        /// </summary>
        public int KitButtonWidth = 19;

        /// <summary>
        /// 测试工具
        /// </summary>
        public bool isLog = false;

        /// <summary>
        /// 设置列间间隔
        /// </summary>
        public int ColumnSpan = 4;

        /// <summary>
        /// 表头高度
        /// </summary>
        public int TableHeadHeight = 26;
        
        /// <summary>
        /// 缩放
        /// </summary>
        public int Resize = 1;
        
        /// <summary>
        /// 页上限
        /// </summary>
        public int ItemMaxCount ;
        
        
        /// <summary>
        /// 默认一页数量
        /// </summary>
        public int PageAmount = 50;

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex ;

        public int MaxResize = 5;

    }
}
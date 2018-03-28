using System;

namespace SmartDataViewer.Editor.BuildInEditor
{
    [Serializable]
    public class EditorProperty:IModel
    {
        public EditorProperty()
        {
            LoadPath = string.Empty;
            OutputPath = string.Empty;
            EditorTitle = string.Empty;
            SearchResourceName = string.Empty;
        }
        
        public string LoadPath;

        public string OutputPath;

        public string EditorTitle;

        public string SearchResourceName ;

        [ConfigEditorField(display:"禁搜索")] 
        public bool DisableSearch;

        [ConfigEditorField(display:"禁保存")]
        public bool DisableSave;

        [ConfigEditorField(display:"禁创建")]
        public bool DisableCreate;
        
        [ConfigEditorField(display:"禁缩放")]
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
    }
}
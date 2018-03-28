using UnityEditor;

namespace SmartDataViewer.Editor.BuildInEditor
{
    public class CustomEditorPropertyConfigEditor:ConfigEditorSchema<EditorProperty>
    {
        [MenuItem("SmartDataViewer/Editor Setting/Custom")]
        static public void OpenView()
        {
            var w = CreateInstance<CustomEditorPropertyConfigEditor>();
            w.ShowUtility();
        }

        public override EditorProperty CreateValue()
        {
            var r = base.CreateValue();
            return r;
        }

        public override void Initialize()
        {
            base.Initialize();
            SetConfigType(new CustomEditorPropertyConfig());
        }
    }
}
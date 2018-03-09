using UnityEditor;
using UnityEngine;

namespace SmartDataViewer.Editor
{
    public class EditorGUIStyle
    {
        private static Texture2D MainBg { get; set; }

        public static Texture2D GetMainBG()
        {
            if (!MainBg)
                MainBg = AssetDatabase.LoadAssetAtPath (PathHelper.GetEditorResourcePath("Background_dark.png"), typeof(Texture2D)) as Texture2D;
            return MainBg;
        }
    }
}
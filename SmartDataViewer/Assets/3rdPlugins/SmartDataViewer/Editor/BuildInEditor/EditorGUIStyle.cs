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
using SmartDataViewer.Helpers;
using UnityEditor;
using UnityEngine;

namespace SmartDataViewer.Editor
{
    public class EditorGUIStyle
    {
        private static Texture2D MainBg { get; set; }
        private static Texture2D TagBg { get; set ; }
        

        public static T LoadEditorResource<T>(string file_name_with_extension) where T: UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(PathHelper.GetEditorResourcePath(file_name_with_extension, PathHelper.PATH_TYPE.ASSETDATABASE));
        }

        private static GUIStyle tagButtonStyle;
        private static GUIStyle groupBoxStyle;
        private static GUIStyle horizontalScrollbarStyle;
        private static GUIStyle pageLabelGuiStyle;
        private static GUIStyle tableGroupBoxStyle;
        
        
        public static GUIStyle GetTagButtonStyle()
        {
            if (tagButtonStyle == null)
            {
                tagButtonStyle = new GUIStyle(GUI.skin.button);
                tagButtonStyle.padding = new RectOffset(4,0,4,2);
                tagButtonStyle.normal.background = LoadEditorResource<Texture2D>("HeadBg.jpg");
                tagButtonStyle.alignment = TextAnchor.MiddleLeft;
            }

            return tagButtonStyle;
        }
        
        public static GUIStyle GetPageLabelGuiStyle()
        {
            if (pageLabelGuiStyle == null)
            {
                pageLabelGuiStyle = new GUIStyle(GUI.skin.label);
                pageLabelGuiStyle.alignment = TextAnchor.MiddleRight;
            }
            return pageLabelGuiStyle;
        }
        


        public static GUIStyle GetGroupBoxStyle()
        {
            if (groupBoxStyle == null)
            {
                groupBoxStyle  = new GUIStyle(GUI.skin.box);
                groupBoxStyle.margin = new RectOffset(0,0,0,0);
                groupBoxStyle.padding = new RectOffset(0,0,0,0);
            }

            return groupBoxStyle ;
        }
        
        
        public static GUIStyle GetTableGroupBoxStyle()
        {
            if (tableGroupBoxStyle == null)
            {
                tableGroupBoxStyle  = new GUIStyle(GUI.skin.box);
                tableGroupBoxStyle.margin = new RectOffset(2,4,0,0);
                tableGroupBoxStyle.padding = new RectOffset(0,0,0,0);
            }

            return tableGroupBoxStyle ;
        }


        public static GUIStyle GetHorizontalScrollbarStyle()
        {
            if (horizontalScrollbarStyle == null)
            {
                horizontalScrollbarStyle = new GUIStyle(GUI.skin.horizontalScrollbar);
                horizontalScrollbarStyle.margin = new RectOffset(0,0,0,0);
                horizontalScrollbarStyle.padding = new RectOffset(0,0,0,0);
            }

            return horizontalScrollbarStyle;
        }

        public static void DrawLine(GUIStyle rLineStyle)
        {
            GUILayout.BeginHorizontal(rLineStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

    }
}
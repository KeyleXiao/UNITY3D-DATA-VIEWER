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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SmartDataViewer.Editor
{
    public class ExportWizard : EditorWindow
    {
        [MenuItem("SmartDataViewer/ExportWizard")]
        static public void OpenView()
        {
            var w = CreateInstance<ExportWizard>();

            w.minSize = new Vector2(350, 250);
            w.maxSize = w.minSize;
            Vector2 pos = new Vector2(Screen.width / 2 - w.minSize.x, Screen.height / 2 - w.minSize.y);
            w.position = new Rect(pos, w.minSize);

            w.ShowUtility();
        }


        static public void OpenView<T>(T obj)
        {
            var w = CreateInstance<ExportWizard>();

            w.minSize = new Vector2(350, 250);
            w.maxSize = w.minSize;
            Vector2 pos = new Vector2(Screen.width / 2, Screen.height / 2);
            w.position = new Rect(pos, w.minSize);

            w.ShowUtility();
        }

        private object rawData { get; set; }

        public void InitRawData(object data)
        {
            rawData = data;
        }

        float currentScrollViewWidth;
        bool resize = false;
        Rect cursorChangeRect;

        void OnEnable()
        {
            this.position = new Rect(200, 200, 400, 300);
            currentScrollViewWidth = this.position.width / 2;
            cursorChangeRect = new Rect(currentScrollViewWidth, 10, 4f, 25);
        }

        public void Callback(object obj)
        {
            Debug.Log("Selected: " + obj);
        }

        void OnGUI()
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("MenuItem1"), false, Callback, "item 1");
            menu.AddItem(new GUIContent("MenuItem2"), false, Callback, "item 2");
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("SubMenu/MenuItem3"), false, Callback, "item 3");
            menu.AddItem(new GUIContent("SubMenu/MenuItem4"), false, Callback, "item 4");
            menu.AddItem(new GUIContent("SubMenu/MenuItem5"), false, Callback, "item 5");
            menu.AddSeparator("SubMenu/");
            menu.AddItem(new GUIContent("SubMenu/MenuItem6"), false, Callback, "item 6");

            menu.ShowAsContext();

        }


        private void ResizeScrollView()
        {
            
            GUI.DrawTexture(cursorChangeRect,EditorGUIUtility.whiteTexture);
            
            EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeHorizontal);

            if (Event.current.type == EventType.MouseDown && cursorChangeRect.Contains(Event.current.mousePosition))
            {
                resize = true;
            }

            if (resize)
            {
                currentScrollViewWidth = Event.current.mousePosition.x;
                cursorChangeRect.Set(currentScrollViewWidth, cursorChangeRect.y, cursorChangeRect.width,
                    cursorChangeRect.height);
            }

            if (Event.current.type == EventType.MouseUp)
                resize = false;
        }
    }
}
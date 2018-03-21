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

using  UnityEditor;
using UnityEngine;

namespace SmartDataViewer.Editor
{
    
    public class ExportWizard:EditorWindow
    {
        [MenuItem("SmartDataViewer/ExportWizard")]
        static public void OpenView()
        {
            var w = CreateInstance<ExportWizard >();
            
            w.minSize = new Vector2(350,250);
            w.maxSize = w.minSize;
            Vector2 pos =new Vector2( Screen.width/2-w.minSize.x,Screen.height/2-w.minSize.y);
            w.position = new Rect(pos,w.minSize);
            
            w.ShowUtility();
        }


        static public void OpenView<T>(T obj)
        {
            var w = CreateInstance<ExportWizard >();
            
            w.minSize = new Vector2(350,250);
            w.maxSize = w.minSize;
            Vector2 pos =new Vector2( Screen.width/2,Screen.height/2);
            w.position = new Rect(pos,w.minSize);
            
            w.ShowUtility();
        }

        private object rawData { get; set; }

        public void InitRawData(object data)
        {
            rawData = data;
        }
        
        
        
    }
}
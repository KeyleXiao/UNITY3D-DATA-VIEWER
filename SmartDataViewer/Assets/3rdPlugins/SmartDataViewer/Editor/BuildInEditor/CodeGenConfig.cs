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

using System;
namespace SmartDataViewer.Editor.BuildInEditor
{
    [Serializable]
    [ConfigEditor(10001)]
    public class CodeGenConfig : ConfigBase<CodeGen> { }

    [Serializable]
    public class CodeGen : IModel
    {
        public CodeGen()
        {
            EditorName = string.Empty;
            ClassType = string.Empty;
            SubType = string.Empty;
            EditorPath = "CustomEditor";
        }
        
        [ConfigEditorField(11010)]
        public string EditorPath;
        [ConfigEditorField(11011)]
        public string EditorName;
        [ConfigEditorField(11012)]
        public string ClassType;
        [ConfigEditorField(11013)]
        public string SubType;
    }
}
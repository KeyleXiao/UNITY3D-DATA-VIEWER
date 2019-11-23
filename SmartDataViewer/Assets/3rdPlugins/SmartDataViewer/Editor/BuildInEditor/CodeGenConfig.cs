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
using UnityEngine;

namespace SmartDataViewer.Editor.BuildInEditor
{
    [Serializable]
    [ConfigEditor(10001)]
    public class CodeGenConfig : ConfigBase<CodeGen>
    {
        public CodeGen SearchByOrderKey(int id)
        {
            if (id == -1)
            {
                return new CodeGen{ID = -1,InOutPath = EditorConfig.CodeGenFilePath,ClassType = "SmartDataViewer.Editor.BuildInEditor.CodeGenConfig",SubType = "CodeGen"};
            }
            
            for (int i = 0; i < ConfigList.Count; i++)
            {
                if (ConfigList[i].ID == id)
                {
                    return ConfigList[i];
                }
            }
            return new CodeGen();
        }
        

        public virtual void Delete(int id)
        {
            int index = 0;
            for (var i = 0; i < ConfigList.Count; i++)
            {
                if (ConfigList[i].ID != id) continue;
                index = i;
                break;
            }

            ConfigList.RemoveAt(index);
        }

    }

    [Serializable]
    public class CodeGen : IModel
    {
        public int GetID()
        {
            return ID;
        }
        
        public void SetID(int value)
        {
            ID = value;
        }

        public void SetComments(string value)
        {
            NickName = value;
        }

        public string GetComments()
        {
            return NickName;
        }

        [ConfigEditorField(11000)]
        public int ID;

        [ConfigEditorField(11001)] 
        public string NickName;
        
        public CodeGen()
        {
            NickName = "";
            EditorName = string.Empty;
            ClassType = string.Empty;
            SubType = string.Empty;
            EditorPath = "CustomEditor";
            InOutPath = string.Empty;
            CodeExportPath ="{ROOT}/Editor/Export/";
            ContainerType = DataContainerType.EDITOR_UNITY_JSON;
        }

        [ConfigEditorField(11015)]
        public DataContainerType ContainerType;
        [ConfigEditorField(11006)]
        public string InOutPath;
        [ConfigEditorField(11010)]
        public string EditorPath;
        [ConfigEditorField(11011)]
        public string EditorName;
        [ConfigEditorField(11012)]
        public string ClassType;
        [ConfigEditorField(11013)]
        public string SubType;
        [ConfigEditorField(11016)]
        public string CodeExportPath;
    }
}
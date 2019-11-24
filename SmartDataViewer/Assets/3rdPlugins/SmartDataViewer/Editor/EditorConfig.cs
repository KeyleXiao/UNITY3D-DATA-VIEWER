//
//    Copyright 2017 KeyleXiao.
//    Contact to Me : Keyle_xiao@hotmail.com 
//
//   	Licensed under the Apache License, Version 2.0 (the "License");
//   	you may not use this file except in compliance with the License.
//   	You may obtain a copy of the License at
//
//   		http://www.apache.org/licenses/LICENSE-2.0
//
//   		Unless required by applicable law or agreed to in writing, software
//   		distributed under the License is distributed on an "AS IS" BASIS,
//   		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   		See the License for the specific language governing permissions and
//   		limitations under the License.

using System.Collections.Generic;
using SmartDataViewer.Editor.BuildInEditor;

namespace SmartDataViewer.Editor
{
    public enum WindowType
    {
        INPUT,
        CALLBACK
    }


    public class EditorConfig
    {
        public static readonly string CodeGenFilePath = "{EDITOR}/Editor/Config/CodeGen.unityjson";

        public static readonly string DefaultControlPropertyConfigPath =
            "{EDITOR}/Editor/Config/DefaultControlProperty.unityjson";

        public static readonly string DefaultEditorPropertyConfigPath =
            "{EDITOR}/Editor/Config/DefaultEditorPropertyConfig.unityjson";

        public static readonly string CustomControlPropertyConfigPath =
            "{EDITOR}/Editor/Config/CustomControlProperty.unityjson";

        public static readonly string CustomEditorPropertyConfigPath =
            "{EDITOR}/Editor/Config/CustomEditorPropertyConfig.unityjson";

//        这里不要chache了 实时读取        
//        private static DefaultControlPropertyConfig ConfigControlProperty {  get; set; }
//        private static DefaultEditorPropertyConfig ConfigEditorProperty {  get; set; }
//        private static CustomControlPropertyConfig ConfigCustomControlProperty {  get; set; }
//        private static CustomEditorPropertyConfig ConfigCustomEditorProperty {  get; set; }
//        private static CodeGenConfig ConfigCodeGen {  get; set; }


        public static CodeGenConfig GetCodeGenConfig(bool reload = false)
        {
//            if (ConfigCodeGen == null || reload)
//            {
//                ConfigCodeGen = ConfigContainerFactory.GetInstance().LoadConfig<CodeGenConfig>(CodeGenFilePath);
//            }
//
//            return ConfigCodeGen;
            return ConfigContainerFactory.GetInstance(DataContainerType.EDITOR_UNITY_JSON).LoadConfig<CodeGenConfig>(CodeGenFilePath);
        }

        public static CustomControlPropertyConfig GetCustomControlPropertyConfig(bool reload = false)
        {
//            if (ConfigCustomControlProperty == null || reload)
//            {
//                ConfigCustomControlProperty = ConfigContainerFactory.GetInstance().LoadConfig<CustomControlPropertyConfig>(CustomControlPropertyConfigPath);
//            }
//
//            return ConfigCustomControlProperty;
            return ConfigContainerFactory.GetInstance(DataContainerType.EDITOR_UNITY_JSON)
                .LoadConfig<CustomControlPropertyConfig>(CustomControlPropertyConfigPath);
        }

        public static CustomEditorPropertyConfig GetCustomEditorPropertyConfig(bool reload = false)
        {
//            if (ConfigCustomEditorProperty == null || reload)
//            {
//                ConfigCustomEditorProperty = ConfigContainerFactory.GetInstance()
//                    .LoadConfig<CustomEditorPropertyConfig>(CustomEditorPropertyConfigPath);
//            }
//
//            return ConfigCustomEditorProperty;
            return ConfigContainerFactory.GetInstance(DataContainerType.EDITOR_UNITY_JSON)
                .LoadConfig<CustomEditorPropertyConfig>(CustomEditorPropertyConfigPath);
        }

        public static DefaultControlPropertyConfig GetDefaultControlPropertyConfig(bool reload = false)
        {
//            if (ConfigControlProperty == null || reload)
//            {
//                ConfigControlProperty = ConfigContainerFactory.GetInstance()
//                    .LoadConfig<DefaultControlPropertyConfig>(DefaultControlPropertyConfigPath);
//            }
//
//            return ConfigControlProperty;
            return ConfigContainerFactory.GetInstance(DataContainerType.EDITOR_UNITY_JSON)
                .LoadConfig<DefaultControlPropertyConfig>(DefaultControlPropertyConfigPath);
        }

        public static DefaultEditorPropertyConfig GetDefaultEditorPropertyConfig(bool reload = false)
        {
//            if (ConfigEditorProperty == null || reload)
//            {
//                ConfigEditorProperty = ConfigContainerFactory.GetInstance()
//                    .LoadConfig<DefaultEditorPropertyConfig>(DefaultEditorPropertyConfigPath);
//            }
//
//            return ConfigEditorProperty;
            return ConfigContainerFactory.GetInstance(DataContainerType.EDITOR_UNITY_JSON)
                .LoadConfig<DefaultEditorPropertyConfig>(DefaultEditorPropertyConfigPath);
        }
    }

    public class Language
    {
        public static readonly string SaveFailed = "保存失败！";
        public static readonly string CantReadOutputPath = "读取不到输出路径，请检查类文件[ConfigEditor]特性是否配置";
        public static readonly string PleaseCheckConsole = "当前编辑器下有错误的逻辑数据 请您查看控制台错误信息 ";
        public static readonly string TableErrorInfoFormat = "编辑器:{0} 详细错误信息如下\n{1}";
        public static readonly string Close = "Close";
        public static readonly string Save = "Save";
        public static readonly string Jump = "Jump:";
        public static readonly string NewLine = "NewLine";
        public static readonly string DiscardChange = "Discard";
        public static readonly string Menu = "Menu";
        public static readonly string Build = "Build";
        public static readonly string ReBuild = "Rebuild";
        public static readonly string Select = "Select";
        public static readonly string Previous = "Previous";
        public static readonly string Next = "Next";
        public static readonly string Add = "Add";
        public static readonly string OutLinkIsNull = "Out link editor field is null";
        public static readonly string Success = "Success ..";
        public static readonly string SuccessAdd = "Success add {0}";
        public static readonly string NickName = @"NickName";
        public static readonly string Delete = @"X";
        public static readonly string Copy = @"C";
        public static readonly string Paste = @"P";
        public static readonly string Verfiy = @"V";
        public static readonly string VerfiyMessageSuccess = @"ID: {0} Is Verfied ！";
        public static readonly string Operation = @"Operation";
        public static readonly string Contract = @"Version 1.3.2 Beta  ";
        public static readonly string OnePageMaxNumber = "Max In Page";
        public static readonly string PageInfoFormate = @"Page |{0}|-|{1}|";
    }
}
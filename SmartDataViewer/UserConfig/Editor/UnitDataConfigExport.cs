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
using UnityEditor;
using SmartDataViewer.Editor;
using SmartDataViewer.Editor.BuildInEditor;

public class UnitDataViewer : ConfigEditorSchema<UnitData>
{
    public static int CODE_GEN_ID = 12;
    
	[MenuItem("SmartDataViewer/项目配置/UnitDataViewer")]
	static public void OpenView()
	{
		UnitDataViewer w = UnityEngine.ScriptableObject.CreateInstance<UnitDataViewer>();
		w.ShowUtility();
	}
	
    public override CodeGen GetCodeGenInfo()
    {
        return EditorConfig.GetCodeGenConfig().SearchByOrderKey(CODE_GEN_ID);
    }

	public override UnitData CreateValue()
	{
		UnitData r = base.CreateValue();
		return r;
	}

	public override void Initialize()
	{
		SetConfigType(new UnitDataConfig());
	}
}

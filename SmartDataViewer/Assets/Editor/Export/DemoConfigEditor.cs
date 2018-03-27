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
using SmartDataViewer.Editor.BuildInEditor;

public class DemoConfigEditor : ConfigEditorSchema<Demo>
{
	[MenuItem("CustomEditor/DemoConfigEditor")]
	static public void OpenView()
	{
		DemoConfigEditor w = CreateInstance<DemoConfigEditor>();
		w.ShowUtility();
	}

	public override Demo CreateValue()
	{
		Demo r = base.CreateValue();
		return r;
	}

	public override void Initialize()
	{
		SetConfigType(new DemoConfig());
	}
}

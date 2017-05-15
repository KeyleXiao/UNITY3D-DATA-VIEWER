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
using System.Collections.Generic;
using SmartDataViewer;

[ConfigEditor]
[Serializable]
public class MapBoxConfig : ConfigBase<MapBox>
{

}

[Serializable]
public class MapBox : IModel
{
	public MapBox()
	{
		MapResourceID = 0;
		GameSceneEvents = new List<int>();
		PointCharts = new List<int>();
		Description = string.Empty;
	}
	/// <summary>
	/// 关联资源表
	/// </summary>
	public int MapResourceID;

	/// <summary>
	/// 关联事件表
	/// </summary>
	public List<int> GameSceneEvents;

	/// <summary>
	/// 关联布点表
	/// </summary>
	[ConfigEditorField(can_editor: true, outLinkEditor: "PointChartConfigEditor", outLinkClass: "PointChart", Width = 120)]
	public List<int> PointCharts;

	[ConfigEditorField(can_editor: true)]
	public string Description;
}
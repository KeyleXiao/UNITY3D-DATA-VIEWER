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
using UnityEngine;

[ConfigEditor]
[Serializable]
public class PointChartConfig : ConfigBase<PointChart>
{

}

[Serializable]
public class PointChart : IModel
{
	public PointChart()
	{
		Points = new List<bool>();
		Description = string.Empty;
		PointColorList = new List<Color>();
		CurveList = new List<AnimationCurve>();
		Curve = new AnimationCurve();
		testBounds = new Bounds();
		BoundsList = new List<Bounds>();
	}




	[ConfigEditorField(can_editor: true)]
	public Vector2 testPoint;


	[ConfigEditorField(can_editor: true)]
	public List<bool> Points;



	public int MapResourceID;

	[ConfigEditorField(can_editor: true, Width = 200)]
	public Bounds testBounds;
	[ConfigEditorField(can_editor: true)]
	public Color PointColor;
	[ConfigEditorField(can_editor: true)]
	public AnimationCurve Curve;
	[ConfigEditorField(can_editor: true)]
	public List<Color> PointColorList;
	[ConfigEditorField(can_editor: true)]
	public List<AnimationCurve> CurveList;
	[ConfigEditorField(can_editor: true, Width = 200)]
	public List<Bounds> BoundsList;

	[ConfigEditorField(can_editor: true)]
	public string Description;
}

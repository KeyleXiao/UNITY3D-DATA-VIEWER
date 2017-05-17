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


	public Vector2 testPoint;


	public List<bool> Points;


	public int MapResourceID;

	public Bounds testBounds;

	public Color PointColor;

	public AnimationCurve Curve;

	public List<Color> PointColorList;


	public List<AnimationCurve> CurveList;

	public List<Bounds> BoundsList;

	public string Description;
}

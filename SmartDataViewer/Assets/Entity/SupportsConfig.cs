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

[Serializable]
public class SupportsConfig : ConfigBase<Supports> { }

[Serializable]
public class Supports : IModel
{
	public Supports()
	{
		boolList = new List<bool>();
		description = string.Empty;
		colorList = new List<Color>();
		curveList = new List<AnimationCurve>();
		curve = new AnimationCurve();
		bounds = new Bounds();
		boundsList = new List<Bounds>();
	}


	public Vector2 testPoint;

	public List<bool> boolList;

	public int testID;

	public Bounds bounds;

	public Color PointColor;

	public AnimationCurve curve;

	public List<Color> colorList;

	public List<AnimationCurve> curveList;

	public List<Bounds> boundsList;

	public string description;
}

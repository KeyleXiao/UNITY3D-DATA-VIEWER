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

[Serializable][ConfigEditor(1)]
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
		descriptionList = new List<string>();
		testPointlist = new List<Vector2>();
		testPointlist3 = new List<Vector3>();
		testPointlist4 = new List<Vector4>();
	}
	
	
	[ConfigEditorField(1)]
	public string description;
	
    [ConfigEditorField(2)]
	public Vector2 testPoint;
	
	[ConfigEditorField(3)]
	public Vector3 testPoint3;
	
	[ConfigEditorField(4)]
	public Vector4 testPoint4;
	

	[ConfigEditorField(5)]
	public List<bool> boolList;

	[ConfigEditorField(6)]
	public int testID;

	[ConfigEditorField(7)]
	public float testFloat;
	
	[ConfigEditorField(8)]
	public Bounds bounds;

	[ConfigEditorField(9)]
	public Color PointColor;

	[ConfigEditorField(10)]
	public AnimationCurve curve;

	
	[ConfigEditorField(11)]
	public List<string> descriptionList;
	
	
	[ConfigEditorField(12)]
	public List<Vector2> testPointlist;
	
	[ConfigEditorField(13)]
	public List<Vector3> testPointlist3;
	
	[ConfigEditorField(14)]
	public List<Vector4> testPointlist4;
	
	[ConfigEditorField(15)]
	public List<Color> colorList;

	[ConfigEditorField(16)]
	public List<AnimationCurve> curveList;

	[ConfigEditorField(17)]
	public List<Bounds> boundsList;

	[ConfigEditorField(18)]
	public List<float> floatList;


}

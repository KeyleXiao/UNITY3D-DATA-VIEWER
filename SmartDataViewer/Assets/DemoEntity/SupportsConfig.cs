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

[Serializable][ConfigEditor(editor_title:"属性支持一览")]
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
	
	
	[ConfigEditorField(display:"字符串")]
	public string description;
	
    [ConfigEditorField(display:"二维向量")]
	public Vector2 testPoint;
	
	[ConfigEditorField(display:"三维向量")]
	public Vector3 testPoint3;
	
	[ConfigEditorField(display:"四维向量")]
	public Vector4 testPoint4;
	

	[ConfigEditorField(display:"布尔数组")]
	public List<bool> boolList;

	[ConfigEditorField(display:"整形")]
	public int testID;

	[ConfigEditorField(display:"浮点")]
	public float testFloat;
	
	[ConfigEditorField(display:"Unity范围(Bounds)")]
	public Bounds bounds;

	[ConfigEditorField(display:"颜色")]
	public Color PointColor;

	[ConfigEditorField(display:"动画曲线")]
	public AnimationCurve curve;

	
	[ConfigEditorField(display:"字符串数组")]
	public List<string> descriptionList;
	
	
	[ConfigEditorField(display:"二维向量数组")]
	public List<Vector2> testPointlist;
	
	[ConfigEditorField(display:"三维向量数组")]
	public List<Vector3> testPointlist3;
	
	[ConfigEditorField(display:"四维向量数组")]
	public List<Vector4> testPointlist4;
	
	[ConfigEditorField(display:"颜色数组")]
	public List<Color> colorList;

	[ConfigEditorField(display:"动画曲线数组")]
	public List<AnimationCurve> curveList;

	[ConfigEditorField(display:"Unity范围(Bounds)数组")]
	public List<Bounds> boundsList;

	[ConfigEditorField(display:"浮点数组")]
	public List<float> floatList;


}

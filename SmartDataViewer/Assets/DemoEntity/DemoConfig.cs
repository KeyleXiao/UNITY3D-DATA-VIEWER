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
using ProtoBuf;
using SmartDataViewer;

[ProtoContract]  
[Serializable][ConfigEditor(2)]
public class DemoConfig : ConfigBase<Demo> { }

[ProtoContract]  
[Serializable]
public class Demo 
{
	public Demo()
	{
		strList = new List<string>();
		list = new List<int>();
		supports = new List<int>();
		description = string.Empty;
		NickName = string.Empty;
	}

	[ProtoMember(3)]
	public List<string> strList;

	[ProtoMember(4)]
	[ConfigEditorField(20)]
	public List<int> list;

	[ProtoMember(5)]
	[ConfigEditorField(19)]
	public List<int> supports;

	[ProtoMember(6)]
	public string description;

	[ProtoMember(7)]
	[ConfigEditorField(19)]
	public int support;

	[ProtoMember(8)]
	[ConfigEditorField(11000)]
	public int ID;

	[ProtoMember(9)]
	[ConfigEditorField(11001)] 
	public string NickName;
}
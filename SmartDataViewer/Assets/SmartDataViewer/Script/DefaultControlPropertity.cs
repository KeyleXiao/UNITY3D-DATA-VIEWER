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
namespace SmartDataViewer
{
	[Serializable]
	public class DefaultControlPropertity : IModel
	{
		public DefaultControlPropertity()
		{
			Display = string.Empty;
			OutLinkEditor = string.Empty;
			OutLinkSubClass = string.Empty;
			OutLinkDisplay = string.Empty;
		}
		public int Order;

		[ConfigEditorField(visibility: false)]
		public string Display;

		public bool CanEditor;

		public int Width;

		[ConfigEditorField(visibility: false)]
		public string OutLinkEditor;

		[ConfigEditorField(visibility: false)]
		public string OutLinkSubClass;

		public string OutLinkClass;

		[ConfigEditorField(visibility: true)]
		public bool Visibility;

		public string OutLinkDisplay;

		public string OutLinkFilePath;
	}
}

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartDataViewer
{
	[Serializable]
	public class IModel
	{
		public IModel() { }

		[ConfigEditorField(99, true)]
		public int ID;

		[ConfigEditorField(98, true)]
		public string NickName;
	}
}

using System;

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

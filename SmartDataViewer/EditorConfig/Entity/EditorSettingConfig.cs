namespace SmartDataViewer.Editor.BuildInEditor
{
	public class EditorSettingConfig : ConfigBase<EditorSetting>
	{
		
	}

	public class EditorSetting
	{
		public EditorSetting()
		{
			EditorGenID = 0;
		}

		public int EditorGenID { get; set; }
	}
}
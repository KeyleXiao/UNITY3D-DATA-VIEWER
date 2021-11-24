using System;
using System.Collections.Generic;
using SmartDataViewer;
using UnityEngine;

[Serializable]
public class ProjectSettingConfig : ConfigBase<ProjectSetting>
{
	public ProjectSetting GetoProjectSetting(int id = 1)
	{
		for (int i = 0; i < ConfigList.Count; i++)
		{
			if (ConfigList[i].ID == id)
			{
				return ConfigList[i];
			}
		}
		return null;
	}

}

[Serializable]
public class ProjectSetting
{
	public ProjectSetting()
	{
		SupportFileExtensions = new List<string>();
	}

	public int ID = 1;
	
	[SerializeField]
	public List<string> SupportFileExtensions;

	[SerializeField]
	public DataContainerType RES_FILE_TYPE = DataContainerType.EDITOR_UNITY_JSON;
}
using System;
using System.Collections.Generic;
using ProtoBuf;
using SmartDataViewer;
using UnityEditor;
using UnityEngine.Serialization;

[ProtoContract,Serializable]
public class ResMappingConfig : ConfigBase<ResMapping>
{
	private Dictionary<string, ResMapping> urlMapping = new Dictionary<string, ResMapping>();
	private Dictionary<string, ResMapping> guidMapping = new Dictionary<string, ResMapping>();
	private Dictionary<int, ResMapping> idMapping = new Dictionary<int, ResMapping>();
	private Dictionary<string, ResMapping> partMapping = new Dictionary<string, ResMapping>();

	public void ClearCache()
	{
		guidMapping.Clear();
		urlMapping.Clear();
		idMapping.Clear();
		partMapping.Clear();
	}
	
	public bool UsePartGetResInfo(string part, out ResMapping item)
	{
		if (partMapping.Count == 0)
		{
			for (int i = 0; i < ConfigList.Count; i++)
			{
				partMapping[ConfigList[i].part] = ConfigList[i];
			}
		}
		return partMapping.TryGetValue(part,out item);
	}
	
	public bool UseIDGetResInfo(int id, out ResMapping item)
	{
		if (idMapping.Count == 0)
		{
			for (int i = 0; i < ConfigList.Count; i++)
			{
				idMapping[ConfigList[i].ID] = ConfigList[i];
			}
		}
		return idMapping.TryGetValue(id,out item);
	}

	public bool UseGuidGetResInfo(string _guid, out ResMapping item)
	{
		if (guidMapping.Count == 0)
		{
			for (int i = 0; i < ConfigList.Count; i++)
			{
				guidMapping[ConfigList[i]._guid] = ConfigList[i];
			}
		}
		return guidMapping.TryGetValue(_guid,out item);
	}

	public bool UseUrlGetResInfo(string url,out ResMapping item)
	{
		if (urlMapping.Count == 0)
		{
			for (int i = 0; i < ConfigList.Count; i++)
			{
				urlMapping[ConfigList[i].url] = ConfigList[i];
			}
		}

		return urlMapping.TryGetValue(url,out item);
	}
}

/// <summary>
/// 资源映射
/// </summary>
[ProtoContract,Serializable]
public class ResMapping
{
	public override string ToString()
	{
		return $"ID:{ID} package:{package} url:{url} _guid{_guid}";
	}

	public ResMapping()
	{
		ID = 0;
		package = string.Empty;
		part = string.Empty;
		url = string.Empty;
		_guid = string.Empty;
	}
	[ProtoMember(1)]
	public int ID;
	/// <summary>
	/// 加载路径
	/// </summary>
	[ProtoMember(2)]
	[ConfigEditorField(250,FiledProperty.SEARCH_KEY)]
	public string url;
	/// <summary>
	/// 分包
	/// </summary>
	[ProtoMember(3)]
	[ConfigEditorField(250)]
	public string package;
	/// <summary>
	/// 部位
	/// </summary>
	[ProtoMember(4)]
	[ConfigEditorField(250)]
	public string part;

	/// <summary>
	/// 资源标志
	/// </summary>
	[FormerlySerializedAs("LocalIdentified")] public string _guid;
}
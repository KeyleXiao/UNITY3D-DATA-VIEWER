# UNITY3D-DATA-VIEWER
UNITY3D 可视化数据编辑器 / UNITY3D EDITOR DATA_VIEWER .

```
├── 3rdPlugins
│   └── SmartDataViewer  --主程序
│       ├── CTS          
│       ├── Config
│       ├── Editor
│       └── Script
├── DemoEntity           --测试用的实体类
└── Resources
    └── Config           --序列化文件导出路径 可修改

```

# Current Version 1.2.2 BETA

初次尝试请直接在UNITY菜单中选择 <code>SmartDataViewer/CodeGen -> build</code> 即可在工程中创建编辑器导出路径 <code>Editor/Export/</code> 。其中有已经成功生成的编辑器脚本。


# Next Version 

下个版本核心：
1. 优化编辑器编辑体验
2. 增加多总类的数据序列化，暂定 lua ,json(支持更多序列化插件),protobuf,YamlDotNet,Excel

敬请期待。

## Video on YouTube


[![Demo alpha](http://img.youtube.com/vi/cjk8dZT1TTU/0.jpg)](https://www.youtube.com/embed/cjk8dZT1TTU)


# 为什么要使用 SmartDataViewer ？
SmartDataViewer 节约程序大量编辑器开发时间，在定义完成基础类型的时候，即可同步生成可视化编辑器。

# 特性
1. 秒速生成编辑器
2. 支持外联多个类
3. 支持针对特定字段进行排序与查询
4. 支持数据导出导入(内建Unity3d Json支持)
5. 支持开放式编辑器事件,几乎所有的事件与组件渲染 用户可自定义
6. 通过标签定制编辑器字段显示方式,包括宽度，显示别名，显示排序，导出导入位置等


# 正确的打开姿势 / Tutorial
## 1.创建容器 / Create Container

Type 1
``` cs
[Serializable]
public class DemoConfig : ConfigBase<Demo> { }

[Serializable]
public class Demo : IModel
{
	public Demo()
	{
		strList = new List<string>();
		list = new List<int>();
		supports = new List<int>();
		description = string.Empty;
	}

	public List<string> strList;

	public List<int> list;

	[ConfigEditorField(outLinkSubClass: "Supports")]
	public List<int> supports;

	public string description;

	[ConfigEditorField(outLinkSubClass: "Supports")]
	public int support;
}
```
Type 2
``` cs
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

```

## 2.添加标签 / Add Attribute

### ConfigEditorAttribute
容器标签 / Container Attribute
``` cs
/// <summary>
/// Initializes a new instance of the <see cref="T:SmartDataViewer.ConfigEditorAttribute"/> class.
/// </summary>
/// <param name="editor_title">当前编辑器显示的名词</param>
/// <param name="load_path">当前编辑器数据文件的位置</param>
/// <param name="output_path">编辑文件导出路径</param>
/// <param name="disableSearch">是否禁用搜索栏</param>
/// <param name="disableSave">是否禁用保存按钮</param>
/// <param name="disableCreate">是否禁用添加按钮</param>
```

### ConfigEditorFieldAttribute 
字段标签 / Fields Attribute
``` cs
/// <summary>
/// Initializes a new instance of the <see cref="T:SmartDataViewer.ConfigEditorFieldAttribute"/> class.
/// </summary>
/// <param name="order">编辑器字段显示顺序</param>
/// <param name="can_editor">If set to <c>true</c> can editor.</param>
/// <param name="display">编辑器中显示别名 不填为字段名</param>
/// <param name="width">编辑器中显示的字段宽度</param>
/// <param name="outLinkEditor">外联到新的编辑器</param>
/// <param name="outLinkSubClass">外联到新的子类型,如果遵循编辑器默认命名规则 只需要填写此项即可</param>
/// <param name="outLinkClass">外联到新的类型</param>
/// <param name="visibility">是否在编辑器中隐藏此字段</param>
/// <param name="outLinkDisplay">将显示外联数据的别名 默认显示外联数据的NickName如果没有则显示ID</param>
/// <param name="outLinkFilePath">外联数据的文件位置</param>
```

## 3.生成代码 / Click Build Button
点击build按钮 则会在指定路径生成数据编辑器
![通过SmartDataViewer生成的编辑器](/A6153579-9537-404D-9007-CE9B85F69BBF.png)


## 完成 / Complete
![通过SmartDataViewer生成的编辑器](/F1CC3692-35AB-4E74-B030-5E8006171256.png)


# 如何加载数据？ ／ How to Load Data ?

``` cs
var c = SupportsConfig.LoadConfig<SupportsConfig>();
//var c = SupportsConfig.LoadConfig<SupportsConfig>("Special path");

c.DeleteFromDisk();
////c.DeleteFromDisk("Special path");

c.SaveToDisk();
//c.SaveToDisk("Special path");

var subType = c.SearchByID(1);

var subType2 = c.SearchByNickName("CommonNickName");
```

#### QQ群 'Game' ／QQ Group 'Game' : 137728654  
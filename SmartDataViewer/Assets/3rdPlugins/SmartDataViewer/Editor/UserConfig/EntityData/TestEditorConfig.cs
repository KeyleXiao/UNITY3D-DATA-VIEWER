using System;
using SmartDataViewer;
using ProtoBuf;

[ConfigEditor(40)]
[Serializable]
[ProtoContract]
public class TestEditorConfig : ConfigBase<TestEditor> { }


[ProtoContract,ProtoInclude(100,typeof(IModel)),Serializable]
public class TestEditor : IModel
{
    public TestEditor()
    {
        Label = "";
//        description = new List<string>();
//        key = new List<string>();
//        path = new List<string>();
    }

    
    [ProtoMember(23)]
    [ConfigEditorField(400)]
    public string Label;
    
//    [ProtoMember(24)]
//    [ConfigEditorField(1)]
//    public List<string> description;
//    
//    [ProtoMember(25)]
//    [ConfigEditorField(2)]
//    public List<string> key;
//    
//    [ProtoMember(26)]
//    [ConfigEditorField(5)]
//    public List<string> path;
}
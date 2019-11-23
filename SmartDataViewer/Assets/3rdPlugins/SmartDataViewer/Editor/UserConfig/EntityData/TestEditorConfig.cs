using System;
using SmartDataViewer;
using ProtoBuf;

[ConfigEditor(40)]
[Serializable]
[ProtoContract]
public class TestEditorConfig : ConfigBase<TestEditor> { }


[ProtoContract,Serializable]
public class TestEditor : IModel
{
    public TestEditor()
    {
        Label = "";
        NickName = "";
        ID = 0;
    }
    
    [ProtoMember(23)]
    public string Label;

    [ConfigEditorField(11000)]
    [ProtoMember(24)]
    public int ID;

    [ConfigEditorField(11001)] 
    [ProtoMember(25)]
    public string NickName;
}
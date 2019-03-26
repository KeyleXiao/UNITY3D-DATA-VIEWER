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
    }

    
    [ProtoMember(23)]
    public string Label;

    public int GetOrderKey()
    {
        return ID;
    }
    public void SetOrderKey(int value)
    {
        ID = value;
    }
    public string GetComments()
    {
        return NickName;
    }
        
    [ConfigEditorField(11000)]
    [ProtoMember(24)]
    public int ID;

    [ConfigEditorField(11001)] 
    [ProtoMember(25)]
    public string NickName;
}
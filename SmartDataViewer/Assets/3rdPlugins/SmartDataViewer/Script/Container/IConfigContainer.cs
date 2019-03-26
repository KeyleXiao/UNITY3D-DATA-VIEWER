using System;

namespace SmartDataViewer
{
    public interface IConfigContainer
    {
        object LoadConfig(Type t, string path);

        T LoadConfig<T>(string path);

        bool LoadText(string path, ref string content);

        bool DeleteFromDisk(string path);

        bool SaveToDisk(string path,object target);
    }
}
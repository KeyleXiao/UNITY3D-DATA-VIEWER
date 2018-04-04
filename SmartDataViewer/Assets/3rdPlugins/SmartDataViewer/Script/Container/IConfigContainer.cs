using System;

namespace SmartDataViewer
{
    public interface IConfigContainer
    {
        V LoadConfig<V>(string path) where V : new();

        object LoadConfig(Type t, string path);

        bool DeleteFromDisk(string path);

        bool SaveToDisk(string path,object target);
    }
}
using System;

namespace SmartDataViewer
{
    public interface IConfigContainer
    {
        object LoadConfig(Type t, string path);

        bool DeleteFromDisk(string path);

        bool SaveToDisk(string path,object target);
    }
}
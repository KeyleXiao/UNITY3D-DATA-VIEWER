using System;
using System.Threading.Tasks;

namespace SmartDataViewer
{
    public interface IConfigContainer
    {
        object LoadConfig(Type t, string path);

        T LoadConfig<T>(string path);
        
        Task<object> LoadConfigAsync(Type t, string path);

        Task<T> LoadConfigAsync<T>(string path);

        bool LoadText(string path, ref string content);

        bool DeleteFromDisk(string path);

        bool SaveToDisk(string path,object target);
    }
}
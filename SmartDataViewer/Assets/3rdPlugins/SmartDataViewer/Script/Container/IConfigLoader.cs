using System;

namespace SmartDataViewer
{
    public interface IConfigLoader
    {
        V LoadConfig<V>(string path) where V : new();

        object LoadConfig(Type t, string path);
    }
}
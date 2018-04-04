using System;
using System.Collections.Generic;

namespace SmartDataViewer
{
    public enum DataLoaderType
    {
        UNITY_JSON
    }
    

    public class ConfigLoaderFactory : IConfigLoader
    {
        /// <summary>
        /// 缓存加载
        /// </summary>
        private Dictionary<DataLoaderType, IConfigLoader> loader { get; set; }
        private DataLoaderType default_loader { get; set; }

        private ConfigLoaderFactory()
        {
            loader = new Dictionary<DataLoaderType, IConfigLoader>();
        }

        private static ConfigLoaderFactory instance { get; set; }

        public static ConfigLoaderFactory GetInstance(DataLoaderType loaderType = DataLoaderType.UNITY_JSON)
        {
            return (instance ?? (instance = new ConfigLoaderFactory())).SetLoader(loaderType);
        }



        public ConfigLoaderFactory SetLoader(DataLoaderType loaderType)
        {
            if (loader.ContainsKey(loaderType))
            {
                default_loader = loaderType;
                return this;
            }

            //---- 处理不同的配置加载逻辑 ----
            if (loaderType == DataLoaderType.UNITY_JSON) loader.Add(loaderType,new UnityJsonLoader());

            
            

            //---- 处理不同的配置加载逻辑 ----
            return this;
        }


        public V LoadConfig<V>(string path) where V : new()
        {
            return loader[default_loader].LoadConfig<V>(path);
        }

        public object LoadConfig(Type t, string path)
        {
            return loader[default_loader].LoadConfig(t,path);
        }
    }
}
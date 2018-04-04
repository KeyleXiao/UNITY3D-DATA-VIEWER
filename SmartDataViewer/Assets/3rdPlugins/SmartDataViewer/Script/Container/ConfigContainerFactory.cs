using System;
using System.Collections.Generic;

namespace SmartDataViewer
{
    public enum DataLoaderType
    {
        UNITY_JSON
    }
    

    public class ConfigContainerFactory : IConfigContainer
    {
        /// <summary>
        /// 缓存加载
        /// </summary>
        private Dictionary<DataLoaderType, IConfigContainer> loader { get; set; }
        private DataLoaderType default_loader { get; set; }

        private ConfigContainerFactory()
        {
            loader = new Dictionary<DataLoaderType, IConfigContainer>();
        }

        private static ConfigContainerFactory instance { get; set; }

        public static ConfigContainerFactory GetInstance(DataLoaderType loaderType = DataLoaderType.UNITY_JSON)
        {
            return (instance ?? (instance = new ConfigContainerFactory())).SetLoader(loaderType);
        }



        public ConfigContainerFactory SetLoader(DataLoaderType loaderType)
        {
            if (loader.ContainsKey(loaderType))
            {
                default_loader = loaderType;
                return this;
            }

            //---- 处理不同的配置加载逻辑 ----
            if (loaderType == DataLoaderType.UNITY_JSON) loader.Add(loaderType,new UnityJsonContainer());

            
            

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
        
        public  bool DeleteFromDisk(string path)
        {
            return loader[default_loader].DeleteFromDisk(path);
        }

        public  bool SaveToDisk(string path,object target)
        {
            return loader[default_loader].SaveToDisk(path,target);
        }
    }
}
using System;
using System.Collections.Generic;

namespace SmartDataViewer
{
    public enum DataContainerType
    {
        UNITY_JSON,
        PROTOBUF
    }
    

    public class ConfigContainerFactory : IConfigContainer
    {
        /// <summary>
        /// 缓存加载
        /// </summary>
        private Dictionary<DataContainerType, IConfigContainer> loader { get; set; }
        private DataContainerType DefaultContainer { get; set; }

        private ConfigContainerFactory()
        {
            loader = new Dictionary<DataContainerType, IConfigContainer>();
        }

        private static ConfigContainerFactory instance { get; set; }

        public static ConfigContainerFactory GetInstance(DataContainerType containerType = DataContainerType.UNITY_JSON)
        {
            return (instance ?? (instance = new ConfigContainerFactory())).SetLoader(containerType);
        }



        public ConfigContainerFactory SetLoader(DataContainerType containerType)
        {
            if (loader.ContainsKey(containerType))
            {
                DefaultContainer = containerType;
                return this;
            }

            //---- 处理不同的配置加载逻辑 ----
            if (containerType == DataContainerType.UNITY_JSON) loader.Add(containerType,new UnityJsonContainer());
            if (containerType == DataContainerType.PROTOBUF) loader.Add(containerType,new ProtobufContainer());
            
            
            

            //---- 处理不同的配置加载逻辑 ----
            return this;
        }


        /// <summary>
        /// 建议使用 LoadConfig(Type t, string path) 函数
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public V LoadConfig<V>(string path) 
        {
            return (V)loader[DefaultContainer].LoadConfig(typeof(V),path);
        }

        public object LoadConfig(Type t, string path)
        {
            return loader[DefaultContainer].LoadConfig(t,path);
        }
        
        public  bool DeleteFromDisk(string path)
        {
            return loader[DefaultContainer].DeleteFromDisk(path);
        }

        public  bool SaveToDisk(string path,object target)
        {
            return loader[DefaultContainer].SaveToDisk(path,target);
        }
    }
}
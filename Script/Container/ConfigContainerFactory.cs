using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SmartDataViewer
{
    public enum DataContainerType
    {
        EDITOR_UNITY_JSON,
        EDITOR_PROTOBUF,
        RUNTIME_UNITY_JSON,
        RUNTIME_PROTOBUF,
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
        
        /// <summary>
        /// 如果可以不要new多个当前工厂，在PB加载下出现多个实例会报错的
        /// </summary>
        /// <param name="containerType">加载类型</param>
        public ConfigContainerFactory(DataContainerType containerType = DataContainerType.EDITOR_UNITY_JSON)
        {
            loader = new Dictionary<DataContainerType, IConfigContainer>();
            SetLoader(containerType);
        }

        private static ConfigContainerFactory instance { get; set; }

        public static ConfigContainerFactory GetInstance(DataContainerType containerType)
        {
            return (instance ?? (instance = new ConfigContainerFactory())).SetLoader(containerType);
        }

        public ConfigContainerFactory SetLoader(DataContainerType containerType)
        {
            DefaultContainer = containerType;
            
            if (loader.ContainsKey(DefaultContainer)) return this;

            //---- 处理不同的配置加载逻辑 ----
            
            #if UNITY_EDITOR
            if (DefaultContainer == DataContainerType.EDITOR_UNITY_JSON) loader.Add(DefaultContainer,new UnityJsonContainer());
            if (DefaultContainer == DataContainerType.EDITOR_PROTOBUF) loader.Add(DefaultContainer,new ProtobufContainer());
            #endif

            
            if (DefaultContainer == DataContainerType.RUNTIME_UNITY_JSON) loader.Add(DefaultContainer,new RuntimeUnityJsonContainer());
            //运行时Protobuf解析
            if (DefaultContainer == DataContainerType.RUNTIME_PROTOBUF) loader.Add(DefaultContainer,new RuntimeProtobufContainer());

            //---- 处理不同的配置加载逻辑 ----
            return this;
        }


        /// <summary>
        /// 加载配置 （建议使用）
        /// </summary>
        /// <param name="content"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public T LoadConfig<T>(string content)
        {
            return loader[DefaultContainer].LoadConfig<T>(content);
        }

        public Task<object> LoadConfigAsync(Type t, string path)
        {
            return loader[DefaultContainer].LoadConfigAsync(t,path);
        }

        public Task<T> LoadConfigAsync<T>(string path)
        {
            return loader[DefaultContainer].LoadConfigAsync<T>(path);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="t"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public object LoadConfig(Type t, string content)
        {
            return loader[DefaultContainer].LoadConfig(t,content);
        }
        
        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public  bool DeleteFromDisk(string content)
        {
            return loader[DefaultContainer].DeleteFromDisk(content);
        }

        /// <summary>
        /// 保存到本地
        /// </summary>
        /// <param name="content"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool SaveToDisk(string content,object target)
        {
            return loader[DefaultContainer].SaveToDisk(content,target);
        }

        /// <summary>
        /// 普通文本加载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool LoadText(string path, ref string content)
        {
            return loader[DefaultContainer].LoadText(path,ref content);
        }
    }
}
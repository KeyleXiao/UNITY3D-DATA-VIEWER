//
//   		Copyright 2017 KeyleXiao.
//     		Contact : Keyle_xiao@hotmail.com 
//
//     		Licensed under the Apache License, Version 2.0 (the "License");
//     		you may not use this file except in compliance with the License.
//     		You may obtain a copy of the License at
//
//     		http://www.apache.org/licenses/LICENSE-2.0
//
//     		Unless required by applicable law or agreed to in writing, software
//     		distributed under the License is distributed on an "AS IS" BASIS,
//     		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     		See the License for the specific language governing permissions and
//     		limitations under the License.
//

using System;
using System.Collections.Generic;
using SmartDataViewer.Helpers;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using ProtoBuf;

namespace SmartDataViewer
{
    [Serializable]
    [DataContract]
    public class ConfigBase<T> where T : new()
    {
        public ConfigBase()
        {
            ConfigList = new List<T>();
        }

        
        [ProtoMember(10000)]
        public List<T> ConfigList;

        /// <summary>
        /// 添加格式化统一调用
        /// </summary>
        public virtual void Format()
        {
        }
    }
}
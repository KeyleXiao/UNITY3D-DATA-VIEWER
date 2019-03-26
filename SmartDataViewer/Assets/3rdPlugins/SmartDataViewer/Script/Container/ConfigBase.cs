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
using ProtoBuf;

namespace SmartDataViewer
{
    [Serializable]
    [DataContract]
    public class ConfigBase<T> where T : IModel
    {
        public ConfigBase()
        {
            ConfigList = new List<T>();
        }

        
        [ProtoMember(10000)]
        public List<T> ConfigList;

        public virtual T SearchByID(int id)
        {
            for (int i = 0; i < ConfigList.Count; i++)
            {
                if (ConfigList[i].ID == id)
                {
                    return ConfigList[i];
                }
            }

            return null;
        }

        public virtual T SearchByNickName(string nickname)
        {
            for (int i = 0; i < ConfigList.Count; i++)
            {
                if (ConfigList[i].NickName == nickname)
                {
                    return ConfigList[i];
                }
            }

            return null;
        }


        public virtual void Delete(int id)
        {
            int index = 0;
            for (var i = 0; i < ConfigList.Count; i++)
            {
                if (ConfigList[i].ID != id) continue;
                index = i;
                break;
            }

            ConfigList.RemoveAt(index);
        }
        
        

    }
}
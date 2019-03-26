//
//    Copyright 2017 KeyleXiao.
//    Contact to Me : Keyle_xiao@hotmail.com 
//
//   	Licensed under the Apache License, Version 2.0 (the "License");
//   	you may not use this file except in compliance with the License.
//   	You may obtain a copy of the License at
//
//   		http://www.apache.org/licenses/LICENSE-2.0
//
//   		Unless required by applicable law or agreed to in writing, software
//   		distributed under the License is distributed on an "AS IS" BASIS,
//   		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   		See the License for the specific language governing permissions and
//   		limitations under the License.
//

using System;

namespace SmartDataViewer
{
    public class BaseAttribute : Attribute
    {
        public static T GetCurrentAttribute<T>(object obj, bool inherited = true)
        {
            object[] o = obj.GetType().GetCustomAttributes(typeof(T), inherited);
            if (o == null || o.Length < 1)
            {
                return default(T);
            }

            return ((T) o[0]);
        }
    }
}
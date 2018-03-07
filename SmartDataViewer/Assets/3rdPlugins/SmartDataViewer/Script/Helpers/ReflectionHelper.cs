using System;
using System.Linq;
using System.Reflection;

namespace SmartDataViewer.Helpers
{
    public class ReflectionHelper
    {
        /// <summary>
        /// Determines if rType was derived from rBaseType (does NOT work with interfaces)
        /// </summary>
        /// <param name="rType">Type that is being tested</param>
        /// <param name="rBaseType">Base type that rType inherited from</param>
        /// <returns></returns>
        public static bool IsSubclassOf(Type rType, Type rBaseType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            if (rType == rBaseType || rBaseType.GetTypeInfo().IsAssignableFrom(rType.GetTypeInfo())) { return true; }
#else
            if (rType == rBaseType || rType.IsSubclassOf(rBaseType)) { return true; }
#endif
                return false;
        }

        /// <summary>
        /// Determines if rType was derived from rBaseType (works with interfaces)
        /// </summary>
        /// <param name="rType">Base Type rDerivedType inherited from</param>
        /// <param name="rDerivedType">Derived type that was derived from rType</param>
        /// <returns></returns>
        public static bool IsAssignableFrom(Type rType, Type rDerivedType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            if (rType == rDerivedType || rType.GetTypeInfo().IsAssignableFrom(rDerivedType.GetTypeInfo())) { return true; }
#else
            if (rType == rDerivedType || rType.IsAssignableFrom(rDerivedType)) { return true; }
#endif
            return false;
        }

        /// <summary>
        /// Grabs an attribute from the class type and returns it
        /// </summary>
        /// <param name="rObjectType">Object type who has the attribute value</param>
        public static T GetAttribute<T>(Type rObjectType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            System.Collections.Generic.IEnumerable<System.Attribute> lInitialAttributes = rObjectType.GetTypeInfo().GetCustomAttributes(typeof(T), true);
            object[] lAttributes = lInitialAttributes.ToArray();
#else
            object[] lAttributes = rObjectType.GetCustomAttributes(typeof(T), true);
#endif

            if (lAttributes == null || lAttributes.Length == 0) { return default(T); }

            return (T)lAttributes[0];           
        }

        /// <summary>
        /// Determines if the specified attribute is defined for the info
        /// </summary>
        /// <param name=""></param>
        /// <param name="rObjectType"></param>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsDefined(Type rObjectType, Type rType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            System.Collections.Generic.IEnumerable<System.Attribute> lInitialAttributes = rObjectType.GetTypeInfo().GetCustomAttributes(rType, true);
            object[] lAttributes = lInitialAttributes.ToArray();
#else
            object[] lAttributes = rObjectType.GetCustomAttributes(rType, true);
#endif

            if (lAttributes != null && lAttributes.Length > 0) { return true; }

            return false;
        }

        /// <summary>
        /// Determines if the specified attribute is defined for the info
        /// </summary>
        /// <param name=""></param>
        /// <param name="rFieldInfo"></param>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsDefined(FieldInfo rFieldInfo, Type rType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            System.Collections.Generic.IEnumerable<System.Attribute> lInitialAttributes = rFieldInfo.GetCustomAttributes(rType, true);
            object[] lAttributes = lInitialAttributes.ToArray();
#else
            object[] lAttributes = rFieldInfo.GetCustomAttributes(rType, true);
#endif

            if (lAttributes != null && lAttributes.Length > 0) { return true; }

            return false;
        }

        /// <summary>
        /// Determines if the specified attribute is defined for the info
        /// </summary>
        /// <param name=""></param>
        /// <param name="rMemberInfo"></param>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsDefined(MemberInfo rMemberInfo, Type rType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            System.Collections.Generic.IEnumerable<System.Attribute> lInitialAttributes = rMemberInfo.GetCustomAttributes(rType, true);
            object[] lAttributes = lInitialAttributes.ToArray();
#else
            object[] lAttributes = rMemberInfo.GetCustomAttributes(rType, true);
#endif

            if (lAttributes != null && lAttributes.Length > 0) { return true; }

            return false;
        }

        /// <summary>
        /// Determines if the specified attribute is defined for the info
        /// </summary>
        /// <param name=""></param>
        /// <param name="rPropertyInfo"></param>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsDefined(PropertyInfo rPropertyInfo, Type rType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            System.Collections.Generic.IEnumerable<System.Attribute> lInitialAttributes = rPropertyInfo.GetCustomAttributes(rType, true);
            object[] lAttributes = lInitialAttributes.ToArray();
#else
            object[] lAttributes = rPropertyInfo.GetCustomAttributes(rType, true);
#endif

            if (lAttributes != null && lAttributes.Length > 0) { return true; }

            return false;
        }

        /// <summary>
        /// Sets the property value if the property exists
        /// </summary>
        public static void SetProperty(object rObject, string rName, object rValue)
        {
            Type lType = rObject.GetType();
            PropertyInfo[] lProperties = lType.GetProperties();
            if (lProperties != null && lProperties.Length > 0)
            {
                for (int i = 0; i < lProperties.Length; i++)
                {
                    if (lProperties[i].Name == rName && lProperties[i].CanWrite)
                    {
                        lProperties[i].SetValue(rObject, rValue, null);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the specified type exists
        /// </summary>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsTypeValid(string rType)
        {
            try
            {
                Type lType = Type.GetType(rType);
                return (lType != null);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the type is a primitive type
        /// </summary>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsPrimitive(Type rType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            return rType.GetTypeInfo().IsPrimitive;
#else
            return rType.IsPrimitive;
#endif
        }

        /// <summary>
        /// Determines if the type is a primitive type
        /// </summary>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsValueType(Type rType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            return rType.GetTypeInfo().IsValueType;
#else
            return rType.IsValueType;
#endif
        }

        /// <summary>
        /// Determines if the type is a generic type
        /// </summary>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsGenericType(Type rType)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            return rType.GetTypeInfo().IsGenericType;
#else
            return rType.IsGenericType;
#endif
        }

        /// <summary>
        /// Grabs the default value for the specified type
        /// </summary>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static object GetDefaultValue(Type rType)
        {
            bool lIsValueType = false ;

#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            lIsValueType = rType.GetTypeInfo().IsValueType;
#else
            lIsValueType = rType.IsValueType;
#endif

            if (lIsValueType)
            {
                return Activator.CreateInstance(rType);
            }
            else
            {
                UnityEngine.Vector3 lDummy = new UnityEngine.Vector3();
                return lDummy.GetType().GetMethod("GetDefaultGeneric").MakeGenericMethod(rType).Invoke(lDummy, null);
            }
        }
    }
}

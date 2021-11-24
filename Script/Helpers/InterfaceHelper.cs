using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace SmartDataViewer.Helpers
{
    /// <summary>
    /// Used to help manage interfaces. In this case, we're using it to help find
    /// monobehaviors that implement interfaces
    /// </summary>
    public static class InterfaceHelper
    {
        /// <summary>
        /// Hold all the types that implement the interface
        /// </summary>
        private static Dictionary<Type, Type[]> mInterfaceTypes;

        /// <summary>
        /// Constructor for class
        /// </summary>
        static InterfaceHelper()
        {
            mInterfaceTypes = new Dictionary<Type, Type[]>();
        }

        /// <summary>
        /// Grab all the component instances that derive from the interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetComponents<T>()
        {
            List<T> lInterfaces = new List<T>();

            Object[] lComponents = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
            for (int i = 0; i < lComponents.Length; i++)
            {
                if (lComponents[i] is T)
                {
                    T lComponent = (T)((object)lComponents[i]);
                    lInterfaces.Add(lComponent);
                }
            }

            return lInterfaces.ToArray();
        }

        /// <summary>
        /// Grab all the component instances that derive from the interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetComponents<T>(GameObject rGameObject)
        {
            List<T> lInterfaces = new List<T>();

            Component[] lComponents = rGameObject.GetComponents(typeof(MonoBehaviour));
            for (int i = 0; i < lComponents.Length; i++)
            {
                if (lComponents[i] is T)
                {
                    T lComponent = (T)((object)lComponents[i]);
                    lInterfaces.Add(lComponent);
                }
            }

            return lInterfaces.ToArray();
        }

        /// <summary>
        /// Grab all the component instances that derive from the interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetComponent<T>(GameObject rGameObject)
        {
            if (rGameObject == null) { return default(T); }

            Type lInterfaceType = typeof(T);

            Type[] lTypes = GetInterfaceTypes(lInterfaceType);
            if (lTypes == null || lTypes.Length == 0) { return default(T); }

            for (int i = 0; i < lTypes.Length; i++)
            {
                Type lType = lTypes[i];

                // Only grab components
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
                if (lType.GetTypeInfo().IsAssignableFrom(typeof(Component).GetTypeInfo()))
#else
                if (lType.IsSubclassOf(typeof(Component)))
#endif
                {
                    object lComponent = rGameObject.GetComponent(lType) as object;
                    if (lComponent != null) { return (T)lComponent; }
                }
                
            }

            // Grab the distinct set of objects
            return default(T);
        }

        /// <summary>
        /// Grab all the component instances that derive from the interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] FindComponentsOfType<T>()
        {
            Type lInterfaceType = typeof(T);

            Type[] lTypes = GetInterfaceTypes(lInterfaceType);
            if (lTypes == null || lTypes.Length == 0) { return null; }

            List<T> lInstances = new List<T>();
            for (int i = 0; i < lTypes.Length; i++)
            {
                Type lType = lTypes[i];

                // Only grab components
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
                if (lType.GetTypeInfo().IsAssignableFrom(typeof(Component).GetTypeInfo()))
#else
                if (lType.IsSubclassOf(typeof(Component)))
#endif
                {
                    lInstances.AddRange(Component.FindObjectsOfType(lType).Cast<T>());
                }
            }

            // Grab the distinct set of objects
            return lInstances.Distinct().ToArray();
        }

        /// <summary>
        /// Grab all the types that implement the interface
        /// </summary>
        /// <param name="rInterface"></param>
        /// <returns></returns>
        public static Type[] GetInterfaceTypes(Type rInterface)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            if (rInterface.GetTypeInfo().IsInterface) { return null; }
#else
            if (!rInterface.IsInterface) { return null; }
#endif

            if (!mInterfaceTypes.ContainsKey(rInterface))
            {
                //Type[] lTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => rInterface.IsAssignableFrom(p) && p != rInterface).ToArray();
                //mInterfaceTypes.Add(rInterface, lTypes);

                Assembly[] lAssemblies = GetAssemblies();
                if (lAssemblies != null && lAssemblies.Length > 0)
                {
                    List<Type> lInterfaceTypes = new List<Type>();

                    for (int i = 0; i < lAssemblies.Length; i++)
                    {
                        try
                        {
                            Type[] lTypes = GetTypes(lAssemblies[i]);
                            if (lTypes != null && lTypes.Length > 0)
                            {
                                for (int j = 0; j < lTypes.Length; j++)
                                {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
                                    if (rInterface.GetTypeInfo().IsAssignableFrom(lTypes[j].GetTypeInfo()) && lTypes[j] != rInterface)
#else
                                    if (rInterface.IsAssignableFrom(lTypes[j]) && lTypes[j] != rInterface)
#endif
                                    {
                                        lInterfaceTypes.Add(lTypes[j]);
                                    }
                                }
                            }
                        }
                        catch { }
                    }

                    mInterfaceTypes.Add(rInterface, lInterfaceTypes.ToArray());
                }
            }

            return mInterfaceTypes[rInterface];
        }

        /// <summary>
        /// Returns the assemblies associated with our projects. We have to support Windows UWP
        /// </summary>
        /// <returns></returns>
        public static Assembly[] GetAssemblies()
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)

            List<Assembly> lAssemblies = new List<Assembly>();
            foreach (var lAssembly in GetAssemblyList().Result)
            {
                lAssemblies.Add(lAssembly);
            }

            return lAssemblies.ToArray();

#else
            return AppDomain.CurrentDomain.GetAssemblies();
#endif
        }

        /// <summary>
        /// Returns the types that are in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] GetTypes(this Assembly assembly)
        {
#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            var typeInfoArray = Enumerable.ToArray<TypeInfo>(assembly.DefinedTypes);
            var typeArray = new Type[typeInfoArray.Length];
            for (int index = 0; index < typeInfoArray.Length; ++index)
                typeArray[index] = typeInfoArray[index].AsType();
            return typeArray;
#else
            return assembly.GetTypes();
#endif
        }

#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)

        /// <summary>
        /// Grab all the assemblies that are part of this project. To do this, we allow Windows UWP to 
        //  look in the DLLs and EXEs.
        /// </summary>
        public static async System.Threading.Tasks.Task<List<Assembly>> GetAssemblyList()
        {
            List<Assembly> assemblies = new List<Assembly>();

            var files = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFilesAsync();
            if (files == null)
                return assemblies;

            foreach (var file in files.Where(file => file.FileType == ".dll" || file.FileType == ".exe"))
            {
                try
                {
                    assemblies.Add(Assembly.Load(new AssemblyName(file.DisplayName)));
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }

            }

            return assemblies;
        }

#endif

    }
}
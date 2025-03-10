using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Kekser.PowerCVar
{
    public static class CVarAttributeCache
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Initialize()
        {
            CacheTypes();
        }
        
        private static Dictionary<string, CVarCache> _cache = new Dictionary<string, CVarCache>();
        private static bool _isCached = false;
        
        public static Dictionary<string, CVarCache> Cache => _cache;
        
        public static bool HasCVar(string name)
        {
            return _cache.ContainsKey(name);
        }
        
        public static CVarCache GetCVar(string name)
        {
            return _cache[name];
        }
        
        public static bool TryGetCVar(string name, out CVarCache cache)
        {
            return _cache.TryGetValue(name, out cache);
        }
        
        private static void CacheCVar(string name, string description, Type type, MemberInfo memberInfo)
        {
            if (_cache.TryAddToDictionary(name, new CVarCache(name, description, type, memberInfo)))
                return;
            Debug.LogErrorFormat("CVar with name {0} already exists. Multiple CVars with the same name are not allowed.", name);
        }

        private static void CacheAttribute(Type type, MemberInfo memberInfo, object attribute)
        {
            CVarAttribute cVarAttribute = attribute as CVarAttribute;
            if (cVarAttribute == null)
                return;
            
            CacheCVar(cVarAttribute.Name, cVarAttribute.Description, type, memberInfo);
        }
        
        private static void CacheMemberInfo(Type type, MemberInfo memberInfo)
        {
            object[] attributes = memberInfo.GetCustomAttributes(typeof(CVarAttribute), false);
                        
            if (attributes.Length == 0)
                return;
            
            foreach (Attribute attribute in attributes)
                CacheAttribute(type, memberInfo, attribute);
        }
        
        private static void CacheType(Type type)
        {
            MemberInfo[] members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public |
                                                   BindingFlags.NonPublic | BindingFlags.Static);

            foreach (MemberInfo member in members)
                CacheMemberInfo(type, member);
        }
        
        private static void CacheAssembly(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
                
            foreach (Type type in types)
                CacheType(type);
        }
        
        private static void CacheTypes()
        {
            if (_isCached)
                return;
            
            string definedIn = typeof(CVarAttribute).Assembly.GetName().Name;
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GlobalAssemblyCache 
                    || assembly.GetName().Name != definedIn 
                    && assembly.GetReferencedAssemblies().All(assemblyName => assemblyName.Name != definedIn))
                    continue;

                CacheAssembly(assembly);
            }
            
            _isCached = true;
        }
    }
}
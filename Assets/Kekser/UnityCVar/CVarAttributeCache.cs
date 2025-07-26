using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kekser.UnityCVar.Converter;
using UnityEngine;

namespace Kekser.UnityCVar
{
    public static class CVarAttributeCache
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
        private static void Initialize()
        {
            CacheTypes();
        }
        
        private static Dictionary<string, ICVar> _cache = new Dictionary<string, ICVar>();
        private static Dictionary<Type, ITypeConverter> _converters = new Dictionary<Type, ITypeConverter>();
        private static bool _isCached = false;
        
        public static IReadOnlyDictionary<string, ICVar> Cache => _cache;
        
        public static bool HasCVar(string name)
        {
            return _cache.ContainsKey(name);
        }
        
        public static ICVar GetCVar(string name)
        {
            return _cache[name];
        }
        
        public static bool TryGetCVar(string name, out ICVar cache)
        {
            return _cache.TryGetValue(name, out cache);
        }
        
        public static bool HasConverter(Type type)
        {
            return _converters.ContainsKey(type);
        }
        
        public static ITypeConverter GetConverter(Type type)
        {
            return _converters[type];
        }
        
        public static bool TryGetConverter(Type type, out ITypeConverter converter)
        {
            return _converters.TryGetValue(type, out converter);
        }
        
        private static void CacheCVar(string name, string description, Type type, MemberInfo memberInfo)
        {
            ICVar cached = CVarFactory.CreateCVar(name, description, type, memberInfo);
            if (_cache.TryAddToDictionary(name, cached))
                return;
            Debug.LogErrorFormat("CVar with name {0} already exists. Multiple CVars with the same name are not allowed.", name);
        }

        private static void CacheCVarAttribute(Type type, MemberInfo memberInfo, object attribute)
        {
            CVarAttribute cVarAttribute = attribute as CVarAttribute;
            if (cVarAttribute == null)
                return;
            
            CacheCVar(cVarAttribute.Name, cVarAttribute.Description, type, memberInfo);
        }
        
        private static void CacheCVarMemberInfo(Type type, MemberInfo memberInfo)
        {
            object[] attributes = memberInfo.GetCustomAttributes(typeof(CVarAttribute), false);
                        
            if (attributes.Length == 0)
                return;
            
            foreach (Attribute attribute in attributes)
                CacheCVarAttribute(type, memberInfo, attribute);
        }
        
        private static void CacheCVarType(Type type)
        {
            MemberInfo[] members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public |
                                                   BindingFlags.NonPublic | BindingFlags.Static | 
                                                   BindingFlags.DeclaredOnly);

            foreach (MemberInfo member in members)
                CacheCVarMemberInfo(type, member);
        }
        
        private static void CacheCVarAssembly(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
                
            foreach (Type type in types)
                CacheCVarType(type);
        }
        
        private static void CacheCVarConverterType(Type type)
        {
            if (!typeof(ITypeConverter).IsAssignableFrom(type))
                return;
            
            TypeConverterAttribute converterAttribute = Attribute.GetCustomAttribute(type, typeof(TypeConverterAttribute)) as TypeConverterAttribute;
            if (converterAttribute == null)
                return;
            
            ITypeConverter converter = Activator.CreateInstance(type) as ITypeConverter;
            if (converter == null)
            {
                Debug.LogErrorFormat("Failed to create instance of type {0}. Make sure the class has a parameterless constructor and implements ITypeConverter.", type.Name);
                return;
            }
            
            if (_converters.TryAddToDictionary(converterAttribute.ConvertType, converter))
                return;
            Debug.LogErrorFormat("Converter for type {0} already exists. Multiple converters for the same type are not allowed.", converterAttribute.ConvertType.Name);
        }
        
        private static void CacheCVarConverterAssembly(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
                
            foreach (Type type in types)
                CacheCVarConverterType(type);
        }
        
        private static void CacheTypes()
        {
            if (_isCached)
                return;
            
            string cVarDefinedIn = typeof(CVarAttribute).Assembly.GetName().Name;
            string cVarConverterDefinedIn = typeof(ITypeConverter).Assembly.GetName().Name;
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.GlobalAssemblyCache 
                    && (assembly.GetName().Name == cVarDefinedIn 
                    || assembly.GetReferencedAssemblies().Any(assemblyName => assemblyName.Name == cVarDefinedIn)))
                    CacheCVarAssembly(assembly);
                
                if (!assembly.GlobalAssemblyCache
                    && (assembly.GetName().Name == cVarConverterDefinedIn 
                    || assembly.GetReferencedAssemblies().Any(assemblyName => assemblyName.Name == cVarConverterDefinedIn)))
                    CacheCVarConverterAssembly(assembly);
            }
            
            _isCached = true;
        }

        public static void RegisterCVar(string name, string description, Type type, string memberName)
        {
            MemberInfo memberInfo = type.GetMember(memberName,
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault();
            if (memberInfo == null)
            {
                Debug.LogErrorFormat("Member '{0}' not found in type '{1}'.", memberName, type.Name);
                return;
            }

            CacheCVar(name, description, type, memberInfo);
        }

    }
}
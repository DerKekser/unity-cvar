using System;
using System.Reflection;

namespace Kekser.PowerCVar
{
    public struct CVarCache
    {
        public string Name;
        public string Description;
        public Type Type;
        public MemberInfo MemberInfo;
        
        public CVarCache(string name, string description, Type type, MemberInfo memberInfo)
        {
            Name = name;
            Description = description;
            Type = type;
            MemberInfo = memberInfo;
        }
    }
}
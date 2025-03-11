using System;
using System.Reflection;

namespace Kekser.UnityCVar
{
    public static class CVarFactory
    {
        public static ICVar CreateCVar(string name, string description, Type type, MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fieldInfo)
                return new CVarField(name, description, type, fieldInfo);
            if (memberInfo is PropertyInfo propertyInfo)
                return new CVarProperty(name, description, type, propertyInfo);
            if (memberInfo is MethodInfo methodInfo)
                return new CVarMethod(name, description, type, methodInfo);
            return null;
        }
    }
}
using System;
using System.Reflection;
using Kekser.UnityCVar.Converter;

namespace Kekser.UnityCVar
{
    public class CVarProperty : ICVar<PropertyInfo>
    {
        public string Name { get; }
        public string Description { get; }
        public Type Type { get; }
        public PropertyInfo MemberInfo { get; }
        
        public bool Static => MemberInfo.GetMethod.IsStatic;

        public CVarProperty(string name, string description, Type type, PropertyInfo memberInfo)
        {
            Name = name;
            Description = description;
            Type = type;
            MemberInfo = memberInfo;
        }
        
        public CVarResult Execute(object target, string[] args)
        {
            if (args.Length == 0 && MemberInfo.CanRead)
                return new CVarResult(true, TypeConverter.ToString(MemberInfo.GetValue(target)));
            if (args.Length == 0 && !MemberInfo.CanRead)
                return new CVarResult(false, $"Property '{Name}' is write-only.");
            if (!MemberInfo.CanWrite)
                return new CVarResult(false, $"Property '{Name}' is read-only.");
            
            if (!TypeConverter.TryConvertValue(args[0], MemberInfo.PropertyType, out object value))
                return new CVarResult(false, $"Failed to convert '{args[0]}' to {MemberInfo.PropertyType.Name}.");
            
            MemberInfo.SetValue(target, value);
            
            return new CVarResult(true, "");
        }
    }
}
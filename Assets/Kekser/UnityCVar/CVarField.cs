using System;
using System.Reflection;
using Kekser.UnityCVar.Converter;

namespace Kekser.UnityCVar
{
    public class CVarField : ICVar<FieldInfo>
    {
        public string Name { get; }
        public string Description { get; }
        public Type Type { get; }
        public FieldInfo MemberInfo { get; }
        
        public bool Static => MemberInfo.IsStatic;
        
        public CVarField(string name, string description, Type type, FieldInfo memberInfo)
        {
            Name = name;
            Description = description;
            Type = type;
            MemberInfo = memberInfo;
        }
        
        public CVarResult Execute(object target, string[] args)
        {
            if (args.Length == 0)
                return new CVarResult(true, TypeConverter.ToString(MemberInfo.GetValue(target)));
            
            if (MemberInfo.IsInitOnly)
                return new CVarResult(false, $"Field '{Name}' is read-only.");

            if (!TypeConverter.TryConvertValue(args[0], MemberInfo.FieldType, out object value))
                return new CVarResult(false, $"Failed to convert '{args[0]}' to {MemberInfo.FieldType.Name}.");
            
            MemberInfo.SetValue(target, value);
            
            return new CVarResult(true, "");
        }
    }
}
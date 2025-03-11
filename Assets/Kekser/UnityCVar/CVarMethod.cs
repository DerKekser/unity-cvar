using System;
using System.Linq;
using System.Reflection;

namespace Kekser.UnityCVar
{
    public class CVarMethod : ICVar<MethodInfo>
    {
        public string Name { get; }
        public string Description { get; }
        public Type Type { get; }
        public MethodInfo MemberInfo { get; }
        
        public bool Static => MemberInfo.IsStatic;

        public CVarMethod(string name, string description, Type type, MethodInfo memberInfo)
        {
            Name = name;
            Description = description;
            Type = type;
            MemberInfo = memberInfo;
        }
        
        public CVarResult Execute(object target, string[] args)
        {
            ParameterInfo[] parameters = MemberInfo.GetParameters();
            ParameterInfo[] optionalParameters = parameters.Where(p => p.IsOptional).ToArray();
            if (args.Length < parameters.Length - optionalParameters.Length || args.Length > parameters.Length)
                return new CVarResult(false, $"Method '{Name}' expects {parameters.Length} arguments.");
            
            object[] arguments = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i >= args.Length)
                    arguments[i] = parameters[i].DefaultValue;
                else if (!Helper.TryConvertValue(args[i], parameters[i].ParameterType, out arguments[i]))
                    return new CVarResult(false, $"Failed to convert '{args[i]}' to {parameters[i].ParameterType.Name}.");
            }
            
            object result = MemberInfo.Invoke(target, arguments);
            return new CVarResult(true, result?.ToString() ?? "");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Kekser.PowerCVar
{
    public static class Helper
    {
        public static bool TryAddToDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                return false;
            dictionary.Add(key, value);
            return true;
        }
        
        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }
        
        public static bool IsStatic(this MemberInfo member)
        {
            if (member is FieldInfo field)
                return field.IsStatic;
            if (member is PropertyInfo property)
                return property.GetMethod.IsStatic;
            if (member is MethodInfo method)
                return method.IsStatic;
            return false;
        }
        
        public static string[] SplitArguments(string input)
        {
            var regex = new Regex(@"[\""].+?[\""]|[^ ]+");
            var matches = regex.Matches(input);
            var args = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                args[i] = matches[i].Value.Trim('"');
            }
            return args;
        }
        
        public static object ConvertValue(string value, Type type)
        {
            if (type == typeof(string))
                return value;
            if (type == typeof(int))
                return int.Parse(value);
            if (type == typeof(float))
                return float.Parse(value);
            if (type == typeof(bool))
                return bool.Parse(value);
            if (type.IsEnum)
                return Enum.Parse(type, value);
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Kekser.UnityCVar
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
        
        public static bool TryConvertValue(string value, Type type, out object result)
        {
            if (type == typeof(string))
            {
                result = value;
                return true;
            }
            if (type == typeof(int) && int.TryParse(value, out int intValue))
            {
                result = intValue;
                return true;
            }
            if (type == typeof(float) && float.TryParse(value, out float floatValue))
            {
                result = floatValue;
                return true;
            }
            if (type == typeof(bool) && bool.TryParse(value, out bool boolValue))
            {
                result = boolValue;
                return true;
            }
            if (type.IsEnum)
            {
                try
                {
                    result = Enum.Parse(type, value);
                    return true;
                }
                catch
                {
                    result = null;
                    return false;
                }
            }
            result = null;
            return false;
        }
    }
}
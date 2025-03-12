using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

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
    }
}
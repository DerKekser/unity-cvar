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
        
        /// <summary>An object's id as text, to list and pick objects by (the entity id on Unity 6.4+, else the instance id).</summary>
        public static string ObjectId(UnityEngine.Object obj)
        {
#if UNITY_6000_4_OR_NEWER
            return obj.GetEntityId().ToString();
#else
            return obj.GetInstanceID().ToString();
#endif
        }

        /// <summary>Every loaded object of the type, inactive ones too (FindObjectsByType on newer Unity versions).</summary>
        public static UnityEngine.Object[] FindAll(Type type)
        {
#if UNITY_6000_4_OR_NEWER
            return UnityEngine.Object.FindObjectsByType(type, FindObjectsInactive.Include);
#elif UNITY_2023_1_OR_NEWER
            return UnityEngine.Object.FindObjectsByType(type, FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
            return UnityEngine.Object.FindObjectsOfType(type, true);
#endif
        }

        /// <summary>Every active object of the type.</summary>
        public static T[] FindAllActive<T>() where T : UnityEngine.Object
        {
#if UNITY_6000_4_OR_NEWER
            return UnityEngine.Object.FindObjectsByType<T>(FindObjectsInactive.Exclude);
#elif UNITY_2023_1_OR_NEWER
            return UnityEngine.Object.FindObjectsByType<T>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
            return UnityEngine.Object.FindObjectsOfType<T>();
#endif
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
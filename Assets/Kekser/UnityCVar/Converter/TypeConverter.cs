using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
    public static class TypeConverter
    {
        private static bool TryParseEnum(string value, Type type, out object result)
        {
            try
            {
                result = Enum.Parse(type, value);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }
        
        public static bool TryConvertValue(string value, object originalValue, Type type, out object result)
        {
            if (type.IsEnum)
                return TryParseEnum(value, type, out result);
            
            if (CVarAttributeCache.TryGetConverter(type, out ITypeConverter converter))
                return converter.TryParse(value, originalValue, out result);

            result = null;
            return false;
        }
        
        public static string ToString(object value)
        {
            if (CVarAttributeCache.TryGetConverter(value.GetType(), out ITypeConverter converter))
                return converter.ToString(value);

            return value.ToString();
        }
    }
}
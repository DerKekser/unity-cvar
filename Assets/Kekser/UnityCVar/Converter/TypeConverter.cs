using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
    public static class TypeConverter
    {
        private static readonly Dictionary<Type, ITypeConverter> Converters = new Dictionary<Type, ITypeConverter>()
        {
            {typeof(string), new StringConverter()},
            {typeof(char), new CharConverter()},
            {typeof(decimal), new DecimalConverter()},
            {typeof(int), new IntConverter()},
            {typeof(uint), new UintConverter()},
            {typeof(long), new LongConverter()},
            {typeof(ulong), new UlongConverter()},
            {typeof(short), new ShortConverter()},
            {typeof(ushort), new UshortConverter()},
            {typeof(float), new FloatConverter()},
            {typeof(double), new DoubleConverter()},
            {typeof(bool), new BoolConverter()},
            {typeof(byte), new ByteConverter()},
            {typeof(sbyte), new SbyteConverter()},
            {typeof(Vector2), new Vector2Converter()},
            {typeof(Vector3), new Vector3Converter()},
            {typeof(Vector4), new Vector4Converter()},
            {typeof(Quaternion), new QuaternionConverter()},
        };
        
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
        
        public static bool TryConvertValue(string value, Type type, out object result)
        {
            if (type.IsEnum)
                return TryParseEnum(value, type, out result);
            
            if (Converters.TryGetValue(type, out ITypeConverter converter))
                return converter.TryParse(value, out result);

            result = null;
            return false;
        }
        
        public static string ToString(object value)
        {
            if (Converters.TryGetValue(value.GetType(), out ITypeConverter converter))
                return converter.ToString(value);

            return value.ToString();
        }
    }
}
using System.Globalization;
using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(Vector4))]
    public class Vector4Converter : ITypeConverter
    {
        private static bool TryConvertVector4(string value, out Vector4 result)
        {
            string[] values = value.Split(',');
            if (values.Length != 4)
            {
                result = Vector4.zero;
                return false;
            }
            if (!float.TryParse(values[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x)
                || !float.TryParse(values[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y)
                || !float.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float z) 
                || !float.TryParse(values[3], NumberStyles.Float, CultureInfo.InvariantCulture, out float w))
            {
                result = Vector4.zero;
                return false;
            }
            result = new Vector4(x, y, z, w);
            return true;
        }
        
        public bool TryParse(string value, object originalValue, out object result)
        {
            if (TryConvertVector4(value, out var s))
            {
                result = s;
                return true;
            }

            result = null;
            return false;
        }
        
        public string ToString(object value)
        {
            Vector4 v = (Vector4)value;
            return $"{v.x},{v.y},{v.z},{v.w}";
        }
    }
}
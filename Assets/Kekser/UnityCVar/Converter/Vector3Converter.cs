using System.Globalization;
using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(Vector3))]
    public class Vector3Converter : ITypeConverter
    {
        private static bool TryConvertVector3(string value, out Vector3 result)
        {
            string[] values = value.Split(',');
            if (values.Length != 3)
            {
                result = Vector3.zero;
                return false;
            }
            if (!float.TryParse(values[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x)
                || !float.TryParse(values[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y)
                || !float.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float z))
            {
                result = Vector3.zero;
                return false;
            }
            result = new Vector3(x, y, z);
            return true;
        }
        
        public bool TryParse(string value, out object result)
        {
            if (TryConvertVector3(value, out var s))
            {
                result = s;
                return true;
            }

            result = null;
            return false;
        }
        
        public string ToString(object value)
        {
            Vector3 v = (Vector3)value;
            return $"{v.x},{v.y},{v.z}";
        }
    }
}
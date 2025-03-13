using System.Globalization;
using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(Vector2))]
    public class Vector2Converter : ITypeConverter
    {
        private static bool TryConvertVector2(string value, out Vector2 result)
        {
            string[] values = value.Split(',');
            if (values.Length != 2)
            {
                result = Vector2.zero;
                return false;
            }
            if (!float.TryParse(values[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x)
                || !float.TryParse(values[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
            {
                result = Vector2.zero;
                return false;
            }
            result = new Vector2(x, y);
            return true;
        }

        public bool TryParse(string value, out object result)
        {
            if (TryConvertVector2(value, out var s))
            {
                result = s;
                return true;
            }

            result = null;
            return false;
        }
        
        public string ToString(object value)
        {
            Vector2 v = (Vector2)value;
            return $"{v.x},{v.y}";
        }
    }
}
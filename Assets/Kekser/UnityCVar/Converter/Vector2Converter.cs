using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
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
            if (!float.TryParse(values[0], out float x) || !float.TryParse(values[1], out float y))
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
            return value.ToString();
        }
    }
}
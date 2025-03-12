using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
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
            if (!float.TryParse(values[0], out float x) || !float.TryParse(values[1], out float y) || !float.TryParse(values[2], out float z) || !float.TryParse(values[3], out float w))
            {
                result = Vector4.zero;
                return false;
            }
            result = new Vector4(x, y, z, w);
            return true;
        }
        
        public bool TryParse(string value, out object result)
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
            return value.ToString();
        }
    }
}
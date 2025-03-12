using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
    public class QuaternionConverter : ITypeConverter
    {
        private static bool TryConvertEuler(string value, out Quaternion result)
        {
            string[] values = value.Split(',');
            if (values.Length != 3)
            {
                result = Quaternion.identity;
                return false;
            }
            if (!float.TryParse(values[0], out float x) || !float.TryParse(values[1], out float y) || !float.TryParse(values[2], out float z))
            {
                result = Quaternion.identity;
                return false;
            }
            result = Quaternion.Euler(x, y, z);
            return true;
        }
        
        public bool TryParse(string value, out object result)
        {
            if (TryConvertEuler(value, out var s))
            {
                result = s;
                return true;
            }

            result = null;
            return false;
        }

        public string ToString(object value)
        {
            Quaternion q = (Quaternion) value;
            return $"{q.eulerAngles.x},{q.eulerAngles.y},{q.eulerAngles.z}";
        }
    }
}
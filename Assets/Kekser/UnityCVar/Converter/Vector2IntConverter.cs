using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
    public class Vector2IntConverter : ITypeConverter
    {
        private static bool TryConvertVector2Int(string value, out Vector2Int result)
        {
            string[] values = value.Split(',');
            if (values.Length != 2)
            {
                result = Vector2Int.zero;
                return false;
            }
            if (!int.TryParse(values[0], out int x) || !int.TryParse(values[1], out int y))
            {
                result = Vector2Int.zero;
                return false;
            }
            result = new Vector2Int(x, y);
            return true;
        }

        public bool TryParse(string value, out object result)
        {
            if (TryConvertVector2Int(value, out var s))
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
using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
    public class Vector3IntConverter : ITypeConverter
    {
        private static bool TryConvertVector3Int(string value, out Vector3Int result)
        {
            string[] values = value.Split(',');
            if (values.Length != 3)
            {
                result = Vector3Int.zero;
                return false;
            }
            if (!int.TryParse(values[0], out int x) || !int.TryParse(values[1], out int y) || !int.TryParse(values[2], out int z))
            {
                result = Vector3Int.zero;
                return false;
            }
            result = new Vector3Int(x, y, z);
            return true;
        }
        
        public bool TryParse(string value, out object result)
        {
            if (TryConvertVector3Int(value, out var s))
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
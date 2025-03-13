namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(byte))]
    public class ByteConverter : ITypeConverter
    {
        public bool TryParse(string value, object originalValue, out object result)
        {
            if (byte.TryParse(value, out var b))
            {
                result = b;
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
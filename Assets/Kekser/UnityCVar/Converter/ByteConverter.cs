namespace Kekser.UnityCVar.Converter
{
    public class ByteConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
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
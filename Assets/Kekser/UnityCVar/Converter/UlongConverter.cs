namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(ulong))]
    public class UlongConverter : ITypeConverter
    {
        public bool TryParse(string value, object originalValue, out object result)
        {
            if (ulong.TryParse(value, out var ul))
            {
                result = ul;
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
namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(long))]
    public class LongConverter : ITypeConverter
    {
        public bool TryParse(string value, object originalValue, out object result)
        {
            if (long.TryParse(value, out var l))
            {
                result = l;
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
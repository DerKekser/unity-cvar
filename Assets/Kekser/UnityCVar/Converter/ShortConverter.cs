namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(short))]
    public class ShortConverter : ITypeConverter
    {
        public bool TryParse(string value, object originalValue, out object result)
        {
            if (short.TryParse(value, out var s))
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
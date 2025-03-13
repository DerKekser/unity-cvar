namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(char))]
    public class CharConverter : ITypeConverter
    {
        public bool TryParse(string value, object originalValue, out object result)
        {
            if (char.TryParse(value, out var s))
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
namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(sbyte))]
    public class SbyteConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            if (sbyte.TryParse(value, out var s))
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
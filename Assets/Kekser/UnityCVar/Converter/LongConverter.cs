namespace Kekser.UnityCVar.Converter
{
    public class LongConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
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
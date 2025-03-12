namespace Kekser.UnityCVar.Converter
{
    public class DecimalConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            if (decimal.TryParse(value, out var s))
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
namespace Kekser.UnityCVar.Converter
{
    public class DoubleConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            if (double.TryParse(value, out var d))
            {
                result = d;
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
namespace Kekser.UnityCVar.Converter
{
    public class FloatConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            if (float.TryParse(value, out var f))
            {
                result = f;
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
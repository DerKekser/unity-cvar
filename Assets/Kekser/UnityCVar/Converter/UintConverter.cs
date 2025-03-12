namespace Kekser.UnityCVar.Converter
{
    public class UintConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            if (uint.TryParse(value, out var u))
            {
                result = u;
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
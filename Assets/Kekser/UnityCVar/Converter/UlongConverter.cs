namespace Kekser.UnityCVar.Converter
{
    public class UlongConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
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
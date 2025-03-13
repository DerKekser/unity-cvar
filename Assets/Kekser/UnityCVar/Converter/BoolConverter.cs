namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(bool))]
    public class BoolConverter : ITypeConverter
    {
        public bool TryParse(string value, object originalValue, out object result)
        {
            if (bool.TryParse(value, out var b))
            {
                result = b;
                return true;
            }
            
            result = null;
            return false;
        }

        public string ToString(object value)
        {
            return value.ToString().ToLower();
        }
    }
}
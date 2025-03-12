namespace Kekser.UnityCVar.Converter
{
    public class BoolConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
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
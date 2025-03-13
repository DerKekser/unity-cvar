namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(int))]
    public class IntConverter : ITypeConverter
    {
        public bool TryParse(string value, object originalValue, out object result)
        {
            if (int.TryParse(value, out var i))
            {
                result = i;
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
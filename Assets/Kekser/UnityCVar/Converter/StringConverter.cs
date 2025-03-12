namespace Kekser.UnityCVar.Converter
{
    public class StringConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            result = value;
            return true;
        }

        public string ToString(object value)
        {
            return value.ToString();
        }
    }
}
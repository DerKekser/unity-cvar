namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(ushort))]
    public class UshortConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            if (ushort.TryParse(value, out var s))
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
using System.Globalization;

namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(decimal))]
    public class DecimalConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var s))
            {
                result = s;
                return true;
            }

            result = null;
            return false;
        }

        public string ToString(object value)
        {
            decimal d = (decimal)value;
            return d.ToString(CultureInfo.InvariantCulture);
        }
    }
}
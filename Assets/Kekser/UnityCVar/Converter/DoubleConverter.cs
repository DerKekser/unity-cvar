using System.Globalization;

namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(double))]
    public class DoubleConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
            {
                result = d;
                return true;
            }

            result = null;
            return false;
        }

        public string ToString(object value)
        {
            double d = (double)value;
            return d.ToString(CultureInfo.InvariantCulture);
        }
    }
}
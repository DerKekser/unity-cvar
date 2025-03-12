using System.Globalization;

namespace Kekser.UnityCVar.Converter
{
    public class FloatConverter : ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var f))
            {
                result = f;
                return true;
            }

            result = null;
            return false;
        }

        public string ToString(object value)
        {
            float f = (float)value;
            return f.ToString(CultureInfo.InvariantCulture);
        }
    }
}
﻿namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(string))]
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
using System;

namespace Kekser.UnityCVar.Converter
{
    public class TypeConverterAttribute : Attribute
    {
        private Type _type;
        
        public Type ConvertType => _type;
        
        public TypeConverterAttribute(Type type)
        {
            _type = type;
        }
    }
}
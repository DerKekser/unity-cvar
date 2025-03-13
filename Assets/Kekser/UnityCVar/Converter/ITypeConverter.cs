namespace Kekser.UnityCVar.Converter
{
    public interface ITypeConverter
    {
        bool TryParse(string value, object originalValue, out object result); 
        string ToString(object value);
    }
}
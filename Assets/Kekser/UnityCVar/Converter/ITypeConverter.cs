namespace Kekser.UnityCVar.Converter
{
    public interface ITypeConverter
    {
        bool TryParse(string value, out object result);
        string ToString(object value);
    }
}
using System.Linq;
using UnityEngine;

namespace Kekser.UnityCVar.Converter
{
    [TypeConverter(typeof(GameObject))]
    public class GameObjectConverter :  ITypeConverter
    {
        public bool TryParse(string value, out object result)
        {
            GameObject[] allObjects = Helper.FindAllActive<GameObject>()
                .Where(go => Helper.ObjectId(go) == value || go.name == value)
                .ToArray();

            if (allObjects.Length == 1)
            {
                result = allObjects[0];
                return true;
            }
            
            result = null;
            return false;
        }

        public string ToString(object value)
        {
            if (value == null)
                return "null";
            GameObject go = (GameObject) value;
            return go == null ? "null" : $"{go.name}: ({Helper.ObjectId(go)})";
        }
    }
}
using System;

namespace Kekser.UnityCVar
{
    public class CVarArgInfo
    {
        public string Name;
        public Type Type;
        
        public CVarArgInfo(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}
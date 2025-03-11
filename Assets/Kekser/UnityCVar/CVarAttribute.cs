using System;

namespace Kekser.UnityCVar
{
    public class CVarAttribute : Attribute
    {
        private string _name;
        private string _description;
        
        public string Name => _name;
        public string Description => _description;
        
        public CVarAttribute(string name, string description = "")
        {
            _name = name;
            _description = description;
        }
    }
}
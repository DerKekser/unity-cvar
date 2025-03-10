namespace Kekser.PowerCVar
{
    public enum CVarTargetType
    {
        None,
        ClassList,
        GameObjectList,
    }
    
    public struct CVarTarget
    {
        public string TargetName;
        public CVarTargetType TargetType;
    }
}
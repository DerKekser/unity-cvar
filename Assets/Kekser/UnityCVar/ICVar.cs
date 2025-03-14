using System;
using System.Reflection;

namespace Kekser.UnityCVar
{
    public interface ICVar
    {
        string Name { get; }
        string Description { get; }
        Type Type { get; }
        bool Static { get; }
        Type ReadType { get; }
        Type[] WriteTypes { get; }
        CVarResult Execute(object target, string[] args);
    }
    
    public interface ICVar<out T> : ICVar where T : MemberInfo
    {
        T MemberInfo { get; }
    }
}
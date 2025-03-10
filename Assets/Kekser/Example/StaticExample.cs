using Kekser.PowerCVar;

namespace Kekser.Example
{
    public static class StaticExample
    {
        [CVar("exp_static_field")]
        private static int _staticField = 0;
        
        [CVar("exp_static_property")]
        private static int StaticProperty { get; set; } = 0;
        
        [CVar("exp_static_method")]
        private static void StaticMethod()
        {
            _staticField++;
            StaticProperty++;
        }
        
        [CVar("exp_static_method_args")]
        private static void StaticMethodArgs(int value)
        {
            _staticField += value;
            StaticProperty += value;
        }
    }
}
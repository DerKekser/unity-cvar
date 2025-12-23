using Kekser.UnityCVar;

namespace Kekser.Tests
{
    public static class StaticTestsVars
    {
        [CVar("test_static_field")]
        private static int _staticField = 0;
        
        [CVar("test_static_property")]
        private static int StaticProperty { get; set; } = 0;
        
        [CVar("test_static_property_get")]
        private static int StaticPropertyGet { get; } = 10;  
        
        [CVar("test_static_method")]
        private static void StaticMethod()
        {
            
        }
        
        [CVar("test_static_method_return")]
        private static string StaticMethodReturn()
        {
            return "test";
        }
        
        [CVar("test_static_method_args_string")]
        private static string StaticMethodArgs(string value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_char")]
        private static char StaticMethodArgs(char value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_decimal")]
        private static decimal StaticMethodArgs(decimal value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_int")]
        private static int StaticMethodArgs(int value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_uint")]
        private static uint StaticMethodArgs(uint value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_long")]
        private static long StaticMethodArgs(long value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_ulong")]
        private static ulong StaticMethodArgs(ulong value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_short")]
        private static short StaticMethodArgs(short value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_ushort")]
        private static ushort StaticMethodArgs(ushort value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_float")]
        private static float StaticMethodArgs(float value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_double")]
        private static double StaticMethodArgs(double value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_bool")]
        private static bool StaticMethodArgs(bool value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_byte")]
        private static byte StaticMethodArgs(byte value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_sbyte")]
        private static sbyte StaticMethodArgs(sbyte value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_vector2")]
        private static UnityEngine.Vector2 StaticMethodArgs(UnityEngine.Vector2 value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_vector2int")]
        private static UnityEngine.Vector2Int StaticMethodArgs(UnityEngine.Vector2Int value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_vector3")]
        private static UnityEngine.Vector3 StaticMethodArgs(UnityEngine.Vector3 value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_vector3int")]
        private static UnityEngine.Vector3Int StaticMethodArgs(UnityEngine.Vector3Int value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_vector4")]
        private static UnityEngine.Vector4 StaticMethodArgs(UnityEngine.Vector4 value)
        {
            return value;
        }
        
        [CVar("test_static_method_args_quaternion")]
        private static UnityEngine.Quaternion StaticMethodArgs(UnityEngine.Quaternion value)
        {
            return value;
        }

        [CVar("test_static_method_args_gameobject")]
        private static UnityEngine.GameObject StaticMethodArgs(UnityEngine.GameObject value)
        {
            return value;
        }
    }
}
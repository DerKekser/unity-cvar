using Kekser.UnityCVar;
using NUnit.Framework;

namespace Kekser.Tests
{
    [TestFixture]
    public class StaticTests
    {
        private CVarManager _cVarManager = new CVarManager();
        
        [Test]
        public void TestStaticField()
        {
            var result = _cVarManager.ExecuteCommand("test_static_field");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("0", result.Message);

            result = _cVarManager.ExecuteCommand("test_static_field 10");
            Assert.IsTrue(result.Success);

            result = _cVarManager.ExecuteCommand("test_static_field");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticProperty()
        {
            var result = _cVarManager.ExecuteCommand("test_static_property");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("0", result.Message);

            result = _cVarManager.ExecuteCommand("test_static_property 10");
            Assert.IsTrue(result.Success);

            result = _cVarManager.ExecuteCommand("test_static_property");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticPropertyGet()
        {
            var result = _cVarManager.ExecuteCommand("test_static_property_get");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethod()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method");
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void TestStaticMethodReturn()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_return");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("test", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsString()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_string test");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("test", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsChar()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_char t");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("t", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsDecimal()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_decimal 10.0");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10.0", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsInt()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_int 10");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsUInt()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_uint 10");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsLong()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_long 10");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsUlong()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_ulong 10");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsShort()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_short 10");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsUshort()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_ushort 10");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsFloat()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_float 10.0");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsDouble()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_double 10.0");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsBool()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_bool true");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("true", result.Message);
        }
        
        [Test]
        public void TestStaticMethodArgsShortBool()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_bool 1");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("true", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsByte()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_byte 10");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsSbyte()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_sbyte 10");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("10", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsVector2()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_vector2 1,2");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("1,2", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsVector2Int()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_vector2int 1,2");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("1,2", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsVector3()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_vector3 1,2,3");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("1,2,3", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsVector3Int()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_vector3int 1,2,3");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("1,2,3", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsVector4()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_vector4 1,2,3,4");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("1,2,3,4", result.Message);
        }

        [Test]
        public void TestStaticMethodArgsQuaternion()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_quaternion 1,2,3");
            Assert.IsTrue(result.Success);
            Assert.AreEqual("1,2,3", result.Message);
        }
        
        [Test]
        public void TestStaticMethodArgsInvalid()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method 10");
            Assert.IsFalse(result.Success);
        }
        
        [Test]
        public void TestStaticMethodArgsMissing()
        {
            var result = _cVarManager.ExecuteCommand("test_static_method_args_int");
            Assert.IsFalse(result.Success);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mangosteen.Test
{
    public class SymbolExtensionsTests
    {
        public class TestClass
        {
            public void AMethod()
            {}

            public void AGenericMethod<V>(V input)
            {}
        }

        public static class StaticTestClass
        {
            public static void StaticTestMethod() {}
        }

        [Fact]
        public void GetMethodInfo_should_return_method_info()
        {
            var methodInfo = SymbolExtensions.GetMethodInfo<TestClass>(c => c.AMethod());
            Assert.True(methodInfo.Name == "AMethod");
        }

        [Fact]
        public void GetMethodInfo_should_return_method_info_for_generic_method()
        {
            var methodInfo = SymbolExtensions.GetMethodInfo<TestClass>(c => c.AGenericMethod(default(int)));

            Assert.True(methodInfo.Name == "AGenericMethod");
            Assert.True(methodInfo.GetParameters().First().ParameterType == typeof(int));
        }

        [Fact]
        public void GetMethodInfo_should_return_method_info_for_static_method_on_static_class()
        {
            var methodInfo = SymbolExtensions.GetMethodInfo(() => StaticTestClass.StaticTestMethod());

            Assert.True(methodInfo.Name == "StaticTestMethod");
            Assert.True(methodInfo.IsStatic);
        }
    }
}


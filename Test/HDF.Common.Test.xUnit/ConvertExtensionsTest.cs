using System;
using System.Collections;
using System.Linq;
using Xunit;

namespace HDF.Common.Test.xUnit
{
    public class ConvertExtensionsTest
    {


        [Fact]
        public void ToTest()
        {
            Assert.Equal(1, "1".To<int>());
            Assert.Equal(new DateTime(2021, 9, 18), "2021-09-18".To<DateTime>());
            Assert.False("false".To<bool>());
        }


        [Fact]
        public void AsTest()
        {
            Assert.IsAssignableFrom<string>(((IComparable)string.Empty).As<string>());
            Assert.IsType<Child>(((Parent)new Child()).As<Child>());
        }

        private class Parent { }
        private class Child : Parent { }



        [Fact]
        public void ToEnumTest()
        {
            Assert.Equal(Sex.Boy, "Boy".ToEnum<Sex>());
            Assert.Equal(Sex.Girl, "Girl".ToEnum<Sex>());
        }

        private enum Sex { Boy, Girl }






        [Fact]
        public void GetCustomAttributeTest()
        {
            Assert.NotNull(typeof(Test).GetCustomAttribute<AAttribute>());
            Assert.NotNull(typeof(Test).GetCustomAttribute<AAttribute>(true));
        }


        private class AAttribute : Attribute { }

        [A]
        private class Test { }
        private class TestChild : Test { }



    }
}

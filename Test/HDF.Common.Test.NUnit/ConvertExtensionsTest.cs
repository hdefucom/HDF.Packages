using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace HDF.Common.Test.NUnit
{
    public class ConvertExtensionsTest
    {


        [Test]
        public void ToTest()
        {
            Assert.AreEqual(1, "1".To<int>());
            Assert.AreEqual(new DateTime(2021, 9, 18), "2021-09-18".To<DateTime>());
            Assert.False("false".To<bool>());
        }


        [Test]
        public void AsTest()
        {
            Assert.IsAssignableFrom<string>(((IComparable)string.Empty).As<string>());
            Assert.IsInstanceOf<Child>(((Parent)new Child()).As<Child>());
        }

        private class Parent { }
        private class Child : Parent { }



        [Test]
        public void ToEnumTest()
        {
            Assert.AreEqual(Sex.Boy, "Boy".ToEnum<Sex>());
            Assert.AreEqual(Sex.Girl, "Girl".ToEnum<Sex>());
        }

        private enum Sex { Boy, Girl }






        [Test]
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

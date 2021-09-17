using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HDF.Common.Test.NUnit
{
    public class IEnumerableExtensionsTest
    {


        [Test]
        public void IsNullOrEmptyTest()
        {
            Assert.False(Enumerable.Range(0, 10).IsNullOrEmpty());

            Assert.True(Enumerable.Range(0, 0).IsNullOrEmpty());

            Assert.True(((IEnumerable<int>)null).IsNullOrEmpty());
        }

        [Test]
        public void ForeachTest()
        {
            var list = Enumerable.Range(0, 10).Select(i => new TestDto(i)).ToList();
            var lsit2 = ((IEnumerable<TestDto>)list).ForEach(o => _ = o.ToString()).ToList();
            Assert.AreEqual(list, list);

            Assert.Throws<ArgumentNullException>(() => Enumerable.Range(0, 10).Select(i => new TestDto(i)).ForEach(null).ToList());
            Assert.Throws<ArgumentNullException>(() => ((IEnumerable<TestDto>)null).ForEach(null).ToList());

        }


        [Test]
        public void ForTest()
        {
            Assert.AreEqual(10, 10.For(i => i).Count());

            Assert.IsEmpty((-1).For(i => i));

            Assert.Throws<ArgumentNullException>(() => 10.For<int>(null).ToList());
        }

        [Test]
        public void NextTest()
        {
            var list = 10.For(i => new TestDto(i)).ToList();

            Assert.AreEqual(list[6], list.Next(list[5]));

            Assert.Null(list.Next(list[9]));

            list[5] = null;
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Next(new TestDto(5)));

            Assert.Throws<ArgumentOutOfRangeException>(() => list.Next(new TestDto(11)));

            Assert.Throws<ArgumentNullException>(() => list.Next(null));

            list = null;
            Assert.Throws<ArgumentNullException>(() => list.Next(null));
        }


        [Test]
        public void PreviousTest()
        {
            var list = 10.For(i => new TestDto(i)).ToList();

            Assert.AreEqual(list[6], list.Previous(list[7]));

            Assert.Null(list.Previous(list[0]));

            list[5] = null;
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Previous(new TestDto(5)));

            Assert.Throws<ArgumentOutOfRangeException>(() => list.Previous(new TestDto(11)));

            Assert.Throws<ArgumentNullException>(() => list.Previous(null));

            list = null;
            Assert.Throws<ArgumentNullException>(() => list.Previous(null));
        }




        private class TestDto
        {
            public int Count;
            public TestDto(int count)
            {
                Count = count;
            }
        }


    }
}

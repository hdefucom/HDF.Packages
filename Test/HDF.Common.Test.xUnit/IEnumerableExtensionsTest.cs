using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HDF.Common.Test.xUnit
{
    public class IEnumerableExtensionsTest
    {


        [Fact]
        public void IsNullOrEmptyTest()
        {
            Assert.False(Enumerable.Range(0, 10).IsNullOrEmpty());

            Assert.True(Enumerable.Range(0, 0).IsNullOrEmpty());

            Assert.True(((IEnumerable<int>)null).IsNullOrEmpty());
        }

        [Fact]
        public void ForeachTest()
        {
            var list = Enumerable.Range(0, 10).Select(i => new TestDto(i)).ToList();
            var lsit2 = ((IEnumerable<TestDto>)list).ForEach(o => _ = o.ToString()).ToList();
            Assert.Equal(list, list);

            Assert.ThrowsAny<ArgumentNullException>(() => Enumerable.Range(0, 10).Select(i => new TestDto(i)).ForEach(null).ToList());
            Assert.ThrowsAny<ArgumentNullException>(() => ((IEnumerable<TestDto>)null).ForEach(null).ToList());

        }


        [Fact]
        public void ForTest()
        {
            Assert.Equal(10, 10.For(i => i).Count());

            Assert.Empty((-1).For(i => i));

            Assert.ThrowsAny<ArgumentNullException>(() => 10.For<int>(null).ToList());
        }

        [Fact]
        public void NextTest()
        {
            var list = 10.For(i => new TestDto(i)).ToList();

            Assert.Equal(list[6], list.Next(list[5]));

            Assert.Null(list.Next(list[9]));

            list[5] = null;
            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => list.Next(new TestDto(5)));

            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => list.Next(new TestDto(11)));

            Assert.ThrowsAny<ArgumentNullException>(() => list.Next(null));

            list = null;
            Assert.ThrowsAny<ArgumentNullException>(() => list.Next(null));
        }


        [Fact]
        public void PreviousTest()
        {
            var list = 10.For(i => new TestDto(i)).ToList();

            Assert.Equal(list[6], list.Previous(list[7]));

            Assert.Null(list.Previous(list[0]));

            list[5] = null;
            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => list.Previous(new TestDto(5)));

            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => list.Previous(new TestDto(11)));

            Assert.ThrowsAny<ArgumentNullException>(() => list.Previous(null));

            list = null;
            Assert.ThrowsAny<ArgumentNullException>(() => list.Previous(null));
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

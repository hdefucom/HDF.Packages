using System;
using System.Diagnostics;
using Xunit;

namespace HDF.Common.Test.xUnit
{
    public class StackTraceExtrensionsTest
    {


        [Fact]
        public void CheckRecursionTest()
        {
            Assert.False(RecursionMethod(0));

            Assert.False(RecursionMethod(1));

            Assert.True(RecursionMethod(2));

            Assert.True(RecursionMethod(4));

            Assert.False(StackTraceExtrensions.CheckRecursion(new StackTrace().FrameCount - 1));

            Assert.False(StackTraceExtrensions.CheckRecursion());

            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => StackTraceExtrensions.CheckRecursion(-1));
        }


        private bool RecursionMethod(int count)
        {
            if (count <= 0)
                return false;
            count--;
            return RecursionMethod(count) || StackTraceExtrensions.CheckRecursion();
        }





        [Fact]
        public void OutputStackTraceTest()
        {
            var data = StackTraceExtrensions.OutputStackTrace();
            Assert.Contains($"{this.GetType().FullName}.{nameof(OutputStackTraceTest)}()", data);
        }




    }
}

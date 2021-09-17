using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;

namespace HDF.Common.Test.NUnit
{
    public class StackTraceExtrensionsTest
    {


        [Test]
        public void CheckRecursionTest()
        {
            Assert.False(RecursionMethod(0));

            Assert.False(RecursionMethod(1));

            Assert.True(RecursionMethod(2));

            Assert.True(RecursionMethod(4));

            Assert.False(StackTraceExtrensions.CheckRecursion(new StackTrace().FrameCount - 1));

            Assert.False(StackTraceExtrensions.CheckRecursion());

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => StackTraceExtrensions.CheckRecursion(-1));
        }

        private bool RecursionMethod(int count)
        {
            if (count <= 0)
                return false;
            count--;
            return RecursionMethod(count) || StackTraceExtrensions.CheckRecursion();
        }





        [Test]
        public void OutputStackTraceTest()
        {
            var data = StackTraceExtrensions.OutputStackTrace();
            var res = data.Contains($"{this.GetType().FullName}.{nameof(OutputStackTraceTest)}()");
            Assert.True(res);
        }




    }
}

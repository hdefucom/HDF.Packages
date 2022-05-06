namespace HDF.DotNet.Features.PropertyInitAccessor.Test
{
    internal class Class1
    {
        public Class1()
        {

        }


        public int MyProperty { get; init; }

    }

    public record TestRecord(string str);
}

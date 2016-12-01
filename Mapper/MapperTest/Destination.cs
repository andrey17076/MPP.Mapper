using System;

namespace MapperTest
{
    internal sealed class Destination
    {
        public long CanConvert { get; set; }
        public string SameType { get; set; }
        public float CantConvert { get; set; }
        public DateTime NotPrimitiveCanConvert { get; set; }
        public DateTime NotPrimitiveCantConvert { get; set; }
        public string FirstName { get; set; }
        public long AnotherNumberCanConvert { get; set; }
        public object CantAssign { get; } = null;
    }
}

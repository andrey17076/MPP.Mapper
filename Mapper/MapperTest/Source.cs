using System;

namespace MapperTest
{
    internal sealed class Source
    {
        public int CanConvert { get; set; }
        public string SameType { get; set; }
        public double CantConvert { get; set; }
        public DateTime NonPremitiveCanConvert { get; set; }
        public string Name { get; set; }
        public int OneNumberCanConvert { get; set; }
        public string CantAssign { get; set; }
    }
}

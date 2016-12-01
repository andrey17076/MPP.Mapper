using System;
using Mapper;

namespace MapperTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var conf = new MapperConfiguration();
            conf.Register<Source, Destination>(source => source.Name, destination => destination.FirstName);
            conf.Register<Source, Destination>(source => source.OneNumberCanConvert, destination => destination.AnotherNumberCanConvert);
            var mapper = new Mapper.Mapper(conf);
            var foo = new Source
            {
                CanConvert = 1,
                SameType = "a",
                CantConvert = 3.14,
                OneNumberCanConvert = 1,
                Name = "a"
            };
            var bar = mapper.Map<Source, Destination>(foo);
             Console.WriteLine(bar.CanConvert);
            Console.WriteLine(bar.CantConvert);
            Console.WriteLine(bar.CantAssign);
            Console.WriteLine(bar.SameType);
            Console.WriteLine(bar.AnotherNumberCanConvert);
            Console.WriteLine(bar.FirstName);
            Console.ReadKey();
        }
    }
}
using System;

namespace Mapper
{
    public class MappingTypes
    {
        public MappingTypes(Type source, Type destination)
        {
            Source = source;
            Destination = destination;
        }

        public Type Source { get; }
        public Type Destination { get; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (obj.GetType() != typeof(MappingTypes)) return false;
            var typesMapping = (MappingTypes) obj;
            return (typesMapping.Source == Source) && (typesMapping.Destination == Destination);
        }

        public override int GetHashCode()
        {
            return (Source.GetHashCode() * 13) ^ Destination.GetHashCode();
        }
    }
}

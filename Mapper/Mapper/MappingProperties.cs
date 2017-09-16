using System.Reflection;

namespace Mapper
{
    public class MappingProperties
    {
        public MappingProperties(PropertyInfo source, PropertyInfo destination)
        {
            Source = source;
            Destination = destination;
        }

        public PropertyInfo Source { get; }
        public PropertyInfo Destination { get; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            if (obj.GetType() != typeof(MappingProperties)) return false;
            var propertiesMapping = (MappingProperties) obj;
            return propertiesMapping.Source.Equals(Source) && propertiesMapping.Destination.Equals(Destination);
        }

        public override int GetHashCode()
        {
            return (Source.GetHashCode() * 37) ^ Destination.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;

namespace Mapper
{
    internal static class AssigmentHelper
    {
        private static readonly Dictionary<Type, HashSet<Type>> ImplicitConversions = new Dictionary<Type, HashSet<Type>> {
            { typeof(sbyte), new HashSet<Type> { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(byte), new HashSet<Type> { typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(short), new HashSet<Type> { typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(ushort), new HashSet<Type> { typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(int), new HashSet<Type> { typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(uint), new HashSet<Type> { typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(long), new HashSet<Type> { typeof(float), typeof(double), typeof(decimal) } },
            { typeof(char), new HashSet<Type> { typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(float), new HashSet<Type> { typeof(double) } },
            { typeof(ulong), new HashSet<Type> { typeof(float), typeof(double), typeof(decimal) } }
        };

        internal static bool CanAssign(MappingProperties propertiesMapping)
        {
            return CanAssign(propertiesMapping.Source.PropertyType, propertiesMapping.Destination.PropertyType);
        }

        internal static bool CanAssign(MappingTypes typesMapping)
        {
            return CanAssign(typesMapping.Source, typesMapping.Destination);
        }

        internal static bool CanAssign(Type sourceType, Type destinationType)
        {
            if (sourceType == destinationType) return true;
            if (!sourceType.IsPrimitive || !destinationType.IsPrimitive) return false;
            if (!ImplicitConversions.ContainsKey(sourceType)) return false;
            return ImplicitConversions[sourceType].Contains(destinationType);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using System.Reflection;

namespace Mapper
{
    public class MapperConfiguration
    {
        private readonly Dictionary<MappingTypes, List<MappingProperties>> _configuratedMappings =
            new Dictionary<MappingTypes, List<MappingProperties>>();

        public MapperConfiguration Register<TSource, TDestination>(
            Expression<Func<TSource, object>> sourcePropertyAccessor,
            Expression<Func<TDestination, object>> destinationPropertyAccessor)
        {
            MappingProperties addingPropertiesMapping = GetAddingsMapping(sourcePropertyAccessor,
                destinationPropertyAccessor);
            var typesMapping = new MappingTypes(typeof(TSource), typeof(TDestination));

            if (Mapper.CanMapProperties(addingPropertiesMapping))
                AddCongiguration(typesMapping, addingPropertiesMapping);

            return this;
        }

        internal IEnumerable<MappingProperties> GetRegisteredMappings<TSource, TDestination>()
        {
            var mappingEntryInfo = new MappingTypes(typeof(TSource), typeof(TDestination));
            List<MappingProperties> registeredMappings;
            if (!_configuratedMappings.TryGetValue(mappingEntryInfo, out registeredMappings))
            {
                registeredMappings = new List<MappingProperties>();
            }
            return registeredMappings;
        }

        private void AddCongiguration(MappingTypes typesMapping, MappingProperties propertiesMapping)
        {
            List<MappingProperties> registeredMappings;
            if (!_configuratedMappings.TryGetValue(typesMapping, out registeredMappings))
            {
                registeredMappings = new List<MappingProperties>();
                _configuratedMappings[typesMapping] = registeredMappings;
            }
            registeredMappings.Add(propertiesMapping);
        }

        private static MappingProperties GetAddingsMapping<TSource, TDestination>(
            Expression<Func<TSource, object>> sourcePropertyAccessor,
            Expression<Func<TDestination, object>> destinationPropertyAccessor)
        {
            PropertyInfo sourceProperty = GetPropertyInfo(sourcePropertyAccessor);
            PropertyInfo destinationProperty = GetPropertyInfo(destinationPropertyAccessor);

            return new MappingProperties(sourceProperty, destinationProperty);
        }

        private static PropertyInfo GetPropertyInfo<TSource>(Expression<Func<TSource, object>> configurationFunction)
        {
            Expression configurationExpression = configurationFunction.Body;
            MemberExpression propertyExpression;
            if (configurationExpression.NodeType == ExpressionType.Convert)
            {
                propertyExpression = (MemberExpression) ((UnaryExpression) configurationExpression).Operand;
            }
            else
            {
                propertyExpression = (MemberExpression) configurationExpression;
            }

            return (PropertyInfo) propertyExpression.Member;
        }
    }
}

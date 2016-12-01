using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mapper
{
    public class Mapper : IMapper
    {
        private readonly MapperConfiguration _configuration;
        private readonly Dictionary<MappingTypes, Delegate> _mappedTypesFunctions;

        public Mapper() : this(new MapperConfiguration()) { }

        public Mapper(MapperConfiguration configuration)
        {
            _configuration = configuration;
            _mappedTypesFunctions = new Dictionary<MappingTypes, Delegate>();
        }

        public TDestination Map<TSource, TDestination>(TSource source) where TDestination : new()
        {
            var mappingTypes = new MappingTypes(typeof(TSource), typeof(TDestination));
            var mappingFunction = GetOrCreateMappingFunction<TSource, TDestination>(mappingTypes);
            return mappingFunction(source);
        }

        private Func<TSource, TDestination> GetOrCreateMappingFunction<TSource, TDestination>(MappingTypes mappingTypes) where TDestination : new()
        {
            Delegate cachedFunction;
            Func<TSource, TDestination> mappingFunction;
            var hasCachedFunction = _mappedTypesFunctions.TryGetValue(mappingTypes, out cachedFunction);
            if (hasCachedFunction)
            {
                mappingFunction = (Func<TSource, TDestination>) cachedFunction;
            }
            else
            {
                var mappingProperties = GetMappingProperties(mappingTypes);
                mappingProperties.AddRange(_configuration.GetRegisteredMappings<TSource, TDestination>());
                mappingFunction = CreateMappingFunction<TSource, TDestination>(mappingProperties);
                _mappedTypesFunctions.Add(mappingTypes, mappingFunction);
            }
            return mappingFunction;
        }

        public static bool CanMapProperties(MappingProperties propertiesMapping)
        {
            return AssigmentHelper.CanAssign(propertiesMapping) && propertiesMapping.Destination.CanWrite;
        }
        
        private static Func<TSource, TDestination> CreateMappingFunction<TSource, TDestination>(IEnumerable<MappingProperties> mappingPropertiesList) where TDestination : new()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TSource), "source");
            var memberBindings = mappingPropertiesList.Select((mapping) => GetMemberBinding(parameterExpression, mapping));
            Expression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TDestination)), memberBindings);
            return Expression.Lambda<Func<TSource, TDestination>>(memberInitExpression, parameterExpression).Compile();
        }

        private static MemberBinding GetMemberBinding(Expression parameterExpression, MappingProperties mappingProperties)
        {
            Expression propertyAccessExpression = Expression.Property(parameterExpression, mappingProperties.Source);
            Expression convertExpression = Expression.Convert(propertyAccessExpression, mappingProperties.Destination.PropertyType);
            return Expression.Bind(mappingProperties.Destination, convertExpression);
        }

        private static List<MappingProperties> GetMappingProperties(MappingTypes mappingTypes)
        {
            return (
                from sourceProperty in mappingTypes.Source.GetProperties()
                join destinationProperty in mappingTypes.Destination.GetProperties()
                on sourceProperty.Name equals destinationProperty.Name
                let mappingProperties = new MappingProperties(sourceProperty, destinationProperty)
                where CanMapProperties(mappingProperties)
                select mappingProperties
            ).ToList();
        }
    }
}

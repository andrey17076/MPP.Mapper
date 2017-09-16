namespace Mapper
{
    internal interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source) where TDestination : new();
    }
}

namespace Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;
using Orleans.StorageProviderInterceptors.Abstractions;
using Orleans.StorageProviderInterceptors.Infrastructure;
using Tester.StorageFacet.Abstractions;
using Tester.StorageFacet.Infrastructure;

/// <summary>
/// TODO
/// </summary>
public static class StorageInterceptorExtensions
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="builder"></param>
    public static ISiloBuilder UseStorageInterceptor(this ISiloBuilder builder) => builder.ConfigureServices(services =>
{
    // storage feature factory infrastructure
    services.AddTransient<INamedStorageInterceptorFactory, NamedStorageInterceptorFactory>();

    // storage feature facet attribute mapper
    services.AddSingleton(typeof(IAttributeToFactoryMapper<StorageInterceptorAttribute>), typeof(StorageInterceptorAttributeMapper));
});

    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="TFactoryType"></typeparam>
    /// <param name="builder"></param>
    public static ISiloBuilder UseAsDefaultStorageInterceptor<TFactoryType>(this ISiloBuilder builder)
        where TFactoryType : class, IStorageInterceptorFactory => builder.ConfigureServices(services => services.AddTransient<IStorageInterceptorFactory, TFactoryType>());
}

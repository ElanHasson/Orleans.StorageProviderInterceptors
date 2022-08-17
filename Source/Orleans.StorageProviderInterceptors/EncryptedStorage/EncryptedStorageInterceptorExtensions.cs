namespace Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;
using Orleans.StorageProviderInterceptors.EncryptedStorage;
using Tester.StorageFacet.Abstractions;
using Tester.StorageFacet.Implementations;

/// <summary>
/// TODO
/// </summary>
public static class EncryptedStorageInterceptorExtensions
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name"></param>
    public static void UseEncryptedStorageInterceptor(this ISiloBuilder builder, string name) => builder.ConfigureServices(services =>
                                                                                                 {
                                                                                                     services.AddTransientNamedService<IStorageInterceptorFactory, EncryptedStorageInterceptorFactory>(name);
                                                                                                     services.AddTransient(typeof(EncryptedStorageInterceptor<>));
                                                                                                 });
}

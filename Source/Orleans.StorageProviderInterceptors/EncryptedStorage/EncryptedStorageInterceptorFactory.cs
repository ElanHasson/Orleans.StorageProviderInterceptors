namespace Tester.StorageFacet.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;
using Orleans.StorageProviderInterceptors.EncryptedStorage;
using Tester.StorageFacet.Abstractions;

/// <summary>
/// TODO
/// </summary>
public class EncryptedStorageInterceptorFactory : IStorageInterceptorFactory
{
    private readonly IGrainActivationContext context;
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="context"></param>
    public EncryptedStorageInterceptorFactory(IGrainActivationContext context) => this.context = context;

    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="config"></param>
    public IStorageInterceptor<TState> Create<TState>(IStorageInterceptorConfig config)
    {
        var storage = this.context.ActivationServices.GetRequiredService<EncryptedStorageInterceptor<TState>>();
        storage.Configure(config);
        return storage;
    }
}

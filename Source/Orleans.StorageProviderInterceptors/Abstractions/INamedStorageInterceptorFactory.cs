namespace Orleans.StorageProviderInterceptors.Abstractions;
using Tester.StorageFacet.Abstractions;

using Orleans.Runtime;

/// <summary>
/// Creates a storage feature by name from a configuration
/// </summary>
public interface INamedStorageInterceptorFactory
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="context"></param>
    /// <param name="config"></param>
    IPersistentState<TState> Create<TState>(IGrainActivationContext context, IStorageInterceptorConfig config);
}

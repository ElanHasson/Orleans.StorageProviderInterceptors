namespace Orleans.StorageProviderInterceptors.Abstractions;

using Orleans.Runtime;

using Orleans.Storage;

/// <summary>
/// Creates a storage feature from a configuration
/// </summary>
public interface IStorageInterceptorFactory
{
    /// <summary>
    /// Creates a storage feature from a configuration
    /// </summary>
    /// <param name="context"></param>
    /// <param name="config"></param>
    /// <param name="fullStateName"></param>
    /// <param name="storageProvider"></param>
    IStorageInterceptor Create(IGrainContext context, IStorageInterceptorConfig config, string fullStateName, IGrainStorage storageProvider);
}

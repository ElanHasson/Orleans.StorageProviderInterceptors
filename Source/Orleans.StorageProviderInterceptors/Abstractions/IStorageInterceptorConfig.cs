namespace Orleans.StorageProviderInterceptors.Abstractions;

using Orleans.Runtime;
using Orleans.Storage;

/// <summary>
/// Feature configuration information which application layer can provide to the
///  feature per instance (by grain type if using attributes).
/// </summary>
public interface IStorageInterceptorConfig
{
    /// <summary>
    /// The State Name to Intercept.
    /// </summary>
    string StateName { get; }

    /// <summary>
    /// The Storage Provider Name to Intercept.
    /// </summary>
    string StorageName { get; }
}

/// <summary>
/// TODO
/// </summary>
/// <typeparam name="TState"></typeparam>
public interface IStorageInterceptorFullConfig<TState> : IStorageInterceptorConfig
{
    /// <summary>
    /// .
    /// </summary>
    IGrainContext Context { get; set; }
    /// <summary>
    /// .
    /// </summary>
    string FullStateName { get; set; }
    /// <summary>
    /// .
    /// </summary>
    IGrainStorage StorageProvider { get; set; }
}
/// <summary>
/// d
/// </summary>
/// <typeparam name="TState"></typeparam>
public class StorageInterceptorFullConfig<TState> : IStorageInterceptorFullConfig<TState>
{
    /// <summary>
    /// .
    /// </summary>
    /// <param name="stateName"></param>
    /// <param name="storageName"></param>
    public StorageInterceptorFullConfig(string stateName, string storageName)
    {
        this.StateName = stateName;
        this.StorageName = storageName;
    }
    /// <summary>
    /// .
    /// </summary>
    public IGrainContext Context { get; set; } = default!;
    /// <summary>
    /// .
    /// </summary>
    public string FullStateName { get; set; } = string.Empty;
    /// <summary>
    /// .
    /// </summary>
    public IPersistentState<TState> PersistentState { get; set; } = default!;
    /// <summary>
    /// .
    /// </summary>
    public IGrainStorage StorageProvider { get; set; } = default!;

    /// <summary>
    /// .
    /// </summary>
    public string StateName { get; }
    /// <summary>
    /// .
    /// </summary>
    public string StorageName { get; }

    /// <summary>
    /// State
    /// </summary>
    public TState? State { get; set; }
}

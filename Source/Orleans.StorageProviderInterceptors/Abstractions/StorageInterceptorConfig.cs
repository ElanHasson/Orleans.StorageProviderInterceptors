namespace Orleans.StorageProviderInterceptors.Abstractions;

/// <summary>
/// Feature configuration utility class
/// </summary>
public class StorageInterceptorConfig : IStorageInterceptorConfig
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="stateName"></param>
    /// <param name="storageName"></param>
    public StorageInterceptorConfig(string stateName, string storageName)
    {
        this.StateName = stateName;
        this.StorageName = storageName;
    }

    /// <summary>
    /// TODO
    /// </summary>
    public string StateName { get; }

    /// <summary>
    /// TODO
    /// </summary>
    public string StorageName { get; }
}

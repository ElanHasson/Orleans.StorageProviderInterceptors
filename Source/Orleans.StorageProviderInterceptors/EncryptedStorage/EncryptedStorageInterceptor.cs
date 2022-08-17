namespace Orleans.StorageProviderInterceptors.EncryptedStorage;
using System.Threading.Tasks;
using Tester.StorageFacet.Abstractions;

/// <summary>
/// todo
/// </summary>
/// <typeparam name="TState"></typeparam>
public class EncryptedStorageInterceptor<TState> : IStorageInterceptor<TState>
{
    private IStorageInterceptorConfig config = default!;

    /// <summary>
    /// TODO
    /// </summary>
    public string Name => this.config.StateName;

    /// <summary>
    /// TODO
    /// </summary>
    public TState State { get; set; } = default!;

    /// <summary>
    /// TODO
    /// </summary>
    public Task Save() => Task.CompletedTask;

    /// <summary>
    /// TODO
    /// </summary>
    public string GetExtendedInfo() => $"Blob:{this.Name}, StateType:{typeof(TState).Name}";

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="cfg"></param>
    public void Configure(IStorageInterceptorConfig cfg) => this.config = cfg;
}

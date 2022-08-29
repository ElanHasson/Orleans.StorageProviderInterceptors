namespace Orleans.StorageProviderInterceptors.EncryptedStorage;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Tester.StorageFacet.Abstractions;

/// <summary>
/// todo
/// </summary>
/// <typeparam name="TState"></typeparam>
public class EncryptedStorageInterceptor<TState> : IStorageInterceptor<TState>, IStorageInterceptor
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
    /// <summary>Called before the Delete / Clear data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">Copy of last-known state data object for this grain.</param>
    /// <returns>Completion promise for the Delete operation on the specified grain.</returns>
    public ValueTask OnBeforeClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => ValueTask.CompletedTask;
    /// <summary>Called after the Delete / Clear data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">Copy of last-known state data object for this grain.</param>
    /// <returns>Completion promise for the Delete operation on the specified grain.</returns>
    public ValueTask OnAfterClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => ValueTask.CompletedTask;
    /// <summary>Called before the Read data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be populated for this grain.</param>
    /// <returns>Completion promise for the Read operation on the specified grain.</returns>
    public ValueTask OnBeforeReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => ValueTask.CompletedTask;
    /// <summary>Called after the Read data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be populated for this grain.</param>
    /// <returns>Completion promise for the Read operation on the specified grain.</returns>
    public ValueTask OnAfterReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => ValueTask.CompletedTask;
    /// <summary>Called before the Write data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be written for this grain.</param>
    /// <returns>Completion promise for the Write operation on the specified grain.</returns>
    public ValueTask OnBeforeWriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => ValueTask.CompletedTask;
    /// <summary>Called after the Write data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be written for this grain.</param>
    /// <returns>Completion promise for the Write operation on the specified grain.</returns>
    public ValueTask OnAfterWriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => ValueTask.CompletedTask;
}

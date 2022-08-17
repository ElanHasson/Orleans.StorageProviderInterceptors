namespace Orleans.StorageProviderInterceptors;

using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;

/// <summary>
/// IGrainStorageInterceptor defines an interceptor.
/// </summary>
public interface IGrainStorageInterceptor
{
    /// <summary>Called before the Delete / Clear data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">Copy of last-known state data object for this grain.</param>
    /// <returns>Completion promise for the Delete operation on the specified grain.</returns>
    ValueTask OnBeforeClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState);

    /// <summary>Called after the Delete / Clear data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">Copy of last-known state data object for this grain.</param>
    /// <returns>Completion promise for the Delete operation on the specified grain.</returns>
    ValueTask OnAfterClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState);

    /// <summary>Called before the Read data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be populated for this grain.</param>
    /// <returns>Completion promise for the Read operation on the specified grain.</returns>
    public ValueTask OnBeforeReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState);

    /// <summary>Called after the Read data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be populated for this grain.</param>
    /// <returns>Completion promise for the Read operation on the specified grain.</returns>
    public ValueTask OnAfterReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState);

    /// <summary>Called before the Write data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be written for this grain.</param>
    /// <returns>Completion promise for the Write operation on the specified grain.</returns>
    public ValueTask OnBeforeWriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState);

    /// <summary>Called after the Write data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be written for this grain.</param>
    /// <returns>Completion promise for the Write operation on the specified grain.</returns>
    public ValueTask OnAfterWriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState);
}

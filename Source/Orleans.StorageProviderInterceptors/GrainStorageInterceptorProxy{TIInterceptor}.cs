namespace Orleans.StorageProviderInterceptors;

using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Orleans.Storage;

/// <summary>
/// GrainStorageInterceptorProxy is responsable for performing the interception of the given storage provider.
/// </summary>
public class GrainStorageInterceptorProxy : IGrainStorage, ILifecycleParticipant<ISiloLifecycle>
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Creates an instance of GrainStorageInterceptorProxy.
    /// </summary>
    /// <param name="serviceProvider">a service provider.</param>
    public GrainStorageInterceptorProxy(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.targetProvider = serviceProvider.Storage
    }

    /// <summary>Delete / Clear data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">Copy of last-known state data object for this grain.</param>
    /// <returns>Completion promise for the Delete operation on the specified grain.</returns>
    public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
    {
        
    }

    /// <summary>
    /// Provides hook to take part in lifecycle.
    /// Also may act as a signal interface indicating that an object can take part in lifecycle.
    /// </summary>
    /// <param name="lifecycle">The lifecycle event.</param>
    public void Participate(ISiloLifecycle lifecycle) => throw new NotImplementedException();

    /// <summary>Read data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be populated for this grain.</param>
    /// <returns>Completion promise for the Read operation on the specified grain.</returns>
    public Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => throw new NotImplementedException();

    /// <summary>Write data function for this storage instance.</summary>
    /// <param name="grainType">Type of this grain [fully qualified class name].</param>
    /// <param name="grainReference">Grain reference object for this grain.</param>
    /// <param name="grainState">State data object to be written for this grain.</param>
    /// <returns>Completion promise for the Write operation on the specified grain.</returns>
    public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => throw new NotImplementedException();
}

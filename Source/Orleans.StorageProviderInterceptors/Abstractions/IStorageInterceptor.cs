namespace Orleans.StorageProviderInterceptors.Abstractions;
using Orleans.Runtime;
using Orleans;

/// <summary>
/// Primary storage feature interface.
///  This is the actual functionality the users need.
/// </summary>
public interface IStorageInterceptor : ILifecycleParticipant<IGrainLifecycle>
{
    /// <summary>
    /// TODO
    /// </summary>
    string Name { get; }
}

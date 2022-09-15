namespace Orleans.Hosting;
using Orleans.Runtime;

/// <summary>
/// TODO
/// </summary>
/// <typeparam name="TState"></typeparam>
public class StorageInterceptorOptions<TState>
{
    /// <summary>
    /// Called before a ClearStateAsync(); return false to prevent writing.
    /// </summary>
    public Func<IGrainActivationContext, IPersistentState<TState>, ValueTask<bool>> OnBeforeClearStateAsync { get; set; } = (_, _) => ValueTask.FromResult(false);

    /// <summary>
    /// Called after ClearStateAsync();
    /// </summary>
    public Func<IGrainActivationContext, IPersistentState<TState>, ValueTask> OnAfterClearStateAsync { get; set; } = (_, _) => ValueTask.CompletedTask;

    /// <summary>
    /// Called before ReadStateAsync(); return false to prevent Reading
    /// </summary>
    public Func<IGrainActivationContext, IPersistentState<TState>, ValueTask<bool>> OnBeforeReadStateAsync { get; set; } = (_, _) => ValueTask.FromResult(false);

    /// <summary>
    /// Called after ReadStateAsync()
    /// </summary>
    public Func<IGrainActivationContext, IPersistentState<TState>, ValueTask> OnAfterReadStateFunc { get; set; } = (_, _) => ValueTask.CompletedTask;

    /// <summary>
    /// Called before WriteStateAsync(); return false to prevent writing
    /// </summary>
    public Func<IGrainActivationContext, IPersistentState<TState>, ValueTask<bool>> OnBeforeWriteStateFunc { get; set; } = (_, _) => ValueTask.FromResult(true);

    /// <summary>
    /// Called after WriteStateAsync()
    /// </summary>
    public Func<IGrainActivationContext, IPersistentState<TState>, ValueTask> OnAfterWriteStateFunc { get; set; } = (_, _) => ValueTask.CompletedTask;
}

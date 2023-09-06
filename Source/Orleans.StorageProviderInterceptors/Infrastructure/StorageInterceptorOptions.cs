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
    public Func<IGrainContext, IPersistentState<TState>, ValueTask<(bool PreventOperation, object? SharedState)>> OnBeforeClearStateAsync { get; set; } = (_, _) => ValueTask.FromResult((false, (object?)null));

    /// <summary>
    /// Called after ClearStateAsync();
    /// </summary>
    public Func<IGrainContext, IPersistentState<TState>, object?, ValueTask> OnAfterClearStateAsync { get; set; } = (_, _, _) => ValueTask.CompletedTask;

    /// <summary>
    /// Called before ReadStateAsync(); return false to prevent Reading
    /// </summary>
    public Func<IGrainContext, IPersistentState<TState>, ValueTask<(bool PreventOperation, object? SharedState)>> OnBeforeReadStateAsync { get; set; } = (_, _) => ValueTask.FromResult((false, (object?)null));

    /// <summary>
    /// Called after ReadStateAsync()
    /// </summary>
    public Func<IGrainContext, IPersistentState<TState>, object?, ValueTask> OnAfterReadStateFunc { get; set; } = (_, _, _) => ValueTask.CompletedTask;

    /// <summary>
    /// Called before WriteStateAsync(); return false to prevent writing
    /// </summary>
    public Func<IGrainContext, IPersistentState<TState>, ValueTask<(bool PreventOperation, object? SharedState)>> OnBeforeWriteStateFunc { get; set; } = (_, _) => ValueTask.FromResult((false, (object?)null));

    /// <summary>
    /// Called after WriteStateAsync()
    /// </summary>
    public Func<IGrainContext, IPersistentState<TState>, object?, ValueTask> OnAfterWriteStateFunc { get; set; } = (_, _, _) => ValueTask.CompletedTask;
}

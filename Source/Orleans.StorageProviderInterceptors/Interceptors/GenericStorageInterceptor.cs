//namespace Orleans.StorageProviderInterceptors.Interceptors;

//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Orleans;
//using Orleans.Core;
//using Orleans.Hosting;
//using Orleans.Runtime;
//using Tester.StorageFacet.Abstractions;

///// <summary>
///// todo
///// </summary>
///// <typeparam name="TState"></typeparam>
//public class GenericStorageInterceptor<TState> : IStorageInterceptor, IStorage<TState>
//{
//    private IStorageInterceptorFullConfig<TState> config = default!;
//    private StorageInterceptorOptions<TState> options = default!;
//    private StateStorageBridge<TState> storage = default!;

//    /// <summary>
//    /// TODO
//    /// </summary>
//    public string Name => this.config.StateName;
//    internal void Configure(StorageInterceptorOptions<TState> options, IStorageInterceptorFullConfig<TState> config)
//    {
//        this.config = config;
//        this.options = options;
//    }

//    /// <summary>
//    /// TODO
//    /// </summary>
//    public TState State { get => this.storage..PersistentState.State; set => _ = this.config.PersistentState.State; }
//    /// <inheritdoc/>
//    public string Etag => this.config.PersistentState.Etag;
//    /// <inheritdoc/>
//    public bool RecordExists => this.config.PersistentState.RecordExists;

//    /// <inheritdoc/>
//    public void Participate(IGrainLifecycle lifecycle) => lifecycle.Subscribe(this.GetType().FullName, GrainLifecycleStage.SetupState, this.OnSetupState);

//    private Task OnSetupState(CancellationToken ct)
//    {
//        if (ct.IsCancellationRequested)
//        {
//            return Task.CompletedTask;
//        }

//        this.storage = new StateStorageBridge<TState>(this.config.FullStateName, this.config.Context.GrainInstance.GrainReference, this.config.StorageProvider, this.config.Context.ActivationServices.GetService<ILoggerFactory>());
//        return this.ReadStateAsync();
//    }

//    /// <inheritdoc/>
//    public async Task ClearStateAsync()
//    {
//        this.storage.ToString();
//        await this.options.OnBeforeClearStateFunc(this.config.Context, this.config.PersistentState);
//        await this.config.PersistentState.ClearStateAsync();
//        await this.options.OnAfterClearStateFunc(this.config.Context, this.config.PersistentState);
//    }

//    /// <inheritdoc/>
//    public async Task WriteStateAsync()
//    {
//        await this.options.OnBeforeWriteStateFunc(this.config.Context, this.config.PersistentState);
//        await this.config.PersistentState.WriteStateAsync();
//        await this.options.OnAfterWriteStateFunc(this.config.Context, this.config.PersistentState);
//    }

//    /// <inheritdoc/>
//    public async Task ReadStateAsync()
//    {
//        await this.options.OnBeforeReadStateFunc(this.config.Context, this.config.PersistentState);
//        await this.config.PersistentState.ReadStateAsync();
//        await this.options.OnAfterReadStateFunc.Invoke(this.config.Context, this.config.PersistentState);
//    }
//}

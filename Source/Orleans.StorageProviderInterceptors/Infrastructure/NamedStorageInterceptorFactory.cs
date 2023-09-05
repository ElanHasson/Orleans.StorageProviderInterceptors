namespace Orleans.StorageProviderInterceptors.Infrastructure;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Core;
using Orleans;
using Orleans.Runtime;
using Orleans.Storage;
using Orleans.StorageProviderInterceptors.Abstractions;
using Orleans.Hosting;
using Orleans.Serialization.TypeSystem;

/// <summary>
/// TODO
/// </summary>
public class NamedStorageInterceptorFactory : INamedStorageInterceptorFactory
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="services"></param>
    public NamedStorageInterceptorFactory(IServiceProvider services) => this.services = services;

    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="context"></param>
    /// <param name="config"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public IPersistentState<TState> Create<TState>(IGrainContext context, IStorageInterceptorConfig config)
    {
        //var factory = string.IsNullOrEmpty(config.StorageName)
        //           ? this.services.GetService<IStorageInterceptorFactory>()
        //           : this.services.GetServiceByName<IStorageInterceptorFactory>(config.StorageName);
        //if (factory is null)
        //{
        //    throw new InvalidOperationException($"Interceptor with name {config.StorageName} not found.");
        //}

        var underlyingStorageProvider = !string.IsNullOrWhiteSpace(config.StorageName)
            ? context.ActivationServices.GetServiceByName<IGrainStorage>(config.StorageName)
            : context.ActivationServices.GetService<IGrainStorage>();
        if (underlyingStorageProvider is null)
        {
            ThrowMissingProviderException(context, config);
        }
        ArgumentNullException.ThrowIfNull(underlyingStorageProvider);

        var fullStateName = this.GetFullStateName(context, config);
        var options = this.services.GetRequiredServiceByName<StorageInterceptorOptions<TState>>($"{config.StorageName}-{config.StateName}");

        var bridge = new PersistentStateBridge<TState>(fullStateName, context, underlyingStorageProvider, options);

        bridge.Participate(context.ObservableLifecycle);

        return bridge;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cfg"></param>
    protected virtual string GetFullStateName(IGrainContext context, IStorageInterceptorConfig cfg) => $"{RuntimeTypeNameFormatter.Format(context.GrainInstance!.GetType())}.{cfg.StateName}";

    private static void ThrowMissingProviderException(IGrainContext context, IStorageInterceptorConfig cfg)
    {
        string errMsg;
        var grainTypeName = context.GrainInstance!.GetType();
        if (string.IsNullOrEmpty(cfg.StorageName))
        {
            errMsg = $"No default storage provider found loading grain type {grainTypeName}.";
        }
        else
        {
            errMsg = $"No storage provider named \"{cfg.StorageName}\" found loading grain type {grainTypeName}.";
        }

        throw new InvalidOperationException(errMsg);
    }

    /// <summary>
    /// Returns the non-generic type name without any special characters.
    /// </summary>
    /// <param name="type">
    /// The type.
    /// </param>
    /// <returns>
    /// The non-generic type name without any special characters.
    /// </returns>
    public static string GetUnadornedTypeName(Type type)
    {
        var index = type.Name.IndexOf('`');

        // An ampersand can appear as a suffix to a by-ref type.
        return (index > 0 ? type.Name[..index] : type.Name).TrimEnd('&');
    }
    private readonly IServiceProvider services;

    /// <inheritdoc/>
    public IPersistentState<TState> Create<TState>(IGrainContext context, IStorageInterceptorFullConfig<TState> config) => throw new NotImplementedException();

    private sealed class PersistentStateBridge<TState> : IPersistentState<TState>, ILifecycleParticipant<IGrainLifecycle>
    {
        private readonly string fullStateName;
        private readonly IGrainContext context;
        private readonly IGrainStorage storageProvider;
        private readonly StorageInterceptorOptions<TState> options;
        private StateStorageBridge<TState> storage = default!;

        public PersistentStateBridge(string fullStateName, IGrainContext context, IGrainStorage storageProvider, StorageInterceptorOptions<TState> options)
        {
            ArgumentNullException.ThrowIfNull(fullStateName);
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(storageProvider);
            ArgumentNullException.ThrowIfNull(options);

            this.fullStateName = fullStateName;
            this.context = context;
            this.storageProvider = storageProvider;
            this.options = options;
        }

        public TState State
        {
            get => this.storage.State;
            set => this.storage.State = value;
        }

        public string? Etag => this.storage.Etag;

        public bool RecordExists => this.storage.RecordExists;

        /// <inheritdoc/>
        public async Task ClearStateAsync()
        {
            var (preventOperation, state) = await this.options.OnBeforeClearStateAsync(this.context, this);
            if (!preventOperation)
            {
                await this.storage.ClearStateAsync();
                await this.options.OnAfterClearStateAsync(this.context, this, state);
            }
        }

        /// <inheritdoc/>
        public async Task WriteStateAsync()
        {
            var (preventOperation, state) = await this.options.OnBeforeWriteStateFunc(this.context, this);
            if (!preventOperation)
            {
                await this.storage.WriteStateAsync();
                await this.options.OnAfterWriteStateFunc(this.context, this, state);
            }
        }

        /// <inheritdoc/>
        public async Task ReadStateAsync()
        {
            var (preventOperation, state) = await this.options.OnBeforeReadStateAsync(this.context, this);
            if (!preventOperation)
            {
                await this.storage.ReadStateAsync();
                await this.options.OnAfterReadStateFunc.Invoke(this.context, this, state);
            }
        }

        public void Participate(IGrainLifecycle lifecycle) => lifecycle.Subscribe(this.GetType().FullName, GrainLifecycleStage.SetupState, this.OnSetupState);

        private Task OnSetupState(CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            this.storage = new StateStorageBridge<TState>(this.fullStateName, this.context, this.storageProvider, this.context.ActivationServices.GetRequiredService<ILoggerFactory>());
            return this.ReadStateAsync();
        }
    }
}

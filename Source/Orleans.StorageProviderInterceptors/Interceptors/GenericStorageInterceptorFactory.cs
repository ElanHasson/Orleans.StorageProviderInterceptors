//namespace Orleans.StorageProviderInterceptors.Interceptors;
//using Microsoft.Extensions.DependencyInjection;
//using Orleans.Hosting;
//using Orleans.Runtime;
//using Orleans.Storage;
//using Tester.StorageFacet.Abstractions;

///// <summary>
///// TODO
///// </summary>
//public class GenericStorageInterceptorFactory : IStorageInterceptorFactory
//{
//    private readonly IGrainContext context;
//    /// <summary>
//    /// TODO
//    /// </summary>
//    /// <param name="context"></param>
//    public GenericStorageInterceptorFactory(IGrainContext context) => this.context = context;
//    /// <inheritdoc/>
//    public IStorageInterceptor Create<TState>(IGrainContext context, IStorageInterceptorConfig config, string fullStateName, IGrainStorage storageProvider)
//    {
//        var storage = this.context.ActivationServices.GetRequiredService<GenericStorageInterceptor<TState>>();
//        var options = this.context.ActivationServices.GetRequiredServiceByName<StorageInterceptorOptions<TState>>(config.StorageName);

//        var fullConfig = (IStorageInterceptorFullConfig<TState>)config;
//        var bridge = new PersistentStateBridge<TState>(fullStateName, context, underlyingStorageProvider!);

//        fullConfig.Context = context;
//        fullConfig.PersistentState = persistentState;
//        fullConfig.FullStateName = fullStateName;
//        fullConfig.State = state;
//        fullConfig.StorageProvider = storageProvider;
//        storage.Configure(options, fullConfig);
//        return storage;
//    }
//}

namespace Orleans.Hosting;
using Orleans.Runtime;

/// <summary>
/// TODO
/// </summary>
public static class GenericStorageInterceptorExtensions
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="builder"></param>
    /// <param name="storageName"></param>
    /// <param name="stateName"></param>
    /// <param name="options"></param>
    public static ISiloBuilder UseGenericStorageInterceptor<TState>(this ISiloBuilder builder, string storageName, string stateName, Action<StorageInterceptorOptions<TState>> options) => builder.ConfigureServices(services =>
    {
        var opts = new StorageInterceptorOptions<TState>();
        options.Invoke(opts);
        services.AddSingletonNamedService($"{storageName}-{stateName}", (_, _) => opts);
        //services.AddTransientNamedService<IStorageInterceptorFactory, GenericStorageInterceptorFactory>(name);
        //services.AddTransientNamedService<IStorageInterceptorFactory, GenericStorageInterceptorFactory>(name);
        //services.AddTransient(typeof(GenericStorageInterceptor<>));
    });
}

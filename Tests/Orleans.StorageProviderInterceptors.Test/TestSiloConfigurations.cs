namespace Orleans.StorageProviderInterceptors.Test;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Orleans.TestingHost;

/// <inheritdoc />
public class TestSiloConfigurations : ISiloConfigurator, IHostConfigurator
{
    internal const string StorageName = "SecretsStorage";

    /// <inheritdoc />
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.UseInMemoryReminderService();
        siloBuilder.AddMemoryStreams("Default", config =>
        {
            config.ConfigureStreamPubSub(StreamPubSubType.ImplicitOnly);
            config.ConfigureCacheEviction(builder => builder.Configure(options =>
            {
                options.DataMinTimeInCache = TimeSpan.FromMinutes(2);
                options.DataMaxAgeInCache = TimeSpan.FromMinutes(10);
                options.MetadataMinTimeInCache = TimeSpan.FromMinutes(60);
            }));
        });
        siloBuilder
            .AddStorageInterceptors()
            .AddMemoryGrainStorage(StorageName)
            .AddStorageInterceptors()
            .UseGenericStorageInterceptor<Dictionary<string, string>>(
                StorageName, "secretsState", c =>
            {
                c.OnBeforeWriteStateFunc = (grainActivationContext, currentState) =>
                {
                    var unencryptedValues = new Dictionary<string, string>(currentState.State.Count);
                    Console.WriteLine(
                        $"OnBeforeWriteState: {grainActivationContext}: Count Is {currentState.State.Count}");
                    foreach (var (key, value) in currentState.State)
                    {
                        Console.WriteLine($"Intercepted: {key}: {value}");

                        // Save the original state to return to the grain
                        unencryptedValues.Add(key, value);

                        // Encrypt the data
                        currentState.State[key] = currentState.State[key].Replace('e', '3');
                    }

                    return ValueTask.FromResult((false, (object?)unencryptedValues));
                };

                c.OnAfterWriteStateFunc = (grainActivationContext, currentState, sharedState) =>
                {
                    var unencryptedValues = (Dictionary<string, string>)sharedState!;
                    Console.WriteLine(
                        $"OnAfterWriteState: {grainActivationContext}: Count Is {currentState.State.Count}");
                    foreach (var (key, value) in currentState.State)
                    {
                        Console.WriteLine($"What was actually persisted: {key}: {value}");

                        currentState.State[key] = unencryptedValues[key];
                        Console.WriteLine($"What will be returned to grain: {key}: {unencryptedValues[key]}");
                    }

                    return ValueTask.CompletedTask;
                };

                c.OnBeforeReadStateAsync = (grainActivationContext, currentState) =>
                {
                    Console.WriteLine(
                        $"OnBeforeReadState: {grainActivationContext}: Count Is {currentState.State.Count}");

                    var unencryptedValues = new Dictionary<string, string>(currentState.State.Count);
                    foreach (var (key, value) in currentState.State)
                    {
                        Console.WriteLine($"Unencrypted Values: {key}: {value}");

                        // Save the original state to return to the grain
                        unencryptedValues.Add(key, value);
                    }

                    return ValueTask.FromResult((false, (object?)unencryptedValues));
                };

                c.OnAfterReadStateFunc = (grainActivationContext, currentState, sharedState) =>
                {
                    var unencryptedValues = (Dictionary<string, string>)sharedState!;
                    if (!currentState.RecordExists)
                    {
                        return ValueTask.CompletedTask;
                    }

                    var list = sharedState as List<string>;
                    Console.WriteLine(
                        $"OnAfterReadState: {grainActivationContext}: Count Is {currentState.State.Count}");

                    foreach (var (key, value) in currentState.State)
                    {
                        Console.WriteLine($"Encrypted Values: {key}: {value}");

                        // Decrypt the data
                        currentState.State[key] = currentState.State[key].Replace('3', 'e');
                    }

                    return ValueTask.CompletedTask;
                };
            });
    }

    /// <inheritdoc />
    public virtual void Configure(IHostBuilder hostBuilder)
    {
        AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);

        hostBuilder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });

        hostBuilder.ConfigureServices((context, services) =>
        {
            // grab the cluster id that owns this silo
            var clusterId = context.Configuration["TestClusterId"];
        });
    }
}

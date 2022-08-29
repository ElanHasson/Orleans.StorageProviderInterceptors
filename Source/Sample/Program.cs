using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using Sample;
using Tester.StorageFacet.Abstractions;

// Configure the host

var host = Host.CreateDefaultBuilder()
    .UseOrleans(
        builder => builder
            .UseLocalhostClustering()
            //Quiet down the silo for the demo.
            .Configure<StatisticsOptions>(c => c.CollectionLevel = Orleans.Runtime.Configuration.StatisticsLevel.Critical)
            .ConfigureLogging(c => c.SetMinimumLevel(LogLevel.None))
            .AddMemoryGrainStorage("SecretsStorage")
            .UseStorageInterceptor()
            .UseGenericStorageInterceptor<Dictionary<string, string>>("SecretsStorage", "secretsState", c =>
            {
                c.OnBeforeReadStateAsync = (grainActivationContext, currentState) =>
                {
                    Console.WriteLine($"OnBeforeReadState: {grainActivationContext.GrainIdentity.IdentityString}: Count Is {currentState.State.Count}");
                    return ValueTask.CompletedTask;
                };

                c.OnAfterReadStateFunc = (grainActivationContext, currentState) =>
                {
                    Console.WriteLine($"OnAfterReadState: {grainActivationContext.GrainIdentity.IdentityString}: Count Is {currentState.State.Count}");

                    foreach (var (key, value) in currentState.State)
                    {
                        Console.WriteLine($"Intercepted: {key}: {value}");

                        // Decrypt the data
                        currentState.State[key] = currentState.State[key].Replace('3', 'e');
                    }
                    return ValueTask.CompletedTask;
                };

                c.OnBeforeWriteStateFunc = (grainActivationContext, currentState) =>
                {
                    Console.WriteLine($"OnBeforeWriteState: {grainActivationContext.GrainIdentity.IdentityString}: Count Is {currentState.State.Count}");
                    foreach (var (key, value) in currentState.State)
                    {
                        Console.WriteLine($"Intercepted: {key}: {value}");

                        // Encrypt the data
                        currentState.State[key] = currentState.State[key].Replace('e', '3');
                    }
                    return ValueTask.CompletedTask;
                };

                c.OnAfterWriteStateFunc = (grainActivationContext, currentState) =>
                {
                    Console.WriteLine($"OnAfterWriteState: {grainActivationContext.GrainIdentity.IdentityString}: Count Is {currentState.State.Count}");
                    foreach (var (key, value) in currentState.State)
                    {
                        Console.WriteLine($"What was actually persisted: {key}: {value}");

                    }
                    return ValueTask.CompletedTask;
                };

            }))
    .Build();

// Start the host

await host.StartAsync();

// Get the grain factory

var grainFactory = host.Services.GetRequiredService<IGrainFactory>();

// Get a reference to the HelloGrain grain with the key "friend"

var secretStore = grainFactory.GetGrain<ISecretStorageGrain>("friend");

// Call the grain and print the result to the console
await secretStore.AddSecret("Password", "Now you See the secrets and now they are seen as safe!");

var result = await secretStore.GetSecret("Password");

Console.WriteLine("\n\n{0}\n\n", result);

Console.ReadLine();

await host.StopAsync();

namespace Sample
{
    internal class SecretStorageGrain : Grain, ISecretStorageGrain
    {
        private readonly IPersistentState<Dictionary<string, string>> secrets;

        public SecretStorageGrain([StorageInterceptor("SecretsStorage", "secretsState")] IPersistentState<Dictionary<string, string>> state) => this.secrets = state;
        public async Task AddSecret(string name, string value)
        {
            this.secrets.State.Add(name, value);
            await this.secrets.WriteStateAsync();
        }

        public Task<string> GetSecret(string name) => Task.FromResult(this.secrets.State[name]);
    }

    internal interface ISecretStorageGrain : IGrainWithStringKey
    {
        Task AddSecret(string name, string value);
        Task<string> GetSecret(string name);
    }
}

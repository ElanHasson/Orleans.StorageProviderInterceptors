using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;
using Sample;
using Tester.StorageFacet.Abstractions;
using Tester.StorageFacet.Implementations;

// Configure the host

var host = Host.CreateDefaultBuilder()
    .UseOrleans(
        builder => builder
            .UseLocalhostClustering()
            .AddMemoryGrainStorage("Secrets")
            .UseStorageInterceptor()
            .UseAsDefaultStorageInterceptor<EncryptedStorageInterceptorFactory>()
            .UseEncryptedStorageInterceptor("Secrets"))
    .Build();

// Start the host

await host.StartAsync();

// Get the grain factory

var grainFactory = host.Services.GetRequiredService<IGrainFactory>();

// Get a reference to the HelloGrain grain with the key "friend"

var secretStore = grainFactory.GetGrain<ISecretStorageGrain>("friend");

// Call the grain and print the result to the console
await secretStore.AddSecret("Password", "123456789");

var result = await secretStore.GetSecret("Password");

Console.WriteLine("\n\n{0}\n\n", result);

Console.WriteLine("Orleans is running.\nPress Enter to terminate...");
Console.ReadLine();
Console.WriteLine("Orleans is stopping...");

await host.StopAsync();

namespace Sample
{
    internal class SecretStorageGrain : Grain, ISecretStorageGrain
    {
        private readonly IPersistentState<Dictionary<string, string>> secrets;

        public SecretStorageGrain([StorageInterceptor("Secrets", "secrets")] IPersistentState<Dictionary<string, string>> state) => this.secrets = state;
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

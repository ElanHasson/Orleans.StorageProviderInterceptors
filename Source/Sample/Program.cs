using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;
using Orleans.StorageProviderInterceptors;
using Sample;

// Configure the host

var host = Host.CreateDefaultBuilder()
    .ConfigureServices(c =>
    {
        c.AddSingleton<GrainStorageInterceptorProxy>();
    })
    .UseOrleans(
        builder => builder
            .UseLocalhostClustering()
            .AddMemoryGrainStorage("AccountState"))
    .Build();

// Start the host

await host.StartAsync();

// Get the grain factory

var grainFactory = host.Services.GetRequiredService<IGrainFactory>();

// Get a reference to the HelloGrain grain with the key "friend"

var friend = grainFactory.GetGrain<IHelloGrain>("friend");

// Call the grain and print the result to the console
var result = await friend.SayHello("Good morning!");
Console.WriteLine("\n\n{0}\n\n", result);

Console.WriteLine("Orleans is running.\nPress Enter to terminate...");
Console.ReadLine();
Console.WriteLine("Orleans is stopping...");

await host.StopAsync();

namespace Sample
{
    internal class HelloGrain : Grain, IHelloGrain
    {
        public Task<string> SayHello(string greeting) => Task.FromResult($"Hello, {greeting}!");
    }

    internal interface IHelloGrain : IGrainWithStringKey
    {
        Task<string> SayHello(string greeting);
    }

    internal class MyInterceptor : IGrainStorageInterceptor
    {
        public ValueTask OnAfterClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => throw new NotImplementedException();
        public ValueTask OnAfterReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => throw new NotImplementedException();
        public ValueTask OnAfterWriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => throw new NotImplementedException();
        public ValueTask OnBeforeClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => throw new NotImplementedException();
        public ValueTask OnBeforeReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => throw new NotImplementedException();
        public ValueTask OnBeforeWriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => throw new NotImplementedException();
    }
}

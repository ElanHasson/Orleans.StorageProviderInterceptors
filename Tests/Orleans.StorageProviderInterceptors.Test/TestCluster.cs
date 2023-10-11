namespace Orleans.StorageProviderInterceptors.Test;

using System;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.TestingHost;

public sealed class TestClusterFixture : IDisposable
{
    public string TestClusterId = new Guid().ToString();

    public Orleans.TestingHost.TestCluster Cluster { get; }

    public IClusterClient? ClusterClient { get; }


    public TestClusterFixture()
    {
        var builder = new TestClusterBuilder();

        builder.ConfigureHostConfiguration(config => config.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { nameof(this.TestClusterId), this.TestClusterId }
                }!));

        builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();

        this.Cluster = builder.Build();
        this.Cluster.Deploy();

        this.ClusterClient = (IClusterClient)this.Cluster.ServiceProvider.GetService(typeof(IClusterClient))!;
    }

    public void Dispose() => this.Cluster.StopAllSilos();
}

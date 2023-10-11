namespace Orleans.StorageProviderInterceptors.Test;

using Xunit;
using Xunit.Abstractions;

[Collection(TestClusterCollection.Name)]
public class BasicTests
{
    private readonly TestClusterFixture fixture;
    private readonly ITestOutputHelper helper;

    public BasicTests(TestClusterFixture fixture, ITestOutputHelper helper)
    {
        this.fixture = fixture;
        this.helper = helper;
    }

    [Fact]
    public async Task Validate_SecretStorageGrain_ReadWrite()
    {
        var grain = this.fixture.Cluster.GrainFactory.GetGrain<ISecretStorageGrain>("foo");
        await grain.AddOrUpdateSecret("bar", "meh");
        var secret = await grain.GetSecret("bar");
        Assert.Equal("meh", secret);
    }
}

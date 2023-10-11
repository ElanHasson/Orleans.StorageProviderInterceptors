namespace Orleans.StorageProviderInterceptors.Test;

using Xunit;

// Important note: Fixtures can be shared across assemblies, but collection definitions must be in the same assembly as the test that uses them.
// https://xunit.net/docs/shared-context
[CollectionDefinition(Name)]
public class TestClusterCollection : ICollectionFixture<TestClusterFixture>
{
    public const string Name = "TestClusterCollection";
}

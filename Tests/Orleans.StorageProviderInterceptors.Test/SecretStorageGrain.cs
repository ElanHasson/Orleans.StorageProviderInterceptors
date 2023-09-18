namespace Orleans.StorageProviderInterceptors.Test;

using Abstractions;
using Runtime;

internal sealed class SecretStorageGrain : Grain, ISecretStorageGrain
{
    private readonly IPersistentState<Dictionary<string, string>> secrets;

    public SecretStorageGrain(
        [StorageInterceptor(
            TestSiloConfigurations.StorageName, "secretsState")]
        IPersistentState<Dictionary<string, string>> state) => this.secrets = state;

    public async Task AddOrUpdateSecret(string name, string value)
    {
        this.secrets.State[name] = value;
        await this.secrets.WriteStateAsync();
    }

    public Task<string> GetSecret(string name) => Task.FromResult(this.secrets.State[name]);
}

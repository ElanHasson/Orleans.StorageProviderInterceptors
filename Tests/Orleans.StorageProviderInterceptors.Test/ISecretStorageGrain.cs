namespace Orleans.StorageProviderInterceptors.Test;

internal interface ISecretStorageGrain : IGrainWithStringKey
{
    Task AddOrUpdateSecret(string name, string value);
    Task<string> GetSecret(string name);
}

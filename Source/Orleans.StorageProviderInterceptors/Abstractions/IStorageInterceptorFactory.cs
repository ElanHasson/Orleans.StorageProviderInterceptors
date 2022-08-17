namespace Tester.StorageFacet.Abstractions;

/// <summary>
/// Creates a storage feature from a configuration
/// </summary>
public interface IStorageInterceptorFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="config"></param>
    /// <returns></returns>
    IStorageInterceptor<TState> Create<TState>(IStorageInterceptorConfig config);
}

namespace Tester.StorageFacet.Abstractions;

/// <summary>
/// Feature configuration information which application layer can provide to the
///  feature per instance (by grain type if using attributes).
/// </summary>
public interface IStorageInterceptorConfig
{
    /// <summary>
    /// 
    /// </summary>
    string StateName { get; }

    /// <summary>
    /// 
    /// </summary>
    string StorageName { get; }
}

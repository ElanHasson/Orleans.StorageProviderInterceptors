namespace Tester.StorageFacet.Abstractions;

/// <summary>
/// Feature configuration information which application layer can provide to the
///  feature per instance (by grain type if using attributes).
/// </summary>
public interface IStorageInterceptorConfig
{
    /// <summary>
    /// The State Name to Intercept.
    /// </summary>
    string StateName { get; }

    /// <summary>
    /// The Storage Provider Name to Intercept.
    /// </summary>
    string StorageName { get; }
}

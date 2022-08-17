namespace Tester.StorageFacet.Abstractions;
using System;
using Orleans;

/// <summary>
/// TODO
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class StorageInterceptorAttribute : Attribute, IFacetMetadata, IStorageInterceptorConfig
{
    /// <summary>
    /// TODO
    /// </summary>
    public string StorageName { get; }

    /// <summary>
    /// TODO
    /// </summary>
    public string StateName { get; }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="storageName"></param>
    /// <param name="stateName"></param>
    public StorageInterceptorAttribute(string storageName, string stateName)
    {
        this.StorageName = storageName;
        this.StateName = stateName;
    }
}

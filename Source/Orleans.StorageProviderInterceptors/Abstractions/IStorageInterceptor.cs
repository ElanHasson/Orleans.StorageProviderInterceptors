namespace Tester.StorageFacet.Abstractions;
using System.Threading.Tasks;

/// <summary>
/// Primary storage feature interface.
///  This is the actual functionality the users need.
/// </summary>
/// <typeparam name="TState"></typeparam>
public interface IStorageInterceptor<TState>
{
    /// <summary>
    /// TODO
    /// </summary>
    TState State { get; set; }

    /// <summary>
    /// TODO
    /// </summary>
    Task Save();

    /// <summary>
    /// TODO
    /// </summary>
    string Name { get; }

    /// <summary>
    /// TODO
    /// </summary>
    string GetExtendedInfo();
}

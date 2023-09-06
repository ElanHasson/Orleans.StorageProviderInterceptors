namespace Orleans.StorageProviderInterceptors.Infrastructure;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Runtime;
using Orleans.StorageProviderInterceptors.Abstractions;

/// <summary>
/// TODO
/// </summary>
public class StorageInterceptorAttributeMapper : IAttributeToFactoryMapper<StorageInterceptorAttribute>
{
    private static readonly MethodInfo CreateMethod = typeof(INamedStorageInterceptorFactory).GetMethod("Create") ?? throw new InvalidOperationException();

    /// <summary>
    /// Responsible for mapping a facet metadata to a cachable factory from the parameter and facet metadata.
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="metadata"></param>
    public Factory<IGrainContext, object> GetFactory(ParameterInfo parameter, StorageInterceptorAttribute metadata)
    {
        ArgumentNullException.ThrowIfNull(parameter);
        IStorageInterceptorConfig config = metadata;
        // set state name to parameter name, if not already specified
        if (string.IsNullOrEmpty(config.StateName))
        {
            config = new StorageInterceptorConfig(parameter.Name ?? string.Empty, parameter.Name ?? string.Empty);
        }
        var genericCreate = CreateMethod.MakeGenericMethod(parameter.ParameterType.GetGenericArguments());
        return context => Create(context, genericCreate, config);
    }

    private static object Create(IGrainContext context, MethodInfo genericCreate, IStorageInterceptorConfig config)
    {
        var factory = context.ActivationServices.GetRequiredService<INamedStorageInterceptorFactory>();
        var args = new object[] { context, config };
        return genericCreate.Invoke(factory, args)!;
    }
}

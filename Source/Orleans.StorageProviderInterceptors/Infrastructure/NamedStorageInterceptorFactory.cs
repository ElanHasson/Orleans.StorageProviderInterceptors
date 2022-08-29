namespace Orleans.StorageProviderInterceptors.Infrastructure;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Core;
using Orleans;
using Orleans.Runtime;
using Orleans.Storage;
using Orleans.Utilities;
using Tester.StorageFacet.Abstractions;
using System.Text;
using System.Collections.Concurrent;
using Orleans.StorageProviderInterceptors.Abstractions;
using Orleans.Hosting;

/// <summary>
/// TODO
/// </summary>
public class NamedStorageInterceptorFactory : INamedStorageInterceptorFactory
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="services"></param>
    public NamedStorageInterceptorFactory(IServiceProvider services) => this.services = services;

    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="context"></param>
    /// <param name="config"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public IPersistentState<TState> Create<TState>(IGrainActivationContext context, IStorageInterceptorConfig config)
    {
        //var factory = string.IsNullOrEmpty(config.StorageName)
        //           ? this.services.GetService<IStorageInterceptorFactory>()
        //           : this.services.GetServiceByName<IStorageInterceptorFactory>(config.StorageName);
        //if (factory is null)
        //{
        //    throw new InvalidOperationException($"Interceptor with name {config.StorageName} not found.");
        //}

        var underlyingStorageProvider = !string.IsNullOrWhiteSpace(config.StorageName)
            ? context.ActivationServices.GetServiceByName<IGrainStorage>(config.StorageName)
            : context.ActivationServices.GetService<IGrainStorage>();
        if (underlyingStorageProvider is null)
        {
            ThrowMissingProviderException(context, config);
        }
        ArgumentNullException.ThrowIfNull(underlyingStorageProvider);

        var fullStateName = this.GetFullStateName(context, config);
        var options = this.services.GetRequiredServiceByName<StorageInterceptorOptions<TState>>($"{config.StorageName}-{config.StateName}");

        var bridge = new PersistentStateBridge<TState>(fullStateName, context, underlyingStorageProvider, options);

        bridge.Participate(context.ObservableLifecycle);

        return bridge;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cfg"></param>
    protected virtual string GetFullStateName(IGrainActivationContext context, IStorageInterceptorConfig cfg) => $"{RuntimeTypeNameFormatter.Format(context.GrainType)}.{cfg.StateName}";
    internal static TypeFormattingOptions LogFormat { get; } = new TypeFormattingOptions(includeGlobal: false);

    private static void ThrowMissingProviderException(IGrainActivationContext context, IStorageInterceptorConfig cfg)
    {
        string errMsg;
        var grainTypeName = BuildParseableName(context.GrainType);
        if (string.IsNullOrEmpty(cfg.StorageName))
        {
            errMsg = $"No default storage provider found loading grain type {grainTypeName}.";
        }
        else
        {
            errMsg = $"No storage provider named \"{cfg.StorageName}\" found loading grain type {grainTypeName}.";
        }

        throw new BadGrainStorageConfigException(errMsg);
    }

    private static string BuildParseableName(Type type)
    {
        var builder = new StringBuilder();
        GetParseableName(
            type,
            builder,
            new Queue<Type>(
                type.IsGenericTypeDefinition
                    ? type.GetGenericArguments()
                    : type.GenericTypeArguments),
            LogFormat,
            t => GetUnadornedTypeName(t) + LogFormat.NameSuffix);
        return builder.ToString();
    }

    /// <summary>
    /// Returns the non-generic type name without any special characters.
    /// </summary>
    /// <param name="type">
    /// The type.
    /// </param>
    /// <returns>
    /// The non-generic type name without any special characters.
    /// </returns>
    public static string GetUnadornedTypeName(Type type)
    {
        var index = type.Name.IndexOf('`');

        // An ampersand can appear as a suffix to a by-ref type.
        return (index > 0 ? type.Name[..index] : type.Name).TrimEnd('&');
    }
    internal static TypeFormattingOptions Default { get; } = new TypeFormattingOptions();
    private static readonly ConcurrentDictionary<Tuple<Type, TypeFormattingOptions>, string> ParseableNameCache = new();
    private readonly IServiceProvider services;

    internal static string GetParseableName(Type type, TypeFormattingOptions? options = null, Func<Type, string>? getNameFunc = null)
    {
        options ??= Default;

        // If a naming function has been specified, skip the cache.
        if (getNameFunc != null)
        {
            return BuildParseableName();
        }

        return ParseableNameCache.GetOrAdd(Tuple.Create(type, options), _ => BuildParseableName());

        string BuildParseableName()
        {
            var builder = new StringBuilder();
            GetParseableName(
                type,
                builder,
                new Queue<Type>(
                    type.IsGenericTypeDefinition
                        ? type.GetGenericArguments()
                        : type.GenericTypeArguments),
                options,
                t => GetUnadornedTypeName(t) + options.NameSuffix);
            return builder.ToString();
        }
    }
    /// <summary>Returns a string representation of <paramref name="type"/>.</summary>
    /// <param name="type">The type.</param>
    /// <param name="builder">The <see cref="StringBuilder"/> to append results to.</param>
    /// <param name="typeArguments">The type arguments of <paramref name="type"/>.</param>
    /// <param name="options">The type formatting options.</param>
    /// <param name="getNameFunc">Delegate that returns name for a type.</param>
    private static void GetParseableName(
        Type type,
        StringBuilder builder,
        Queue<Type> typeArguments,
        TypeFormattingOptions options,
        Func<Type, string> getNameFunc)
    {
        if (type.IsArray)
        {
            var elementType = GetParseableName(type.GetElementType()!, options);
            if (!string.IsNullOrWhiteSpace(elementType))
            {
                _ = builder.Append(elementType).Append('[').Append(new string(',', type.GetArrayRank() - 1)).Append(']');
            }

            return;
        }

        if (type.IsGenericParameter)
        {
            if (options.IncludeGenericTypeParameters)
            {
                builder.Append(GetUnadornedTypeName(type));
            }

            return;
        }

        if (type.DeclaringType != null)
        {
            // This is not the root type.
            GetParseableName(type.DeclaringType, builder, typeArguments, options, t => GetUnadornedTypeName(t));
            builder.Append(options.NestedTypeSeparator);
        }
        else if (!string.IsNullOrWhiteSpace(type.Namespace) && options.IncludeNamespace)
        {
            // This is the root type, so include the namespace.
            var namespaceName = type.Namespace;
            if (options.NestedTypeSeparator != '.')
            {
                namespaceName = namespaceName.Replace('.', options.NestedTypeSeparator);
            }

            if (options.IncludeGlobal)
            {
                builder.Append("global::");
            }

            builder.Append(namespaceName).Append(options.NestedTypeSeparator);
        }

        if (type.IsConstructedGenericType)
        {
            // Get the unadorned name, the generic parameters, and add them together.
            var unadornedTypeName = getNameFunc(type);
            builder.Append(EscapeIdentifier(unadornedTypeName));
            var generics =
                Enumerable.Range(0, Math.Min(type.GetGenericArguments().Length, typeArguments.Count))
                    .Select(_ => typeArguments.Dequeue())
                    .ToList();
            if (generics.Count > 0 && options.IncludeTypeParameters)
            {
                var genericParameters = string.Join(
                    ",",
                    generics.Select(generic => GetParseableName(generic, options)));
                builder.Append('<').Append(genericParameters).Append('>');
            }
        }
        else if (type.IsGenericTypeDefinition)
        {
            // Get the unadorned name, the generic parameters, and add them together.
            var unadornedTypeName = getNameFunc(type);
            builder.Append(EscapeIdentifier(unadornedTypeName));
            var generics =
                Enumerable.Range(0, Math.Min(type.GetGenericArguments().Length, typeArguments.Count))
                    .Select(_ => typeArguments.Dequeue())
                    .ToList();
            if (generics.Count > 0 && options.IncludeTypeParameters)
            {
                var genericParameters = string.Join(
                    ",",
                    generics.Select(_ => options.IncludeGenericTypeParameters ? _.ToString() : string.Empty));
                builder.Append('<').Append(genericParameters).Append('>');
            }
        }
        else
        {
            builder.Append(EscapeIdentifier(getNameFunc(type)));
        }
    }
    private static string EscapeIdentifier(string identifier)
    {
        if (IsCSharpKeyword(identifier))
        {
            return "@" + identifier;
        }

        return identifier;
    }
    internal static bool IsCSharpKeyword(string identifier) => identifier switch
    {
        "abstract" or "add" or "alias" or "as" or "ascending" or "async" or "await" or "base" or "bool" or "break" or "byte" or "case" or "catch" or "char" or "checked" or "class" or "const" or "continue" or "decimal" or "default" or "delegate" or "descending" or "do" or "double" or "dynamic" or "else" or "enum" or "event" or "explicit" or "extern" or "false" or "finally" or "fixed" or "float" or "for" or "foreach" or "from" or "get" or "global" or "goto" or "group" or "if" or "implicit" or "in" or "int" or "interface" or "internal" or "into" or "is" or "join" or "let" or "lock" or "long" or "nameof" or "namespace" or "new" or "null" or "object" or "operator" or "orderby" or "out" or "override" or "params" or "partial" or "private" or "protected" or "public" or "readonly" or "ref" or "remove" or "return" or "sbyte" or "sealed" or "select" or "set" or "short" or "sizeof" or "stackalloc" or "static" or "string" or "struct" or "switch" or "this" or "throw" or "true" or "try" or "typeof" or "uint" or "ulong" or "unchecked" or "unsafe" or "ushort" or "using" or "value" or "var" or "virtual" or "void" or "volatile" or "when" or "where" or "while" or "yield" => true,
        _ => false,
    };
    /// <inheritdoc/>
    public IPersistentState<TState> Create<TState>(IGrainActivationContext context, IStorageInterceptorFullConfig<TState> config) => throw new NotImplementedException();

    private class PersistentStateBridge<TState> : IPersistentState<TState>, ILifecycleParticipant<IGrainLifecycle>
    {
        private readonly string fullStateName;
        private readonly IGrainActivationContext context;
        private readonly IGrainStorage storageProvider;
        private readonly StorageInterceptorOptions<TState> options;
        private IStorage<TState> storage = default!;

        public PersistentStateBridge(string fullStateName, IGrainActivationContext context, IGrainStorage storageProvider, StorageInterceptorOptions<TState> options)
        {
            ArgumentNullException.ThrowIfNull(fullStateName);
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(storageProvider);
            ArgumentNullException.ThrowIfNull(options);

            this.fullStateName = fullStateName;
            this.context = context;
            this.storageProvider = storageProvider;
            this.options = options;
        }

        public TState State
        {
            get => this.storage.State;
            set => this.storage.State = value;
        }

        public string Etag => this.storage.Etag;

        public bool RecordExists => this.storage.RecordExists;

        /// <inheritdoc/>
        public async Task ClearStateAsync()
        {
            if (!await this.options.OnBeforeClearStateAsync(this.context, this))
            {
                await this.storage.ClearStateAsync();
                await this.options.OnAfterClearStateAsync(this.context, this);
            }
        }

        /// <inheritdoc/>
        public async Task WriteStateAsync()
        {
            if (!await this.options.OnBeforeWriteStateFunc(this.context, this))
            {
                await this.storage.WriteStateAsync();
                await this.options.OnAfterWriteStateFunc(this.context, this);
            }
        }

        /// <inheritdoc/>
        public async Task ReadStateAsync()
        {
            if (!await this.options.OnBeforeReadStateAsync(this.context, this))
            {
                await this.storage.ReadStateAsync();
                await this.options.OnAfterReadStateFunc.Invoke(this.context, this);
            }
        }

        public void Participate(IGrainLifecycle lifecycle) => lifecycle.Subscribe(this.GetType().FullName, GrainLifecycleStage.SetupState, this.OnSetupState);

        private Task OnSetupState(CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            this.storage = new StateStorageBridge<TState>(this.fullStateName, this.context.GrainInstance.GrainReference, this.storageProvider, this.context.ActivationServices.GetService<ILoggerFactory>());
            return this.ReadStateAsync();
        }
    }
}

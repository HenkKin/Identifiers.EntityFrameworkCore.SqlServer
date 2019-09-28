using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identifiers.EntityFrameworkCore.SqlServer
{
    /// <summary>
    /// Thanks to https://andrewlock.net/strongly-typed-ids-in-ef-core-using-strongly-typed-entity-ids-to-avoid-primitive-obsession-part-4/
    /// </summary>
    /// <typeparam name="TDatabaseClrType"></typeparam>
    public class IdentifierValueConverterSelector<TDatabaseClrType> : ValueConverterSelector where TDatabaseClrType : IConvertible
    {
        // The dictionary in the base type is private, so we need our own one here.
        private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
            = new ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo>();

        public IdentifierValueConverterSelector(ValueConverterSelectorDependencies dependencies) : base(dependencies)
        { }

        public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type providerClrType = null)
        {
            var baseConverters = base.Select(modelClrType, providerClrType);
            foreach (var converter in baseConverters)
            {
                yield return converter;
            }

            // Extract the "real" type T from Nullable<T> if required
            var underlyingModelType = UnwrapNullableType(modelClrType);
            var underlyingProviderType = UnwrapNullableType(providerClrType);

            // 'null' means 'get any value converters for the modelClrType'
            if (underlyingModelType == typeof(Identifier) && (underlyingProviderType is null || underlyingProviderType == typeof(TDatabaseClrType)))
            {
                // Try and get a nested class with the expected name. 
                //var converterType = typeof(IdentifierValueConverter);

                yield return _converters.GetOrAdd(
                    (underlyingModelType, typeof(TDatabaseClrType)),
                    k =>
                    {
                        // Create an instance of the converter whenever it's requested.
                        Func<ValueConverterInfo, ValueConverter> factory =
                            info => new IdentifierValueConverter<TDatabaseClrType>(info.MappingHints);

                        // (ValueConverter) Activator.CreateInstance(converterType, info.MappingHints);

                        // Build the info for our strongly-typed ID => TType converter
                        return new ValueConverterInfo(modelClrType, typeof(TDatabaseClrType), factory);
                    }
                );
            }
        }

        private static Type UnwrapNullableType(Type type)
        {
            if (type is null) { return null; }

            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}

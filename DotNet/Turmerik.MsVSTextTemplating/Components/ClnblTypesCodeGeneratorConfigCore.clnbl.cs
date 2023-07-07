using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.MsVSTextTemplating.Components
{
    public static partial class ClnblTypesCodeGeneratorConfigCore
    {
        public interface IClnbl
        {
            string HelperClassSuffix { get; }
            string GetterMethodsPrefix { get; }

            IEnumerable<string> GetIncludedNamespaceStatements();
            IEnumerable<string> GetAddedNamespaceStatements();
            IEnumerable<string> GetRemovedNamespaceStatements();

            TypeNames.IClnbl GetTypeNames();
            HelperMethodNames.IClnbl GetHelperMethodNames();
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                HelperClassSuffix = src.HelperClassSuffix;
                GetterMethodsPrefix = src.GetterMethodsPrefix;

                IncludedNamespaceStatements = src.GetIncludedNamespaceStatements()?.RdnlC();
                AddedNamespaceStatements = src.GetAddedNamespaceStatements()?.RdnlC();
                RemovedNamespaceStatements = src.GetRemovedNamespaceStatements()?.RdnlC();

                TypeNames = src.GetTypeNames()?.ToImmtbl();
                HelperMethodNames = src.GetHelperMethodNames()?.ToImmtbl();
            }

            public string HelperClassSuffix { get; }
            public string GetterMethodsPrefix { get; }

            public ReadOnlyCollection<string> IncludedNamespaceStatements { get; }
            public ReadOnlyCollection<string> AddedNamespaceStatements { get; }
            public ReadOnlyCollection<string> RemovedNamespaceStatements { get; }

            public TypeNames.Immtbl TypeNames { get; }
            public HelperMethodNames.Immtbl HelperMethodNames { get; }

            public IEnumerable<string> GetIncludedNamespaceStatements() => IncludedNamespaceStatements;
            public IEnumerable<string> GetAddedNamespaceStatements() => AddedNamespaceStatements;
            public IEnumerable<string> GetRemovedNamespaceStatements() => RemovedNamespaceStatements;

            public TypeNames.IClnbl GetTypeNames() => TypeNames;
            public HelperMethodNames.IClnbl GetHelperMethodNames() => HelperMethodNames;
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                HelperClassSuffix = src.HelperClassSuffix;
                GetterMethodsPrefix = src.GetterMethodsPrefix;

                IncludedNamespaceStatements = src.GetIncludedNamespaceStatements()?.RdnlC();
                AddedNamespaceStatements = src.GetAddedNamespaceStatements()?.RdnlC();
                RemovedNamespaceStatements = src.GetRemovedNamespaceStatements()?.RdnlC();

                TypeNames = src.GetTypeNames()?.ToMtbl();
                HelperMethodNames = src.GetHelperMethodNames()?.ToMtbl();
            }

            public string HelperClassSuffix { get; set; }
            public string GetterMethodsPrefix { get; set; }

            public ReadOnlyCollection<string> IncludedNamespaceStatements { get; set; }
            public ReadOnlyCollection<string> AddedNamespaceStatements { get; set; }
            public ReadOnlyCollection<string> RemovedNamespaceStatements { get; set; }

            public TypeNames.Mtbl TypeNames { get; set; }
            public HelperMethodNames.Mtbl HelperMethodNames { get; set; }

            public IEnumerable<string> GetIncludedNamespaceStatements() => IncludedNamespaceStatements;
            public IEnumerable<string> GetAddedNamespaceStatements() => AddedNamespaceStatements;
            public IEnumerable<string> GetRemovedNamespaceStatements() => RemovedNamespaceStatements;

            public TypeNames.IClnbl GetTypeNames() => TypeNames;
            public HelperMethodNames.IClnbl GetHelperMethodNames() => HelperMethodNames;
        }

        public static Immtbl ToImmtbl(
            this IClnbl src) => new Immtbl(src);

        public static Immtbl AsImmtbl(
            this IClnbl src) => (src as Immtbl) ?? src?.ToImmtbl();

        public static Mtbl ToMtbl(
            this IClnbl src) => new Mtbl(src);

        public static Mtbl AsMtbl(
            this IClnbl src) => (src as Mtbl) ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl> ToImmtblCllctn(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl> AsImmtblCllctn(
            this IEnumerable<IClnbl> src) => (
            src as ReadOnlyCollection<Immtbl>) ?? src?.ToImmtblCllctn();

        public static List<Mtbl> ToMtblList(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl> AsMtblList(
            this IEnumerable<IClnbl> src) => (src as List<Mtbl>) ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl> AsImmtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => (src as ReadOnlyDictionary<TKey, Immtbl>) ?? (src as Dictionary<TKey, Mtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl> AsMtblDictnr<TKey>(
            IDictionaryCore<TKey, IClnbl> src) => (src as Dictionary<TKey, Mtbl>) ?? (src as ReadOnlyDictionary<TKey, Immtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());
    }
}

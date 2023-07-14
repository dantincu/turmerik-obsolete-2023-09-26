using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
    public static class ClnblTypesCodeParserOutput
    {
        public interface IClnbl
        {
            SyntaxTree SyntaxTree { get; }
            CompilationUnitSyntax Root { get; }
            string Namespace { get; }
            bool NamespaceIsFileScoped { get; }

            IEnumerable<string> GetUsingNamespaceStatements();
            IEnumerable<string> GetUsedNamespaces();
            IEnumerable<string> GetStaticallyUsedNamespaces();

            IDictionaryCore<string, string> GetNamespaceAliases();

            IEnumerable<ParserOutputClassDefinition.IClnbl> GetClassDefinitions();
            IEnumerable<ParserOutputInterfaceDefinition.IClnbl> GetInterfaceDefinitions();
        }

        public class Immtbl : IClnbl
        {
            public Immtbl()
            {
            }

            public Immtbl(IClnbl src)
            {
                SyntaxTree = src.SyntaxTree;
                Root = src.Root;
                Namespace = src.Namespace;
                NamespaceIsFileScoped = src.NamespaceIsFileScoped;

                UsingNamespaceStatements = src.GetUsingNamespaceStatements()?.RdnlC();
                UsedNamespaces = src.GetUsedNamespaces()?.RdnlC();
                StaticallyUsedNamespaces = src.GetStaticallyUsedNamespaces()?.RdnlC();
                NamespaceAliases = src.GetNamespaceAliases().AsRdnlDictnr();
                ClassDefinitions = src.GetClassDefinitions()?.AsImmtblCllctn();
                InterfaceDefinitions = src.GetInterfaceDefinitions()?.AsImmtblCllctn();
            }

            public SyntaxTree SyntaxTree { get; }
            public CompilationUnitSyntax Root { get; }
            public string Namespace { get; }
            public bool NamespaceIsFileScoped { get; }

            public ReadOnlyCollection<string> UsingNamespaceStatements { get; }
            public ReadOnlyCollection<string> UsedNamespaces { get; }
            public ReadOnlyCollection<string> StaticallyUsedNamespaces { get; }
            public ReadOnlyDictionary<string, string> NamespaceAliases { get; }
            public ReadOnlyCollection<ParserOutputClassDefinition.Immtbl> ClassDefinitions { get; }
            public ReadOnlyCollection<ParserOutputInterfaceDefinition.Immtbl> InterfaceDefinitions { get; }

            public IEnumerable<string> GetUsingNamespaceStatements() => UsingNamespaceStatements;
            public IEnumerable<string> GetUsedNamespaces() => UsedNamespaces;
            public IEnumerable<string> GetStaticallyUsedNamespaces() => StaticallyUsedNamespaces;
            public IDictionaryCore<string, string> GetNamespaceAliases() => NamespaceAliases.AsDictnrCore();
            public IEnumerable<ParserOutputClassDefinition.IClnbl> GetClassDefinitions() => ClassDefinitions;
            public IEnumerable<ParserOutputInterfaceDefinition.IClnbl> GetInterfaceDefinitions() => InterfaceDefinitions;
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                SyntaxTree = src.SyntaxTree;
                Root = src.Root;
                Namespace = src.Namespace;
                NamespaceIsFileScoped = src.NamespaceIsFileScoped;

                UsingNamespaceStatements = src.GetUsingNamespaceStatements()?.ToList();
                UsedNamespaces = src.GetUsedNamespaces()?.ToList();
                StaticallyUsedNamespaces = src.GetStaticallyUsedNamespaces()?.ToList();
                NamespaceAliases = src.GetNamespaceAliases().AsDictnr();
                ClassDefinitions = src.GetClassDefinitions()?.AsMtblList();
                InterfaceDefinitions = src.GetInterfaceDefinitions()?.AsMtblList();
            }

            public SyntaxTree SyntaxTree { get; set; }
            public CompilationUnitSyntax Root { get; set; }
            public string Namespace { get; set; }
            public bool NamespaceIsFileScoped { get; set; }

            public List<string> UsingNamespaceStatements { get; set; }
            public List<string> UsedNamespaces { get; set; }
            public List<string> StaticallyUsedNamespaces { get; set; }
            public Dictionary<string, string> NamespaceAliases { get; set; }
            public List<ParserOutputClassDefinition.Mtbl> ClassDefinitions { get; set; }
            public List<ParserOutputInterfaceDefinition.Mtbl> InterfaceDefinitions { get; set; }

            public IEnumerable<string> GetUsingNamespaceStatements() => UsingNamespaceStatements;
            public IEnumerable<string> GetUsedNamespaces() => UsedNamespaces;
            public IEnumerable<string> GetStaticallyUsedNamespaces() => StaticallyUsedNamespaces;
            public IDictionaryCore<string, string> GetNamespaceAliases() => NamespaceAliases as IDictionaryCore<string, string>;
            public IEnumerable<ParserOutputClassDefinition.IClnbl> GetClassDefinitions() => ClassDefinitions;
            public IEnumerable<ParserOutputInterfaceDefinition.IClnbl> GetInterfaceDefinitions() => InterfaceDefinitions;
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

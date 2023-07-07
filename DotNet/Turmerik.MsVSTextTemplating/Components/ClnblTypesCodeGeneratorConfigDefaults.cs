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
    public static class ClnblTypesCodeGeneratorConfigDefaults
    {
        /// <summary>
        /// The name of <c>typeof</c> operator.
        /// </summary>
        public const string TYPEOF = "typeof";

        /// <summary>
        /// The name of <c>public</c> keyword.
        /// </summary>
        public const string PUBLIC = "public";

        /// <summary>
        /// The name of <c>static</c> keyword.
        /// </summary>
        public const string STATIC = "static";

        /// <summary>
        /// The name of <c>partial</c> keyword.
        /// </summary>
        public const string PARTIAL = "partial";

        /// <summary>
        /// The name of <c>class</c> keyword.
        /// </summary>
        public const string CLASS = "class";

        /// <summary>
        /// The name of <c>interface</c> keyword.
        /// </summary>
        public const string INTERFACE = "interface";

        /// <summary>
        /// Helper class suffix.
        /// </summary>
        public const string HX_SFFX = "_HX";

        /// <summary>
        /// Getter methods prefix.
        /// </summary>
        public const string GET = "Get";

        /// <summary>
        /// Name of cloneable interface types.
        /// </summary>
        public const string ICLNBL = "IClnbl";

        /// <summary>
        /// Name of immutable types.
        /// </summary>
        public const string IMMTBL = "Immtbl";

        /// <summary>
        /// Name of mutable types.
        /// </summary>
        public const string MTBL = "Mtbl";

        public static string EnumerableIntfTypeName = nameof(IEnumerable<int>);
        public static string DictionaryCoreIntfTypeName = nameof(IDictionaryCore<int, int>);

        public static string ListTypeName = nameof(List<int>);
        public static string DictionaryTypeName = nameof(Dictionary<int, int>);
        
        public static string ReadOnlyCollectionTypeName = nameof(ReadOnlyCollection<int>);
        public static string ReadOnlyDictionaryTypeName = nameof(ReadOnlyDictionary<int, int>);

        public static string ClnblNsTypeAttrTypeName = nameof(ClnblNsTypeAttribute);

        public static readonly ReadOnlyCollection<string> IncludedNamespaces = new string[]
        {
            "using System;",
            "using System.Collections;",
            "using System.Collections.Generic;",
            "using System.Collections.ObjectModel;",
            "using System.Linq;",
            "using System.Text;",
            "using System.Threading.Tasks;",
            "using Turmerik.Cloneable;",
            "using Turmerik.Collections;"
        }.RdnlC();

        /// <summary>
        /// Name of the linq <c>ToList</c> extension helper method which converts an enumerable to a list.
        /// </summary>
        public static readonly string ToList = nameof(ToList);

        /// <summary>
        /// Name of the linq <c>ToDictionary</c> extension helper method which converts an enumerable to regular dictionary.
        /// </summary>
        public static readonly string ToDictionary = nameof(ToDictionary);

        /// <summary>
        /// Name of the <c>RdnlC</c> extension helper method which converts an enumerable to a read-only collection.
        /// </summary>
        public static readonly string RdnlC = nameof(RdnlC);

        /// <summary>
        /// Name of the <c>RdnlD</c> extension helper method which converts an regular dictionary to a read-only dictionary.
        /// </summary>
        public static readonly string RdnlD = nameof(RdnlD);

        /// <summary>
        /// Name of <c>ToImmtbl</c> method that takes as single argument a cloneable object and creates and returns
        /// an immutable object using the provided cloneable object as source.
        /// </summary>
        public static readonly string ToImmtbl = nameof(ToImmtbl);

        /// <summary>
        /// Name of <c>AsImmtbl</c> method that takes as single argument a cloneable object and,
        /// if the provided object already is of the immutable type, it casts it to the immutable type and returns it;
        /// or, if it is rather a mutable type, it converts it to the immutable type using the <c>ToImmtbl</c> method and returns the result.
        /// </summary>
        public static readonly string AsImmtbl = nameof(AsImmtbl);

        /// <summary>
        /// Name of <c>ToMtbl</c> method that takes as single argument a cloneable object and creates and returns
        /// a mutable object using the provided cloneable object as source.
        /// </summary>
        public static readonly string ToMtbl = nameof(ToMtbl);

        /// <summary>
        /// Name of <c>AsMtbl</c> method that takes as single argument a cloneable object and,
        /// if the provided object already is of the mutable type, it casts it to the mutable type and returns it;
        /// or, if it is rather an immutable type, it converts it to the mutable type using the <c>ToMtbl</c> method and returns the result.
        /// </summary>
        public static readonly string AsMtbl = nameof(AsMtbl);

        /// <summary>
        /// Name of <c>ToImmtblCllctn</c> method that takes as single argument an enumerable of the cloneable type
        /// and creates and returns a read-only collection of the immutable type using the provided enumerable as source.
        /// </summary>
        public static readonly string ToImmtblCllctn = nameof(ToImmtblCllctn);

        /// <summary>
        /// Name of <c>AsImmtblCllctn</c> method that takes as single argument an enumerable of the cloneable type and,
        /// if the provided enumerable already is a read-only collection of the immutable type,
        /// it casts it to the read-only collection of the immutable type and returns it;
        /// or, if it is rather a list of the mutable type, it converts it to the read-only collection of the immutable type
        /// using the <c>ToImmtblCllctn</c> method and returns the result.
        /// </summary>
        public static readonly string AsImmtblCllctn = nameof(AsImmtblCllctn);

        /// <summary>
        /// Name of <c>ToMtblList</c> method that takes as single argument an enumerable of the cloneable type
        /// and creates and returns a list of the mutable type using the provided enumerable as source.
        /// </summary>
        public static readonly string ToMtblList = nameof(ToMtblList);

        /// <summary>
        /// Name of <c>AsMtblList</c> method that takes as single argument an enumerable of the cloneable type and,
        /// if the provided enumerable already is a list of the mutable type,
        /// it casts it to the list of the mutable type and returns it;
        /// or, if it is rather a read-only collection of the immutable type,
        /// it converts it to the list of the mutable type
        /// using the <c>ToMtblList</c> method and returns the result.
        /// </summary>
        public static readonly string AsMtblList = nameof(AsMtblList);

        /// <summary>
        /// Name of <c>AsImmtblDictnr</c> method that takes as single argument an enumerable of key-value-pairs of the cloneable type and,
        /// if the provided enumerable already is a read-only dictionary of the immutable type,
        /// it casts it to the read-only dictionary of the immutable type and returns it;
        /// or, if it is rather a regular dictionary of mutable type, it converts it to the read-only dictionary of the immutable type
        /// using the provided enumerable as source and returns the result.
        /// </summary>
        public static readonly string AsImmtblDictnr = nameof(AsImmtblDictnr);

        /// <summary>
        /// Name of <c>AsMtblDictnr</c> method that takes as single argument an enumerable of key-value-pairs of the cloneable type and,
        /// if the provided enumerable already is a regular dictionary of the mutable type,
        /// it casts it to the regular dictionary of the mutable type and returns it;
        /// or, if it is rather a read-only dictionary of immutable type, it converts it to the regular dictionary of the mutable type
        /// using the provided enumerable as source and returns the result.
        /// </summary>
        public static readonly string AsMtblDictnr = nameof(AsMtblDictnr);
    }
}

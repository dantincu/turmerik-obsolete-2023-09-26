using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.Cloneable
{
    /// <summary>
    /// This class should NOT be instantiated. It only contains nested types, despite not having made it static.
    /// I did not make it static so that it can be inherited for easier referencing the nested generic classes.
    /// </summary>
    /// <typeparam name="TClnbl">The cloneable type.</typeparam>
    /// <typeparam name="TImmtbl">The immutable type.</typeparam>
    /// <typeparam name="TMtbl">The mutable type</typeparam>
    public class ClnblCore
    {
        public interface IClnbl
        {
            IClnbl ToImmtbl();
            IClnbl ToMtbl();
            IClnbl AsImmtbl();
            IClnbl AsMtbl();
        }

        public interface IClnblCollection<TClnbl> : IEnumerable<TClnbl>
            where TClnbl : IClnbl
        {
            IEnumerable<TClnbl> AsNmrbl();
        }

        public interface IClnblDictnr<TKey, TClnbl> : IEnumerable<KeyValuePair<TKey, TClnbl>>
            where TClnbl : IClnbl
        {
            IEnumerable<KeyValuePair<TKey, TClnbl>> AsNmrbl();
        }
    }
}
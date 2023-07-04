using System;
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
        public interface IClnblCore
        {
            IClnblCore ToImmtbl();
            IClnblCore ToMtbl();
            IClnblCore AsImmtbl();
            IClnblCore AsMtbl();
        }

        public interface IClnblCollection<TClnbl> : IEnumerable<TClnbl>
            where TClnbl : IClnblCore
        {
            IEnumerable<TClnbl> AsNmrbl();
        }

        public interface IClnblDictnr<TKey, TClnbl> : IEnumerable<KeyValuePair<TKey, TClnbl>>
            where TClnbl : IClnblCore
        {
            IEnumerable<KeyValuePair<TKey, TClnbl>> AsNmrbl();
        }
    }

    /// <summary>
    /// This class should NOT be instantiated. It only contains nested types, despite not having made it static.
    /// I did not make it static so that it can be inherited for easier referencing the nested generic classes.
    /// </summary>
    /// <typeparam name="TClnbl">The cloneable type.</typeparam>
    /// <typeparam name="TImmtbl">The immutable type.</typeparam>
    /// <typeparam name="TMtbl">The mutable type</typeparam>
    public class ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : ClnblCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : TClnbl, ClnblCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TMtbl : TClnbl, ClnblCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
    {
        public interface IClnblCore : ClnblCore.IClnblCore
        {
            new TImmtbl ToImmtbl();
            new TMtbl ToMtbl();
            new TImmtbl AsImmtbl();
            new TMtbl AsMtbl();
        }

        public interface IClnblCollection : ClnblCore.IClnblCollection<TClnbl>
        {
            ImmtblCollection ToImmtblCllctn();
            MtblList ToMtblList();
            ImmtblCollection AsImmtblCllctn();
            MtblList AsMtblList();
        }

        public interface IClnblDictnr<TKey> : ClnblCore.IClnblDictnr<TKey, TClnbl>
        {
            ImmtblDictionary<TKey> ToImmtblDictnr();
            MtblDictionary<TKey> ToMtblDictnr();
            ImmtblDictionary<TKey> AsImmtblDictnr();
            MtblDictionary<TKey> AsMtblDictnr();
        }

        public abstract class ImmtblCoreBase : IClnblCore
        {
            protected ImmtblCoreBase(TClnbl src)
            {
            }

            public virtual TImmtbl ToImmtbl() => this.CreateInstance<TImmtbl>();
            public virtual TMtbl ToMtbl() => this.CreateInstance<TMtbl>();

            public TImmtbl AsImmtbl() => (TImmtbl)(IClnblCore)this;
            public TMtbl AsMtbl() => ToMtbl();

            ClnblCore.IClnblCore ClnblCore.IClnblCore.ToImmtbl() => ToImmtbl();
            ClnblCore.IClnblCore ClnblCore.IClnblCore.ToMtbl() => ToMtbl();
            ClnblCore.IClnblCore ClnblCore.IClnblCore.AsImmtbl() => AsImmtbl();
            ClnblCore.IClnblCore ClnblCore.IClnblCore.AsMtbl() => AsMtbl();
        }

        public abstract class MtblCoreBase : IClnblCore
        {
            protected MtblCoreBase(TClnbl src)
            {
            }

            protected MtblCoreBase()
            {
            }

            public virtual TImmtbl ToImmtbl() => this.CreateInstance<TImmtbl>();
            public virtual TMtbl ToMtbl() => this.CreateInstance<TMtbl>();

            public TImmtbl AsImmtbl() => ToImmtbl();
            public TMtbl AsMtbl() => (TMtbl)(IClnblCore)this;

            ClnblCore.IClnblCore ClnblCore.IClnblCore.ToImmtbl() => ToImmtbl();
            ClnblCore.IClnblCore ClnblCore.IClnblCore.ToMtbl() => ToMtbl();
            ClnblCore.IClnblCore ClnblCore.IClnblCore.AsImmtbl() => AsImmtbl();
            ClnblCore.IClnblCore ClnblCore.IClnblCore.AsMtbl() => AsMtbl();
        }

        public class ImmtblCollection : ReadOnlyCollection<TImmtbl>, IClnblCollection
        {
            public ImmtblCollection(IList<TImmtbl> list) : base(list)
            {
            }

            public ImmtblCollection ToImmtblCllctn() => new ImmtblCollection(
                ((ReadOnlyCollection<TImmtbl>)this).Select(
                        src => src.IfNotNull(obj => obj.ToImmtbl())).ToArray());

            public MtblList ToMtblList() => new MtblList(
                ((ReadOnlyCollection<TImmtbl>)this).Select(
                        src => src.IfNotNull(obj => obj.ToMtbl())));

            public ImmtblCollection AsImmtblCllctn() => this;
            public MtblList AsMtblList() => ToMtblList();

            IEnumerator<TClnbl> IEnumerable<TClnbl>.GetEnumerator() => new TransformedEnumerator<TImmtbl, TClnbl>(
                GetEnumerator(), src => src);

            public IEnumerable<TClnbl> AsNmrbl() => this;

            public ReadOnlyCollection<TImmtbl> AsRdnlCllctn() => this;
        }

        public class MtblList : List<TMtbl>, IClnblCollection
        {
            public MtblList()
            {
            }

            public MtblList(IEnumerable<TMtbl> collection) : base(collection)
            {
            }

            public MtblList(int capacity) : base(capacity)
            {
            }

            public ImmtblCollection ToImmtblCllctn() => new ImmtblCollection(
                ((List<TMtbl>)this).Select(
                        src => src.IfNotNull(obj => obj.ToImmtbl())).ToArray());

            public MtblList ToMtblList() => new MtblList(
                ((List<TMtbl>)this).Select(
                        src => src.IfNotNull(obj => obj.ToMtbl())));

            public ImmtblCollection AsImmtblCllctn() => ToImmtblCllctn();
            public MtblList AsMtblList() => this;

            IEnumerator<TClnbl> IEnumerable<TClnbl>.GetEnumerator() => new TransformedEnumerator<TMtbl, TClnbl>(
                GetEnumerator(), src => src);

            public IEnumerable<TClnbl> AsNmrbl() => this;
            public List<TMtbl> AsList() => this;
        }

        public class ImmtblDictionary<TKey> : ReadOnlyDictionary<TKey, TImmtbl>, IClnblDictnr<TKey>
        {
            public ImmtblDictionary(IDictionary<TKey, TImmtbl> dictionary) : base(dictionary)
            {
            }

            public ImmtblDictionary<TKey> ToImmtblDictnr() => new ImmtblDictionary<TKey>(
                ((ReadOnlyDictionary<TKey, TImmtbl>)this).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.IfNotNull(obj => obj.ToImmtbl())));

            public MtblDictionary<TKey> ToMtblDictnr() => new MtblDictionary<TKey>(
                ((ReadOnlyDictionary<TKey, TImmtbl>)this).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.IfNotNull(obj => obj.ToMtbl())));

            public ImmtblDictionary<TKey> AsImmtblDictnr() => this;
            public MtblDictionary<TKey> AsMtblDictnr() => ToMtblDictnr();

            IEnumerator<KeyValuePair<TKey, TClnbl>> IEnumerable<KeyValuePair<TKey, TClnbl>>.GetEnumerator(
                ) => new TransformedEnumerator<KeyValuePair<TKey, TImmtbl>, KeyValuePair<TKey, TClnbl>>(
                GetEnumerator(), src => new KeyValuePair<TKey, TClnbl>(
                    src.Key, src.Value));

            public IEnumerable<KeyValuePair<TKey, TClnbl>> AsNmrbl() => this;
            public ReadOnlyDictionary<TKey, TImmtbl> AsRdnlDictnr() => this;
        }

        public class MtblDictionary<TKey> : Dictionary<TKey, TMtbl>, IClnblDictnr<TKey>
        {
            public MtblDictionary()
            {
            }

            public MtblDictionary(IDictionary<TKey, TMtbl> dictionary) : base(dictionary)
            {
            }

            public MtblDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
            {
            }

            public MtblDictionary(int capacity) : base(capacity)
            {
            }

            public MtblDictionary(IDictionary<TKey, TMtbl> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer)
            {
            }

            public MtblDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
            {
            }

            protected MtblDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

            IEnumerator<KeyValuePair<TKey, TClnbl>> IEnumerable<KeyValuePair<TKey, TClnbl>>.GetEnumerator(
                ) => new TransformedEnumerator<KeyValuePair<TKey, TMtbl>, KeyValuePair<TKey, TClnbl>>(
                GetEnumerator(), src => new KeyValuePair<TKey, TClnbl>(
                    src.Key, src.Value));

            public ImmtblDictionary<TKey> ToImmtblDictnr() => new ImmtblDictionary<TKey>(
                ((Dictionary<TKey, TMtbl>)this).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.IfNotNull(obj => obj.ToImmtbl())));

            public ImmtblDictionary<TKey> AsImmtblDictnr() => ToImmtblDictnr();
            public MtblDictionary<TKey> AsMtblDictnr() => this;

            public MtblDictionary<TKey> ToMtblDictnr() => new MtblDictionary<TKey>(
                ((Dictionary<TKey, TMtbl>)this).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.IfNotNull(obj => obj.ToMtbl())));

            public IEnumerable<KeyValuePair<TKey, TClnbl>> AsNmrbl() => this;
            public Dictionary<TKey, TMtbl> AsDictnr() => this;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ClnblIgnoreMethodAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ClnblIgnorePropertyAttribute : Attribute
    {
    }
}
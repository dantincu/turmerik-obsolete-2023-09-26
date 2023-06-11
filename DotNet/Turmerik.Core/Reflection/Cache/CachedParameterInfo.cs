using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Collections;
using Turmerik.Synchronized;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedParameterInfo : ICachedItem<ParameterInfo, CachedParameterFlags.IClnbl>
    {
        string Name { get; }
        int Position { get; }
        Lazy<ICachedTypeInfo> Type { get; }
        Lazy<ReadOnlyCollection<Attribute>> CustomAttributes { get; }
    }

    public class CachedParameterInfo : CachedItemBase<ParameterInfo, CachedParameterFlags.IClnbl>, ICachedParameterInfo
    {
        public CachedParameterInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            ParameterInfo value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
            Name = value.Name;
            Position = value.Position;

            Type = new Lazy<ICachedTypeInfo>(
                () => TypesMap.Value.Get(
                    Data.ParameterType));

            CustomAttributes = new Lazy<ReadOnlyCollection<Attribute>>(
                () => Data.GetCustomAttributes().RdnlC());
        }

        public string Name { get; }
        public int Position { get; }
        public Lazy<ICachedTypeInfo> Type { get; }

        public Lazy<ReadOnlyCollection<Attribute>> CustomAttributes { get; }

        protected override CachedParameterFlags.IClnbl GetFlags() => CachedParameterFlags.Create(this);
    }
}

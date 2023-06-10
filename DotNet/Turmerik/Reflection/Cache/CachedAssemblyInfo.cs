using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedAssemblyInfo : ICachedItem<Assembly>
    {
        Lazy<ReadOnlyCollection<ICachedTypeInfo>> AllTypes { get; }
        IStaticDataCache<string, ICachedTypeInfo> AssemblyTypesMap { get; }
    }

    public class CachedAssemblyInfo : CachedItemBase<Assembly>, ICachedAssemblyInfo
    {
        public CachedAssemblyInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory,
            Assembly value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                nonSynchronizedStaticDataCacheFactory,
                value)
        {
            AllTypes = new Lazy<ReadOnlyCollection<ICachedTypeInfo>>(
                () => Data.GetTypes().Select(
                    type => TypesMap.Value.Get(
                        type)).RdnlC());

            AssemblyTypesMap = nonSynchronizedStaticDataCacheFactory.Create<string, ICachedTypeInfo>(
                typeName => Data.GetType(typeName)?.WithValue(
                    type => TypesMap.Value.Get(
                        type)));
        }

        public Lazy<ReadOnlyCollection<ICachedTypeInfo>> AllTypes { get; }
        public IStaticDataCache<string, ICachedTypeInfo> AssemblyTypesMap { get; }
    }
}

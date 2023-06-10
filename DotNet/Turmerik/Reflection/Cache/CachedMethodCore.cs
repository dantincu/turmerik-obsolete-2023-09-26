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
    public interface ICachedMethodCore<TMethodBase, TFlags> : ICachedMemberInfo<TMethodBase, TFlags>
        where TMethodBase : MethodBase
    {
        Lazy<ReadOnlyCollection<ICachedParameterInfo>> Parameters { get; }
    }

    public abstract class CachedMethodBase<TMethodBase, TFlags> : CachedMemberInfoBase<TMethodBase, TFlags>, ICachedMethodCore<TMethodBase, TFlags>
        where TMethodBase : MethodBase
        where TFlags : ICachedMemberFlagsCore
    {
        public CachedMethodBase(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory,
            TMethodBase value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                nonSynchronizedStaticDataCacheFactory,
                value)
        {
            Parameters = new Lazy<ReadOnlyCollection<ICachedParameterInfo>>(
                () => Data.GetParameters().Select(
                    ItemsFactory.ParameterInfo).RdnlC());
        }

        public Lazy<ReadOnlyCollection<ICachedParameterInfo>> Parameters { get; }
    }
}

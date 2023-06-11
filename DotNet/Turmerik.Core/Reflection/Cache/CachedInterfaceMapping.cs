using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Collections;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedInterfaceMapping : ICachedItem<InterfaceMapping>
    {
        Lazy<ReadOnlyCollection<ICachedMethodInfo>> InterfaceMethods { get; }
        ICachedTypeInfo InterfaceType { get; }
        Lazy<ReadOnlyCollection<ICachedMethodInfo>> TargetMethods { get; }
        ICachedTypeInfo TargetType { get; }
    }

    public class CachedInterfaceMapping : CachedItemBase<InterfaceMapping>, ICachedInterfaceMapping
    {
        public CachedInterfaceMapping(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory,
            ICachedTypeInfo type,
            ICachedTypeInfo interfaceType) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                nonSynchronizedStaticDataCacheFactory,
                type.Data.GetInterfaceMap(
                    interfaceType.Data))
        {
            InterfaceMethods = new Lazy<ReadOnlyCollection<ICachedMethodInfo>>(
                () => Data.InterfaceMethods.Select(
                    method => ItemsFactory.MethodInfo(method)).RdnlC());

            InterfaceType = interfaceType;

            TargetMethods = new Lazy<ReadOnlyCollection<ICachedMethodInfo>>(
                () => Data.TargetMethods.Select(
                    method => ItemsFactory.MethodInfo(method)).RdnlC());

            TargetType = type;
        }

        public Lazy<ReadOnlyCollection<ICachedMethodInfo>> InterfaceMethods { get; }
        public ICachedTypeInfo InterfaceType { get; }
        public Lazy<ReadOnlyCollection<ICachedMethodInfo>> TargetMethods { get; }
        public ICachedTypeInfo TargetType { get; }
    }
}

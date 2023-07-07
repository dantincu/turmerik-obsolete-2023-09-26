using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Turmerik.Cache;
using Turmerik.Collections;
using Turmerik.Reflection;
using Turmerik.Reflection.Cache;

namespace Turmerik.Mapping
{
    public readonly struct MappingPropsPair
    {
        public MappingPropsPair(
            ICachedPropertyInfo srcProp,
            ICachedPropertyInfo destnProp)
        {
            SrcProp = srcProp ?? throw new ArgumentNullException(nameof(srcProp));
            DestnProp = destnProp ?? throw new ArgumentNullException(nameof(destnProp));
        }

        public ICachedPropertyInfo SrcProp { get; }
        public ICachedPropertyInfo DestnProp { get; }
    }

    public interface ITypesMappingCache
    {
        IStaticDataCache<Type, IStaticDataCache<Type, ReadOnlyCollection<MappingPropsPair>>> PropsCache { get; }
    }

    public class TypesMappingCache : ITypesMappingCache
    {
        public TypesMappingCache(
            ICachedTypesMap cachedTypesMap,
            IThreadSafeStaticDataCacheFactory cacheFactory)
        {
            CachedTypesMap = cachedTypesMap ?? throw new ArgumentNullException(nameof(cachedTypesMap));
            CacheFactory = cacheFactory ?? throw new ArgumentNullException(nameof(cacheFactory));

            SrcPropsFilter = GetSrcPropsFilter();
            DestnPropsFilter = GetDestnPropsFilter();

            PropsCache = GetPropsCache();
        }

        public IStaticDataCache<Type, IStaticDataCache<Type, ReadOnlyCollection<MappingPropsPair>>> PropsCache { get; }

        private ICachedTypesMap CachedTypesMap { get; }
        private IThreadSafeStaticDataCacheFactory CacheFactory { get; }
        private PropertyAccessibilityFilter SrcPropsFilter { get; }
        private PropertyAccessibilityFilter DestnPropsFilter { get; }

        private IStaticDataCache<Type, IStaticDataCache<Type, ReadOnlyCollection<MappingPropsPair>>> GetPropsCache(
            ) => CacheFactory.Create(
                (Type srcType) => CacheFactory.Create(
                (Type destnType) =>
                {
                    var cachedSrcType = CachedTypesMap.Get(srcType);
                    var cachedDestnType = CachedTypesMap.Get(destnType);

                    var destnPropsCllctn = cachedSrcType.InstanceProps.Value.ExtAsmVisible.Value.Filtered.Get(
                        DestnPropsFilter);

                    var srcPropsCllctn = cachedSrcType.InstanceProps.Value.ExtAsmVisible.Value.Filtered.Get(
                        SrcPropsFilter);

                    var propsList = new List<MappingPropsPair>();

                    foreach (var destnProp in destnPropsCllctn)
                    {
                        string propName = destnProp.Name;

                        var srcMatchProp = srcPropsCllctn.SingleOrDefault(
                            srcProp => srcProp.Name == propName && destnProp.DeclaringType.Value.Data.IsAssignableFrom(
                                srcProp.DeclaringType.Value.Data));

                        if (srcMatchProp != null)
                        {
                            propsList.Add(
                                new MappingPropsPair(
                                    srcMatchProp,
                                    destnProp));
                        }
                    }

                    return propsList.RdnlC();
                }));

        private PropertyAccessibilityFilter GetSrcPropsFilter() => new PropertyAccessibilityFilter(
            MemberScope.Instance,
            true, null,
            MemberVisibility.Public);

        private PropertyAccessibilityFilter GetDestnPropsFilter() => new PropertyAccessibilityFilter(
            MemberScope.Instance,
            true, true,
            MemberVisibility.Public,
            MemberVisibility.Public);
    }
}

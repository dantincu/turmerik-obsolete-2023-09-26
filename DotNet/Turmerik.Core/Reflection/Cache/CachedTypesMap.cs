using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedTypesMap : IStaticDataCache<Type, ICachedTypeInfo>
    {

    }

    public class CachedTypesMap : StaticDataCache<Type, ICachedTypeInfo>, ICachedTypesMap
    {
        public CachedTypesMap(
            IDataCache<Type, ICachedTypeInfo> innerCache,
            ICachedReflectionItemsFactory cachedTypeInfoFactory) : base(
                innerCache,
                cachedTypeInfoFactory.TypeInfo)
        {
        }
    }
}

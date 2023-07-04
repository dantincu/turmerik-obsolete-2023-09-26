using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Turmerik.Cache;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedInheritedConstructorsCollection : ICachedInheritedItemsCollection<ConstructorInfo, ICachedConstructorInfo, MemberVisibility, ICachedConstructorsCollection>
    {
    }

    public class CachedInheritedConstructorsCollection : CachedInheritedItemsCollectionBase<ConstructorInfo, ICachedConstructorInfo, MemberVisibility, ICachedConstructorsCollection>, ICachedInheritedConstructorsCollection
    {
        public CachedInheritedConstructorsCollection(
            ICachedTypesMap typesMap,
            ICachedReflectionItemsFactory itemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            ICachedTypeInfo type,
            Func<MemberVisibility, MemberVisibility> ownFilterReducer,
            Func<MemberVisibility, MemberVisibility> allVisibleFilterReducer,
            Func<MemberVisibility, MemberVisibility> asmVisibleFilterReducer) : base(
                typesMap,
                itemsFactory,
                staticDataCacheFactory,
                type,
                (arg, filter) => arg.Matches(filter),
                ownFilterReducer,
                allVisibleFilterReducer,
                asmVisibleFilterReducer)
        {
        }

        protected override ICachedConstructorsCollection CreateCollection(
            ReadOnlyCollection<ICachedConstructorInfo> items,
            Func<MemberVisibility, MemberVisibility> filterReducer) => ItemsFactory.Constructors(
                items,
                FilterMatchPredicate,
                filterReducer);

        protected override ICachedConstructorsCollection GetBaseTypeAllVisibleItems(
            ICachedTypeInfo baseType) => baseType.Constructors.Value.AsmVisible.Value;

        protected override ICachedConstructorsCollection GetBaseTypeAsmVisibleItems(
            ICachedTypeInfo baseType) => baseType.Constructors.Value.AllVisible.Value;

        protected override ICachedConstructorsCollection GetBaseTypeOwnItems(
            ICachedTypeInfo baseType) => baseType.Constructors.Value.Own.Value;

        protected override ICachedConstructorInfo[] GetOwnItems(
            ICachedTypeInfo type) => type.Data.GetConstructors(
                ReflC.Filter.BindingFlag.DeclaredOnly).Select(
                constructor => ItemsFactory.ConstructorInfo(constructor)).ToArray();
    }
}

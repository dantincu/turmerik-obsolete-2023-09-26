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
    public interface ICachedTypeInfo : ICachedMemberInfo<Type, ICachedTypeFlags>
    {
        string? FullName { get; }
        string? FullDisplayName { get; }
        Lazy<ICachedTypeInfo?> BaseType { get; }
        Lazy<ReadOnlyCollection<ICachedTypeInfo>> Interfaces { get; }
        Lazy<IStaticDataCache<Type, ICachedInterfaceMapping>> InterfacesMap { get; }
        Lazy<ICachedInheritedFieldsCollection> Fields { get; }
        Lazy<ICachedInheritedMethodsCollection> Methods { get; }
        Lazy<ICachedInheritedPropertiesCollection> InstanceProps { get; }
        Lazy<ICachedInheritedPropertiesCollection> StaticProps { get; }
        Lazy<ICachedInheritedEventsCollection> Events { get; }
        Lazy<ICachedAssemblyInfo> Assembly { get; }
    }

    public class CachedTypeInfo : CachedMemberInfoBase<Type, ICachedTypeFlags>, ICachedTypeInfo
    {
        public CachedTypeInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            INonSynchronizedStaticDataCacheFactory nonSynchronizedStaticDataCacheFactory,
            Type value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                nonSynchronizedStaticDataCacheFactory,
                value)
        {
            FullName = value.FullName;
            FullDisplayName = ReflH.GetTypeFullDisplayName(FullName);

            BaseType = new Lazy<ICachedTypeInfo?>(
                () => Data.BaseType?.WithValue(
                    TypesMap.Value.Get));

            Interfaces = new Lazy<ReadOnlyCollection<ICachedTypeInfo>>(
                () => Data.GetInterfaces().Select(TypesMap.Value.Get).RdnlC());

            InterfacesMap = new Lazy<IStaticDataCache<Type, ICachedInterfaceMapping>>(
                () => StaticDataCacheFactory.Create<Type, ICachedInterfaceMapping>(
                    type => ItemsFactory.InterfaceMapping(
                        this,
                        TypesMap.Value.Get(type))));

            Fields = new Lazy<ICachedInheritedFieldsCollection>(
                () => ItemsFactory.InheritedFields(this));

            Methods = new Lazy<ICachedInheritedMethodsCollection>(
                () => ItemsFactory.InheritedMethods(this));

            InstanceProps = new Lazy<ICachedInheritedPropertiesCollection>(
                () => ItemsFactory.InheritedProperties(this, true));

            StaticProps = new Lazy<ICachedInheritedPropertiesCollection>(
                () => ItemsFactory.InheritedProperties(this, false));

            Events = new Lazy<ICachedInheritedEventsCollection>(
                () => ItemsFactory.InheritedEvents(this));

            Assembly = new Lazy<ICachedAssemblyInfo>(
                () => ItemsFactory.AssemblyInfo(Data.Assembly));
        }

        public string? FullName { get; }
        public string? FullDisplayName { get; }
        public Lazy<ICachedTypeInfo?> BaseType { get; }
        public Lazy<ReadOnlyCollection<ICachedTypeInfo>> Interfaces { get; }
        public Lazy<IStaticDataCache<Type, ICachedInterfaceMapping>> InterfacesMap { get; }

        public Lazy<ICachedInheritedFieldsCollection> Fields { get; }
        public Lazy<ICachedInheritedMethodsCollection> Methods { get; }
        public Lazy<ICachedInheritedPropertiesCollection> InstanceProps { get; }
        public Lazy<ICachedInheritedPropertiesCollection> StaticProps { get; }
        public Lazy<ICachedInheritedEventsCollection> Events { get; }
        public Lazy<ICachedAssemblyInfo> Assembly { get; }

        protected override ICachedTypeFlags GetFlags() => CachedTypeFlags.Create(this);
    }
}

using Newtonsoft.Json.Linq;
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
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedTypeInfo : ICachedMemberInfo<Type, CachedTypeFlags.IClnbl>
    {
        string FullName { get; }
        string FullDisplayName { get; }
        bool IsInterface { get; }
        bool IsAbstract { get; }
        bool IsSealed { get; }
        bool IsStaticClass { get; }
        Lazy<ICachedTypeInfo> BaseType { get; }
        Lazy<ReadOnlyCollection<ICachedTypeInfo>> Interfaces { get; }
        Lazy<IStaticDataCache<Type, ICachedInterfaceMapping>> InterfacesMap { get; }
        Lazy<ICachedInheritedFieldsCollection> StaticFields { get; }
        Lazy<ICachedInheritedFieldsCollection> InstanceFields { get; }
        Lazy<ICachedInheritedMethodsCollection> InstanceMethods { get; }
        Lazy<ICachedInheritedMethodsCollection> StaticMethods { get; }
        Lazy<ICachedInheritedPropertiesCollection> InstanceProps { get; }
        Lazy<ICachedInheritedPropertiesCollection> StaticProps { get; }
        Lazy<ICachedInheritedEventsCollection> Events { get; }
        Lazy<ICachedInheritedConstructorsCollection> Constructors { get; }
        Lazy<ICachedAssemblyInfo> Assembly { get; }
    }

    public class CachedTypeInfo : CachedMemberInfoBase<Type, CachedTypeFlags.IClnbl>, ICachedTypeInfo
    {
        public CachedTypeInfo(
            Lazy<ICachedTypesMap> cachedTypesMap,
            ICachedReflectionItemsFactory cachedReflectionItemsFactory,
            IStaticDataCacheFactory staticDataCacheFactory,
            Type value) : base(
                cachedTypesMap,
                cachedReflectionItemsFactory,
                staticDataCacheFactory,
                value)
        {
            FullName = value.FullName;
            FullDisplayName = ReflH.GetTypeFullDisplayName(FullName);
            IsInterface = value.IsInterface;
            IsAbstract = value.IsAbstract;
            IsSealed = value.IsSealed;
            IsStaticClass = IsAbstract && IsSealed;

            BaseType = new Lazy<ICachedTypeInfo>(
                () => IsInterface ? null : IsStaticClass ? null : Data.BaseType?.WithValue(
                    TypesMap.Value.Get));

            Interfaces = new Lazy<ReadOnlyCollection<ICachedTypeInfo>>(
                () => IsStaticClass ? null : Data.GetInterfaces().Select(TypesMap.Value.Get).RdnlC());

            InterfacesMap = new Lazy<IStaticDataCache<Type, ICachedInterfaceMapping>>(
                () => IsStaticClass ? null : StaticDataCacheFactory.Create<Type, ICachedInterfaceMapping>(
                    type => ItemsFactory.InterfaceMapping(
                        this,
                        TypesMap.Value.Get(type))));

            InstanceFields = new Lazy<ICachedInheritedFieldsCollection>(
                () => IsInterface ? null : IsStaticClass ? null : ItemsFactory.InheritedFields(this, true));

            StaticFields = new Lazy<ICachedInheritedFieldsCollection>(
                () => IsInterface ? null : ItemsFactory.InheritedFields(this, false, IsStaticClass));

            InstanceMethods = new Lazy<ICachedInheritedMethodsCollection>(
                () => IsStaticClass ? null : ItemsFactory.InheritedMethods(this, true, IsInterface));

            StaticMethods = new Lazy<ICachedInheritedMethodsCollection>(
                () => IsInterface ? null : ItemsFactory.InheritedMethods(this, false, false, IsStaticClass));

            InstanceProps = new Lazy<ICachedInheritedPropertiesCollection>(
                () => IsStaticClass ? null : ItemsFactory.InheritedProperties(this, true, IsInterface));

            StaticProps = new Lazy<ICachedInheritedPropertiesCollection>(
                () => IsInterface ? null : ItemsFactory.InheritedProperties(this, false, false, IsStaticClass));

            Constructors = new Lazy<ICachedInheritedConstructorsCollection>(
                () => IsInterface ? null : IsStaticClass ? null : ItemsFactory.InheritedConstructors(this));

            Events = new Lazy<ICachedInheritedEventsCollection>(
                () => IsStaticClass ? null : ItemsFactory.InheritedEvents(this, IsInterface));

            Assembly = new Lazy<ICachedAssemblyInfo>(
                () => ItemsFactory.AssemblyInfo(Data.Assembly));
        }

        public string FullName { get; }
        public string FullDisplayName { get; }
        public bool IsInterface { get; }
        public bool IsAbstract { get; }
        public bool IsSealed { get; }
        public bool IsStaticClass { get; }
        public Lazy<ICachedTypeInfo> BaseType { get; }
        public Lazy<ReadOnlyCollection<ICachedTypeInfo>> Interfaces { get; }
        public Lazy<IStaticDataCache<Type, ICachedInterfaceMapping>> InterfacesMap { get; }

        public Lazy<ICachedInheritedFieldsCollection> InstanceFields { get; }
        public Lazy<ICachedInheritedFieldsCollection> StaticFields { get; }
        public Lazy<ICachedInheritedMethodsCollection> InstanceMethods { get; }
        public Lazy<ICachedInheritedMethodsCollection> StaticMethods { get; }
        public Lazy<ICachedInheritedPropertiesCollection> InstanceProps { get; }
        public Lazy<ICachedInheritedPropertiesCollection> StaticProps { get; }
        public Lazy<ICachedInheritedConstructorsCollection> Constructors { get; }
        public Lazy<ICachedInheritedEventsCollection> Events { get; }
        public Lazy<ICachedAssemblyInfo> Assembly { get; }

        protected override CachedTypeFlags.IClnbl GetFlags() => CachedTypeFlags.Create(this);
    }
}

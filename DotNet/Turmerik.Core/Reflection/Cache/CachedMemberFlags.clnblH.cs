using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public static partial class CachedMemberFlagsCore
    {
        public static CachedMemberFlagsCoreImmtbl Create<TMethodBase, TFlags>(
            ICachedMethodCore<TMethodBase, TFlags> cached)
            where TMethodBase : MethodBase => cached.Data.WithValue(
                data => new CachedMemberFlagsCoreMtbl
                {
                    IsPublic = data.IsPublic,
                    IsAssembly = data.IsAssembly,
                    IsFamily = data.IsFamily,
                    IsFamilyAndAssembly = data.IsFamilyAndAssembly,
                    IsPrivate = data.IsPrivate,
                    IsFamilyOrAssembly = data.IsFamilyOrAssembly,
                }).ToImmtbl();
    }

    public static partial class CachedMemberFlags
    {
        public static CachedMemberFlagsImmtbl Create(
            ICachedMethodInfo cached) => cached.Data.WithValue(
                data => new CachedMemberFlagsMtbl
                {
                    IsPublic = data.IsPublic,
                    IsAssembly = data.IsAssembly,
                    IsFamily = data.IsFamily,
                    IsFamilyAndAssembly = data.IsFamilyAndAssembly,
                    IsPrivate = data.IsPrivate,
                    IsFamilyOrAssembly = data.IsFamilyOrAssembly,
                    IsStatic = data.IsStatic,
                }.ToImmtbl());
    }

    public static partial class CachedTypeFlags
    {
        public static CachedTypeFlagsImmtbl Create(
            ICachedTypeInfo cached) => cached.Data.WithValue(
                data => new CachedTypeFlagsMtbl
                {
                    IsPublic = data.IsPublic,
                    IsInternal = data.IsVisible,
                    IsNested = data.IsNested,
                    IsNestedFamily = data.IsNestedFamily,
                    IsNestedFamORAssem = data.IsNestedFamORAssem,
                    IsNestedFamANDAssem = data.IsNestedFamANDAssem,
                    IsNestedPrivate = data.IsNestedPrivate,
                    IsAbstract = data.IsAbstract,
                    IsSealed = data.IsSealed,
                    IsStatic = data.IsStaticClass(),
                    IsGenericType = data.IsGenericType,
                    IsGenericTypeDefinition = data.IsGenericTypeDefinition,
                    IsArray = data.IsArray
                }.ToImmtbl());
    }

    public static partial class CachedFieldFlags
    {
        public static CachedFieldFlagsImmtbl Create(
            ICachedFieldInfo cached) => cached.Data.WithValue(
                data => new CachedFieldFlagsMtbl
                {
                    IsPublic = data.IsPublic,
                    IsAssembly = data.IsAssembly,
                    IsFamily = data.IsFamily,
                    IsFamilyAndAssembly = data.IsFamilyAndAssembly,
                    IsPrivate = data.IsPrivate,
                    IsFamilyOrAssembly = data.IsFamilyOrAssembly,
                    IsStatic = data.IsStatic,
                    IsEditable = data.IsEditable(),
                    IsInitOnly = data.IsInitOnly,
                    IsLiteral = data.IsLiteral,
                }.ToImmtbl());
    }

    public static partial class CachedPropertyFlags
    {
        public static CachedPropertyFlagsImmtbl Create(
            ICachedPropertyInfo cached) => new CachedPropertyFlagsMtbl
            {
                IsStatic = new Lazy<bool>(
                    () => (cached.Getter.Value ?? cached.Setter.Value).Flags.Value.IsStatic),
                CanRead = cached.CanRead,
                CanWrite = cached.CanWrite,
                Getter = new Lazy<ICachedMemberFlagsCore>(
                    () => cached.Getter.Value?.WithValue(
                    gttr => CachedMemberFlagsCore.Create(gttr))),
                Setter = new Lazy<ICachedMemberFlagsCore>(
                    () => cached.Setter.Value?.WithValue(
                    sttr => CachedMemberFlagsCore.Create(sttr)))
            }.ToImmtbl();
    }

    public static partial class CachedEventFlags
    {
        public static CachedEventFlagsImmtbl Create(
            ICachedEventInfo cached) => new CachedEventFlagsMtbl
            {
                IsMulticast = cached.Data.IsMulticast,
                AddMethod = new Lazy<ICachedMemberFlagsCore>(
                () => cached.Adder.Value?.WithValue(
                adder => CachedMemberFlagsCore.Create(adder))),
                RemoveMethod = new Lazy<ICachedMemberFlagsCore>(
                () => cached.Remover.Value?.WithValue(
                adder => CachedMemberFlagsCore.Create(adder))),
                InvokeMethod = new Lazy<ICachedMemberFlagsCore>(
                () => cached.Invoker.Value?.WithValue(
                adder => CachedMemberFlagsCore.Create(adder)))
            }.ToImmtbl();
    }

    public static partial class CachedParameterFlags
    {
        public static CachedParameterFlagsImmtbl Create(
            ICachedParameterInfo cached) => cached.Data.WithValue(
                data => new CachedParameterFlagsMtbl
                {
                    IsIn = data.IsIn,
                    IsLcid = data.IsLcid,
                    IsOptional = data.IsOptional,
                    IsOut = data.IsOut,
                    IsRetval = data.IsRetval,
                }.ToImmtbl());
    }
}

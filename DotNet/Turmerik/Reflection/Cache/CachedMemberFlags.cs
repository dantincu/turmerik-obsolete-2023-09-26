using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Turmerik.Reflection.Cache
{
    public interface ICachedMemberFlagsCore
    {
        bool IsPublic { get; }
        bool IsFamily { get; }
        bool IsAssembly { get; }
        bool IsFamilyOrAssembly { get; }
        bool IsFamilyAndAssembly { get; }
        bool IsPrivate { get; }
    }

    public interface ICachedMemberFlags : ICachedMemberFlagsCore
    {
        bool IsStatic { get; }
    }

    public interface ICachedTypeFlags
    {
        bool IsPublic { get; }
        bool IsInternal { get; }
        bool IsNested { get; }
        bool IsNestedFamily { get; }
        bool IsNestedFamORAssem { get; }
        bool IsNestedFamANDAssem { get; }
        bool IsNestedPrivate { get; }
        bool IsAbstract { get; }
        bool IsSealed { get; }
        bool IsStatic { get; }
        bool IsGenericType { get; }
        bool IsGenericTypeDefinition { get; }
        bool IsArray { get; }
    }

    public interface ICachedFieldFlags : ICachedMemberFlags
    {
        bool IsEditable { get; }
        bool IsInitOnly { get; }
        bool IsLiteral { get; }
    }

    public interface ICachedPropertyFlags
    {
        Lazy<bool> IsStatic { get; }
        bool CanRead { get; }
        bool CanWrite { get; }
        Lazy<ICachedMemberFlagsCore> Getter { get; }
        Lazy<ICachedMemberFlagsCore> Setter { get; }
    }

    public interface ICachedEventFlags
    {
        bool IsMulticast { get; }
        Lazy<ICachedMemberFlagsCore> AddMethod { get; }
        Lazy<ICachedMemberFlagsCore> RemoveMethod { get; }
        Lazy<ICachedMemberFlagsCore> InvokeMethod { get; }
    }

    public interface ICachedParameterFlags
    {
        bool IsIn { get; }
        bool IsLcid { get; }
        bool IsOptional { get; }
        bool IsOut { get; }
        bool IsRetval { get; }
    }

    public class CachedMemberFlagsCore : ICachedMemberFlagsCore
    {
        public bool IsPublic { get; init; }
        public bool IsFamily { get; init; }
        public bool IsAssembly { get; init; }
        public bool IsFamilyOrAssembly { get; init; }
        public bool IsFamilyAndAssembly { get; init; }
        public bool IsPrivate { get; init; }

        public static CachedMemberFlagsCore Create<TMethodBase, TFlags>(
            ICachedMethodCore<TMethodBase, TFlags> cached)
            where TMethodBase : MethodBase => cached.Data.WithValue(
                data => new CachedMemberFlagsCore
            {
                IsPublic = data.IsPublic,
                IsAssembly = data.IsAssembly,
                IsFamily = data.IsFamily,
                IsFamilyAndAssembly = data.IsFamilyAndAssembly,
                IsPrivate = data.IsPrivate,
                IsFamilyOrAssembly = data.IsFamilyOrAssembly,
            });
    }

    public class CachedMemberFlags : CachedMemberFlagsCore, ICachedMemberFlags
    {
        public bool IsStatic { get; init; }

        public static CachedMemberFlags Create(
            ICachedMethodInfo cached) => cached.Data.WithValue(
                data => new CachedMemberFlags
            {
                IsPublic = data.IsPublic,
                IsAssembly = data.IsAssembly,
                IsFamily = data.IsFamily,
                IsFamilyAndAssembly = data.IsFamilyAndAssembly,
                IsPrivate = data.IsPrivate,
                IsFamilyOrAssembly = data.IsFamilyOrAssembly,
                IsStatic = data.IsStatic,
            });
    }

    public class CachedTypeFlags : ICachedTypeFlags
    {
        public bool IsPublic { get; init; }
        public bool IsInternal { get; init; }
        public bool IsNested { get; init; }
        public bool IsNestedFamily { get; init; }
        public bool IsNestedFamORAssem { get; init; }
        public bool IsNestedFamANDAssem { get; init; }
        public bool IsNestedPrivate { get; init; }
        public bool IsAbstract { get; init; }
        public bool IsSealed { get; init; }
        public bool IsStatic { get; init; }
        public bool IsGenericType { get; init; }
        public bool IsGenericTypeDefinition { get; init; }
        public bool IsArray { get; init; }

        public static CachedTypeFlags Create(
            ICachedTypeInfo cached) => cached.Data.WithValue(
                data => new CachedTypeFlags
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
                });
    }

    public class CachedFieldFlags : CachedMemberFlags, ICachedFieldFlags
    {
        public bool IsEditable { get; init; }
        public bool IsInitOnly { get; init; }
        public bool IsLiteral { get; init; }

        public static CachedFieldFlags Create(
            ICachedFieldInfo cached) => cached.Data.WithValue(
                data => new CachedFieldFlags
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
            });
    }

    public class CachedPropertyFlags : ICachedPropertyFlags
    {
        public Lazy<bool> IsStatic { get; init; }
        public bool CanRead { get; init; }
        public bool CanWrite { get; init; }
        public Lazy<ICachedMemberFlagsCore> Getter { get; init; }
        public Lazy<ICachedMemberFlagsCore> Setter { get; init; }

        public static CachedPropertyFlags Create(
            ICachedPropertyInfo cached) => new CachedPropertyFlags
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
            };
    }

    public class CachedEventFlags : ICachedEventFlags
    {
        public bool IsMulticast { get; init; }
        public Lazy<ICachedMemberFlagsCore> AddMethod { get; init; }
        public Lazy<ICachedMemberFlagsCore> RemoveMethod { get; init; }
        public Lazy<ICachedMemberFlagsCore> InvokeMethod { get; init; }

        public static CachedEventFlags Create(
            ICachedEventInfo cached) => new CachedEventFlags
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
            };
    }

    public class CachedParameterFlags : ICachedParameterFlags
    {
        public bool IsIn { get; init; }
        public bool IsLcid { get; init; }
        public bool IsOptional { get; init; }
        public bool IsOut { get; init; }
        public bool IsRetval { get; init; }

        public static CachedParameterFlags Create(
            ICachedParameterInfo cached) => cached.Data.WithValue(
                data => new CachedParameterFlags
            {
                IsIn = data.IsIn,
                IsLcid = data.IsLcid,
                IsOptional = data.IsOptional,
                IsOut = data.IsOut,
                IsRetval = data.IsRetval,
            });
    }
}

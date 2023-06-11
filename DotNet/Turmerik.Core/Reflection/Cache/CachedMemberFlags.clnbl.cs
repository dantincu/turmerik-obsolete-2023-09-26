using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Utils;

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
        Lazy<CachedMemberFlagsCore.IClnbl> Getter { get; }
        Lazy<CachedMemberFlagsCore.IClnbl> Setter { get; }
    }

    public interface ICachedEventFlags
    {
        bool IsMulticast { get; }
        Lazy<CachedMemberFlagsCore.IClnbl> AddMethod { get; }
        Lazy<CachedMemberFlagsCore.IClnbl> RemoveMethod { get; }
        Lazy<CachedMemberFlagsCore.IClnbl> InvokeMethod { get; }
    }

    public interface ICachedParameterFlags
    {
        bool IsIn { get; }
        bool IsLcid { get; }
        bool IsOptional { get; }
        bool IsOut { get; }
        bool IsRetval { get; }
    }

    public partial class CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl> : ClnblCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.ICoreClnbl
        where TImmtbl : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.CoreImmtbl, TClnbl
        where TMtbl : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.CoreMtbl, TClnbl
    {
        public interface ICoreClnbl : IClnblCore, ICachedMemberFlagsCore
        {
        }

        public class CoreImmtbl : ImmtblCoreBase, ICoreClnbl
        {
            public CoreImmtbl(TClnbl src) : base(src)
            {
                IsPublic = src.IsPublic;
                IsFamily = src.IsFamily;
                IsAssembly = src.IsAssembly;
                IsFamilyOrAssembly = src.IsFamilyOrAssembly;
                IsFamilyAndAssembly = src.IsFamilyAndAssembly;
                IsPrivate = src.IsPrivate;
            }

            public bool IsPublic { get; }
            public bool IsFamily { get; }
            public bool IsAssembly { get; }
            public bool IsFamilyOrAssembly { get; }
            public bool IsFamilyAndAssembly { get; }
            public bool IsPrivate { get; }
        }

        public class CoreMtbl : MtblCoreBase, ICoreClnbl
        {
            public CoreMtbl()
            {
            }

            public CoreMtbl(TClnbl src) : base(src)
            {
                IsPublic = src.IsPublic;
                IsFamily = src.IsFamily;
                IsAssembly = src.IsAssembly;
                IsFamilyOrAssembly = src.IsFamilyOrAssembly;
                IsFamilyAndAssembly = src.IsFamilyAndAssembly;
                IsPrivate = src.IsPrivate;
            }

            public bool IsPublic { get; set; }
            public bool IsFamily { get; set; }
            public bool IsAssembly { get; set; }
            public bool IsFamilyOrAssembly { get; set; }
            public bool IsFamilyAndAssembly { get; set; }
            public bool IsPrivate { get; set; }
        }
    }

    public partial class CachedMemberFlagsCore : CachedMemberFlagsCore<CachedMemberFlagsCore.IClnbl, CachedMemberFlagsCore.Immtbl, CachedMemberFlagsCore.Mtbl>
    {
        public interface IClnbl : ICoreClnbl
        {

        }

        public class Immtbl : CoreImmtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : CoreMtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
            }
        }

        public static Immtbl Create<TMethodBase, TFlags>(
            ICachedMethodCore<TMethodBase, TFlags> cached)
            where TMethodBase : MethodBase => cached.Data.WithValue(
                data => new Mtbl
                {
                    IsPublic = data.IsPublic,
                    IsAssembly = data.IsAssembly,
                    IsFamily = data.IsFamily,
                    IsFamilyAndAssembly = data.IsFamilyAndAssembly,
                    IsPrivate = data.IsPrivate,
                    IsFamilyOrAssembly = data.IsFamilyOrAssembly,
                }).ToImmtbl();
    }

    public partial class CachedMemberFlags<TClnbl, TImmtbl, TMtbl> : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : CachedMemberFlags<TClnbl, TImmtbl, TMtbl>.ICoreClnbl
        where TImmtbl : CachedMemberFlags<TClnbl, TImmtbl, TMtbl>.CoreImmtbl, TClnbl
        where TMtbl : CachedMemberFlags<TClnbl, TImmtbl, TMtbl>.CoreMtbl, TClnbl
    {
        public new interface ICoreClnbl : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.ICoreClnbl, ICachedMemberFlags
        {
        }

        public new class CoreImmtbl : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.CoreImmtbl, ICoreClnbl
        {
            public CoreImmtbl(TClnbl src) : base(src)
            {
                IsStatic = src.IsStatic;
            }

            public bool IsStatic { get; }
        }

        public new class CoreMtbl : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.CoreMtbl, ICoreClnbl
        {
            public CoreMtbl()
            {
            }

            public CoreMtbl(TClnbl src) : base(src)
            {
                IsStatic = src.IsStatic;
            }

            public bool IsStatic { get; set; }
        }
    }

    public partial class CachedMemberFlags : CachedMemberFlags<CachedMemberFlags.IClnbl, CachedMemberFlags.Immtbl, CachedMemberFlags.Mtbl>
    {
        public interface IClnbl : ICoreClnbl
        {
        }

        public class Immtbl : CoreImmtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : CoreMtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
            }
        }

        public static Immtbl Create(
            ICachedMethodInfo cached) => cached.Data.WithValue(
                data => new Mtbl
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

    public partial class CachedTypeFlags : ClnblCore<CachedTypeFlags.IClnbl, CachedTypeFlags.Immtbl, CachedTypeFlags.Mtbl>
    {
        public interface IClnbl : IClnblCore, ICachedTypeFlags
        {
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                IsPublic = src.IsPublic;
                IsInternal = src.IsInternal;
                IsNested = src.IsNested;
                IsNestedFamily = src.IsNestedFamily;
                IsNestedFamORAssem = src.IsNestedFamORAssem;
                IsNestedFamANDAssem = src.IsNestedFamANDAssem;
                IsNestedPrivate = src.IsNestedPrivate;
                IsAbstract = src.IsAbstract;
                IsSealed = src.IsSealed;
                IsStatic = src.IsStatic;
                IsGenericType = src.IsGenericType;
                IsGenericTypeDefinition = src.IsGenericTypeDefinition;
                IsArray = src.IsArray;
            }

            public bool IsPublic { get; }
            public bool IsInternal { get; }
            public bool IsNested { get; }
            public bool IsNestedFamily { get; }
            public bool IsNestedFamORAssem { get; }
            public bool IsNestedFamANDAssem { get; }
            public bool IsNestedPrivate { get; }
            public bool IsAbstract { get; }
            public bool IsSealed { get; }
            public bool IsStatic { get; }
            public bool IsGenericType { get; }
            public bool IsGenericTypeDefinition { get; }
            public bool IsArray { get; }
        }

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                IsPublic = src.IsPublic;
                IsInternal = src.IsInternal;
                IsNested = src.IsNested;
                IsNestedFamily = src.IsNestedFamily;
                IsNestedFamORAssem = src.IsNestedFamORAssem;
                IsNestedFamANDAssem = src.IsNestedFamANDAssem;
                IsNestedPrivate = src.IsNestedPrivate;
                IsAbstract = src.IsAbstract;
                IsSealed = src.IsSealed;
                IsStatic = src.IsStatic;
                IsGenericType = src.IsGenericType;
                IsGenericTypeDefinition = src.IsGenericTypeDefinition;
                IsArray = src.IsArray;
            }

            public bool IsPublic { get; set; }
            public bool IsInternal { get; set; }
            public bool IsNested { get; set; }
            public bool IsNestedFamily { get; set; }
            public bool IsNestedFamORAssem { get; set; }
            public bool IsNestedFamANDAssem { get; set; }
            public bool IsNestedPrivate { get; set; }
            public bool IsAbstract { get; set; }
            public bool IsSealed { get; set; }
            public bool IsStatic { get; set; }
            public bool IsGenericType { get; set; }
            public bool IsGenericTypeDefinition { get; set; }
            public bool IsArray { get; set; }
        }

        public static Immtbl Create(
            ICachedTypeInfo cached) => cached.Data.WithValue(
                data => new Mtbl
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

    public partial class CachedFieldFlags : CachedMemberFlags<CachedFieldFlags.IClnbl, CachedFieldFlags.Immtbl, CachedFieldFlags.Mtbl>
    {
        public interface IClnbl : ICoreClnbl, ICachedFieldFlags
        {
        }

        public class Immtbl : CoreImmtbl, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                IsEditable = src.IsEditable;
                IsInitOnly = src.IsInitOnly;
                IsLiteral = src.IsLiteral;
            }

            public bool IsEditable { get; }
            public bool IsInitOnly { get; }
            public bool IsLiteral { get; }
        }

        public class Mtbl : CoreMtbl, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                IsEditable = src.IsEditable;
                IsInitOnly = src.IsInitOnly;
                IsLiteral = src.IsLiteral;
            }

            public bool IsEditable { get; set; }
            public bool IsInitOnly { get; set; }
            public bool IsLiteral { get; set; }
        }

        public static Immtbl Create(
            ICachedFieldInfo cached) => cached.Data.WithValue(
                data => new Mtbl
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

    public partial class CachedPropertyFlags : ClnblCore<CachedPropertyFlags.IClnbl, CachedPropertyFlags.Immtbl, CachedPropertyFlags.Mtbl>
    {
        public interface IClnbl : IClnblCore, ICachedPropertyFlags
        {
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                IsStatic = src.IsStatic;
                CanRead = src.CanRead;
                CanWrite = src.CanWrite;
                Getter = src.Getter;
                Setter = src.Setter;
            }

            public Lazy<bool> IsStatic { get; }
            public bool CanRead { get; }
            public bool CanWrite { get; }
            public Lazy<CachedMemberFlagsCore.IClnbl> Getter { get; }
            public Lazy<CachedMemberFlagsCore.IClnbl> Setter { get; }
        }

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                IsStatic = src.IsStatic;
                CanRead = src.CanRead;
                CanWrite = src.CanWrite;
                Getter = src.Getter;
                Setter = src.Setter;
            }

            public Lazy<bool> IsStatic { get; set; }
            public bool CanRead { get; set; }
            public bool CanWrite { get; set; }
            public Lazy<CachedMemberFlagsCore.IClnbl> Getter { get; set; }
            public Lazy<CachedMemberFlagsCore.IClnbl> Setter { get; set; }
        }

        public static Immtbl Create(
            ICachedPropertyInfo cached) => new Mtbl
            {
                IsStatic = new Lazy<bool>(
                    () => (cached.Getter.Value ?? cached.Setter.Value).Flags.Value.IsStatic),
                CanRead = cached.CanRead,
                CanWrite = cached.CanWrite,
                Getter = new Lazy<CachedMemberFlagsCore.IClnbl>(
                    () => cached.Getter.Value?.WithValue(
                    gttr => CachedMemberFlagsCore.Create(gttr))),
                Setter = new Lazy<CachedMemberFlagsCore.IClnbl>(
                    () => cached.Setter.Value?.WithValue(
                    sttr => CachedMemberFlagsCore.Create(sttr)))
            }.ToImmtbl();
    }

    public partial class CachedEventFlags : ClnblCore<CachedEventFlags.IClnbl, CachedEventFlags.Immtbl, CachedEventFlags.Mtbl>
    {
        public interface IClnbl : IClnblCore, ICachedEventFlags
        {
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                IsMulticast = src.IsMulticast;
                AddMethod = src.AddMethod;
                RemoveMethod = src.RemoveMethod;
                InvokeMethod = src.InvokeMethod;
            }

            public bool IsMulticast { get; }
            public Lazy<CachedMemberFlagsCore.IClnbl> AddMethod { get; }
            public Lazy<CachedMemberFlagsCore.IClnbl> RemoveMethod { get; }
            public Lazy<CachedMemberFlagsCore.IClnbl> InvokeMethod { get; }
        }

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                IsMulticast = src.IsMulticast;
                AddMethod = src.AddMethod;
                RemoveMethod = src.RemoveMethod;
                InvokeMethod = src.InvokeMethod;
            }

            public bool IsMulticast { get; set; }
            public Lazy<CachedMemberFlagsCore.IClnbl> AddMethod { get; set; }
            public Lazy<CachedMemberFlagsCore.IClnbl> RemoveMethod { get; set; }
            public Lazy<CachedMemberFlagsCore.IClnbl> InvokeMethod { get; set; }
        }

        public static Immtbl Create(
            ICachedEventInfo cached) => new Mtbl
            {
                IsMulticast = cached.Data.IsMulticast,
                AddMethod = new Lazy<CachedMemberFlagsCore.IClnbl>(
                () => cached.Adder.Value?.WithValue(
                adder => CachedMemberFlagsCore.Create(adder))),
                RemoveMethod = new Lazy<CachedMemberFlagsCore.IClnbl>(
                () => cached.Remover.Value?.WithValue(
                adder => CachedMemberFlagsCore.Create(adder))),
                InvokeMethod = new Lazy<CachedMemberFlagsCore.IClnbl>(
                () => cached.Invoker.Value?.WithValue(
                adder => CachedMemberFlagsCore.Create(adder)))
            }.ToImmtbl();
    }

    public partial class CachedParameterFlags : ClnblCore<CachedParameterFlags.IClnbl, CachedParameterFlags.Immtbl, CachedParameterFlags.Mtbl>
    {
        public interface IClnbl : IClnblCore, ICachedParameterFlags
        {
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                IsIn = src.IsIn;
                IsLcid = src.IsLcid;
                IsOptional = src.IsOptional;
                IsOut = src.IsOut;
                IsRetval = src.IsRetval;
            }

            public bool IsIn { get; }
            public bool IsLcid { get; }
            public bool IsOptional { get; }
            public bool IsOut { get; }
            public bool IsRetval { get; }
        }

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                IsIn = src.IsIn;
                IsLcid = src.IsLcid;
                IsOptional = src.IsOptional;
                IsOut = src.IsOut;
                IsRetval = src.IsRetval;
            }

            public bool IsIn { get; set; }
            public bool IsLcid { get; set; }
            public bool IsOptional { get; set; }
            public bool IsOut { get; set; }
            public bool IsRetval { get; set; }
        }

        public static Immtbl Create(
            ICachedParameterInfo cached) => cached.Data.WithValue(
                data => new Mtbl
                {
                    IsIn = data.IsIn,
                    IsLcid = data.IsLcid,
                    IsOptional = data.IsOptional,
                    IsOut = data.IsOut,
                    IsRetval = data.IsRetval,
                }.ToImmtbl());
    }
}

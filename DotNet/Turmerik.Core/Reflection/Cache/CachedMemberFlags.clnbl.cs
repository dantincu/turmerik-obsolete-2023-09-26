﻿using System;
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
    public class CachedMemberFlagsCoreBase
    {
        public interface IClnblCore
        {
            bool IsPublic { get; }
            bool IsFamily { get; }
            bool IsAssembly { get; }
            bool IsFamilyOrAssembly { get; }
            bool IsFamilyAndAssembly { get; }
            bool IsPrivate { get; }
        }
    }

    public class CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        public interface IClnblCore : CachedMemberFlagsCoreBase.IClnblCore
        {
        }

        public class ImmtblCore : IClnblCore
        {
            public ImmtblCore(TClnbl src)
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

        public class MtblCore : IClnblCore
        {
            public MtblCore()
            {
            }

            public MtblCore(TClnbl src)
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

    public class CachedMemberFlagsCore : CachedMemberFlagsCore<CachedMemberFlagsCore.IClnbl, CachedMemberFlagsCore.Immtbl, CachedMemberFlagsCore.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
        }

        public class Immtbl : ImmtblCore, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : MtblCore, IClnbl
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
                data => new Immtbl(new Mtbl
                {
                    IsPublic = data.IsPublic,
                    IsAssembly = data.IsAssembly,
                    IsFamily = data.IsFamily,
                    IsFamilyAndAssembly = data.IsFamilyAndAssembly,
                    IsPrivate = data.IsPrivate,
                    IsFamilyOrAssembly = data.IsFamilyOrAssembly,
                }));
    }

    public class CachedMemberFlagsBase
    {
        public new interface IClnblCore : CachedMemberFlagsCoreBase.IClnblCore
        {
            bool IsStatic { get; }
        }
    }

    public class CachedMemberFlags<TClnbl, TImmtbl, TMtbl> : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>
        where TClnbl : CachedMemberFlags<TClnbl, TImmtbl, TMtbl>.IClnblCore
        where TImmtbl : CachedMemberFlags<TClnbl, TImmtbl, TMtbl>.ImmtblCore, TClnbl
        where TMtbl : CachedMemberFlags<TClnbl, TImmtbl, TMtbl>.MtblCore, TClnbl
    {
        public new interface IClnblCore : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.IClnblCore, CachedMemberFlagsBase.IClnblCore
        {
        }

        public new class ImmtblCore : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.ImmtblCore, IClnblCore
        {
            public ImmtblCore(TClnbl src) : base(src)
            {
                IsStatic = src.IsStatic;
            }

            public bool IsStatic { get; }
        }

        public new class MtblCore : CachedMemberFlagsCore<TClnbl, TImmtbl, TMtbl>.MtblCore, IClnblCore
        {
            public MtblCore()
            {
            }

            public MtblCore(TClnbl src) : base(src)
            {
                IsStatic = src.IsStatic;
            }

            public bool IsStatic { get; set; }
        }
    }

    public class CachedMemberFlags : CachedMemberFlags<CachedMemberFlags.IClnbl, CachedMemberFlags.Immtbl, CachedMemberFlags.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
        }

        public class Immtbl : ImmtblCore, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
            }
        }

        public class Mtbl : MtblCore, IClnbl
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
                data => new Immtbl(new Mtbl
                {
                    IsPublic = data.IsPublic,
                    IsAssembly = data.IsAssembly,
                    IsFamily = data.IsFamily,
                    IsFamilyAndAssembly = data.IsFamilyAndAssembly,
                    IsPrivate = data.IsPrivate,
                    IsFamilyOrAssembly = data.IsFamilyOrAssembly,
                    IsStatic = data.IsStatic,
                }));
    }

    public class CachedTypeFlags
    {
        public interface IClnbl
        {
            bool IsPublic { get; }
            bool IsInternal { get; }
            bool IsNested { get; }
            bool IsNestedFamily { get; }
            bool IsNestedFamORAssem { get; }
            bool IsNestedFamANDAssem { get; }
            bool IsNestedPrivate { get; }
            bool IsInterface { get; }
            bool IsAbstract { get; }
            bool IsSealed { get; }
            bool IsStaticClass { get; }
            bool IsGenericType { get; }
            bool IsGenericTypeDefinition { get; }
            bool IsArray { get; }
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                IsPublic = src.IsPublic;
                IsInternal = src.IsInternal;
                IsNested = src.IsNested;
                IsNestedFamily = src.IsNestedFamily;
                IsNestedFamORAssem = src.IsNestedFamORAssem;
                IsNestedFamANDAssem = src.IsNestedFamANDAssem;
                IsNestedPrivate = src.IsNestedPrivate;
                IsInterface = src.IsInterface;
                IsAbstract = src.IsAbstract;
                IsSealed = src.IsSealed;
                IsStaticClass = src.IsStaticClass;
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
            public bool IsInterface { get; }
            public bool IsAbstract { get; }
            public bool IsSealed { get; }
            public bool IsStaticClass { get; }
            public bool IsGenericType { get; }
            public bool IsGenericTypeDefinition { get; }
            public bool IsArray { get; }
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                IsPublic = src.IsPublic;
                IsInternal = src.IsInternal;
                IsNested = src.IsNested;
                IsNestedFamily = src.IsNestedFamily;
                IsNestedFamORAssem = src.IsNestedFamORAssem;
                IsNestedFamANDAssem = src.IsNestedFamANDAssem;
                IsNestedPrivate = src.IsNestedPrivate;
                IsInterface = src.IsInterface;
                IsAbstract = src.IsAbstract;
                IsSealed = src.IsSealed;
                IsStaticClass = src.IsStaticClass;
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
            public bool IsInterface { get; set; }
            public bool IsAbstract { get; set; }
            public bool IsSealed { get; set; }
            public bool IsStaticClass { get; set; }
            public bool IsGenericType { get; set; }
            public bool IsGenericTypeDefinition { get; set; }
            public bool IsArray { get; set; }
        }

        public static Immtbl Create(
            ICachedTypeInfo cached) => cached.Data.WithValue(
                data => new Immtbl(new Mtbl
                {
                    IsPublic = data.IsPublic,
                    IsInternal = data.IsVisible,
                    IsNested = data.IsNested,
                    IsNestedFamily = data.IsNestedFamily,
                    IsNestedFamORAssem = data.IsNestedFamORAssem,
                    IsNestedFamANDAssem = data.IsNestedFamANDAssem,
                    IsNestedPrivate = data.IsNestedPrivate,
                    IsInterface = cached.IsInterface,
                    IsAbstract = cached.IsAbstract,
                    IsSealed = cached.IsSealed,
                    IsStaticClass = cached.IsStaticClass,
                    IsGenericType = data.IsGenericType,
                    IsGenericTypeDefinition = data.IsGenericTypeDefinition,
                    IsArray = data.IsArray
                }));
    }

    public class CachedFieldFlags : CachedMemberFlags<CachedFieldFlags.IClnbl, CachedFieldFlags.Immtbl, CachedFieldFlags.Mtbl>
    {
        public interface IClnbl : CachedMemberFlagsBase.IClnblCore, IClnblCore
        {
            bool IsEditable { get; }
            bool IsInitOnly { get; }
            bool IsLiteral { get; }
        }

        public class Immtbl : ImmtblCore, IClnbl
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

        public class Mtbl : MtblCore, IClnbl
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
                data => new Immtbl(new Mtbl
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
                }));
    }

    public class CachedPropertyFlags
    {
        public interface IClnbl
        {
            Lazy<bool> IsStatic { get; }
            bool CanRead { get; }
            bool CanWrite { get; }
            Lazy<CachedMemberFlagsCore.IClnbl> Getter { get; }
            Lazy<CachedMemberFlagsCore.IClnbl> Setter { get; }
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
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

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
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
            ICachedPropertyInfo cached) => new Immtbl(new Mtbl
            {
                IsStatic = new Lazy<bool>(
                    () => (cached.Getter.Value ?? cached.Setter.Value).Flags.Value.IsStatic),
                CanRead = cached.Data.CanRead,
                CanWrite = cached.Data.CanWrite,
                Getter = new Lazy<CachedMemberFlagsCore.IClnbl>(
                    () => cached.Getter.Value?.WithValue(
                    gttr => CachedMemberFlagsCore.Create(gttr))),
                Setter = new Lazy<CachedMemberFlagsCore.IClnbl>(
                    () => cached.Setter.Value?.WithValue(
                    sttr => CachedMemberFlagsCore.Create(sttr)))
            });
    }

    public class CachedEventFlags
    {
        public interface IClnbl
        {
            bool IsMulticast { get; }
            Lazy<CachedMemberFlagsCore.IClnbl> AddMethod { get; }
            Lazy<CachedMemberFlagsCore.IClnbl> RemoveMethod { get; }
            Lazy<CachedMemberFlagsCore.IClnbl> InvokeMethod { get; }
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
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

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
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
            ICachedEventInfo cached) => new Immtbl(new Mtbl
            {
                IsMulticast = cached.Data.IsMulticast,
                AddMethod = new Lazy<CachedMemberFlagsCore.IClnbl>(
                () => cached.Adder.Value?.WithValue(
                adder => CachedMemberFlagsCore.Create(adder))),
                RemoveMethod = new Lazy<CachedMemberFlagsCore.IClnbl>(
                () => cached.Remover.Value?.WithValue(
                adder => CachedMemberFlagsCore.Create(adder))),
                InvokeMethod = new Lazy<CachedMemberFlagsCore.IClnbl>(
                () => cached.Raiser.Value?.WithValue(
                adder => CachedMemberFlagsCore.Create(adder)))
            });
    }

    public class CachedParameterFlags
    {
        public interface IClnbl
        {
            bool IsIn { get; }
            bool IsLcid { get; }
            bool IsOptional { get; }
            bool IsOut { get; }
            bool IsRetval { get; }
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
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

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
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
                data => new Immtbl(new Mtbl
                {
                    IsIn = data.IsIn,
                    IsLcid = data.IsLcid,
                    IsOptional = data.IsOptional,
                    IsOut = data.IsOut,
                    IsRetval = data.IsRetval,
                }));
    }
}

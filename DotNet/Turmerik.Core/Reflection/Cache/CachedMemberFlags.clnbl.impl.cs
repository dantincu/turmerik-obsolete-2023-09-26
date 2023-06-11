using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.Reflection.Cache
{
    public static partial class CachedMemberFlagsCore
    {
        public static CachedMemberFlagsCoreImmtbl ToImmtbl(
            this ICachedMemberFlagsCore src) => new CachedMemberFlagsCoreImmtbl(src);

        public static CachedMemberFlagsCoreImmtbl AsImmtbl(
            this ICachedMemberFlagsCore src) => (src as CachedMemberFlagsCoreImmtbl) ?? src?.ToImmtbl();

        public static CachedMemberFlagsCoreMtbl ToMtbl(
            this ICachedMemberFlagsCore src) => new CachedMemberFlagsCoreMtbl(src);

        public static CachedMemberFlagsCoreMtbl AsMtbl(
            this ICachedMemberFlagsCore src) => (src as CachedMemberFlagsCoreMtbl) ?? src?.ToMtbl();

        public static ReadOnlyCollection<CachedMemberFlagsCoreImmtbl> ToImmtblRdnlC(
            this IEnumerable<ICachedMemberFlagsCore> nmrbl) => nmrbl.Select(
                src => src.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<CachedMemberFlagsCoreImmtbl> AsImmtblRdnlC(
            this IEnumerable<ICachedMemberFlagsCore> nmrbl) => (
            nmrbl as ReadOnlyCollection<CachedMemberFlagsCoreImmtbl>) ?? nmrbl?.ToImmtblRdnlC();

        public static List<CachedMemberFlagsCoreMtbl> ToMtblList(
            this IEnumerable<ICachedMemberFlagsCore> nmrbl) => nmrbl.Select(
                src => src.AsMtbl()).ToList();

        public static ReadOnlyDictionary<TKey, ICachedMemberFlagsCore> ToImmtblRdnlDictnr<TKey>(
            this IEnumerable<KeyValuePair<TKey, ICachedMemberFlagsCore>> dictnr) => dictnr.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsImmtbl() as ICachedMemberFlagsCore).RdnlD();

        public static ReadOnlyDictionary<TKey, ICachedMemberFlagsCore> AsImmtblRdnlDictnr<TKey>(
            this IEnumerable<KeyValuePair<TKey, ICachedMemberFlagsCore>> dictnr) => (
            dictnr as ReadOnlyDictionary<TKey, ICachedMemberFlagsCore>) ?? dictnr.ToImmtblRdnlDictnr();

        public static Dictionary<TKey, ICachedMemberFlagsCore> ToMtblDictnr<TKey>(
            this IEnumerable<KeyValuePair<TKey, ICachedMemberFlagsCore>> dictnr) => dictnr.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsMtbl() as ICachedMemberFlagsCore);

        public static Dictionary<TKey, ICachedMemberFlagsCore> AsMtblDicntr<TKey>(
            this IEnumerable<KeyValuePair<TKey, ICachedMemberFlagsCore>> dictnr) => (
            dictnr as Dictionary<TKey, ICachedMemberFlagsCore>) ?? dictnr.ToMtblDictnr();
    }

    public static partial class CachedMemberFlags
    {
        public static CachedMemberFlagsImmtbl ToImmtbl(
            this ICachedMemberFlags src) => new CachedMemberFlagsImmtbl(src);
    }

    public static partial class CachedTypeFlags
    {
        public static CachedTypeFlagsImmtbl ToImmtbl(
            this ICachedTypeFlags src) => new CachedTypeFlagsImmtbl(src);
    }

    public static partial class CachedFieldFlags
    {
        public static CachedFieldFlagsImmtbl ToImmtbl(
            this ICachedFieldFlags src) => new CachedFieldFlagsImmtbl(src);
    }

    public static partial class CachedPropertyFlags
    {
        public static CachedPropertyFlagsImmtbl ToImmtbl(
            this ICachedPropertyFlags src) => new CachedPropertyFlagsImmtbl(src);
    }

    public static partial class CachedEventFlags
    {
        public static CachedEventFlagsImmtbl ToImmtbl(
            this ICachedEventFlags src) => new CachedEventFlagsImmtbl(src);
    }

    public static partial class CachedParameterFlags
    {
        public static CachedParameterFlagsImmtbl ToImmtbl(
            this ICachedParameterFlags src) => new CachedParameterFlagsImmtbl(src);
    }

    public class CachedMemberFlagsCoreImmtbl : ICachedMemberFlagsCore
    {
        public CachedMemberFlagsCoreImmtbl(ICachedMemberFlagsCore src)
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

    public class CachedMemberFlagsCoreMtbl : ICachedMemberFlagsCore
    {
        public CachedMemberFlagsCoreMtbl()
        {
        }

        public CachedMemberFlagsCoreMtbl(ICachedMemberFlagsCore src)
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

    public class CachedMemberFlagsImmtbl : CachedMemberFlagsCoreImmtbl, ICachedMemberFlags
    {
        public CachedMemberFlagsImmtbl(ICachedMemberFlags src) : base(src)
        {
            IsStatic = src.IsStatic;
        }

        public bool IsStatic { get; }
    }

    public class CachedMemberFlagsMtbl : CachedMemberFlagsCoreMtbl, ICachedMemberFlags
    {
        public CachedMemberFlagsMtbl()
        {
        }

        public CachedMemberFlagsMtbl(ICachedMemberFlags src) : base(src)
        {
            IsStatic = src.IsStatic;
        }

        public bool IsStatic { get; set; }
    }

    public class CachedTypeFlagsImmtbl : ICachedTypeFlags
    {
        public CachedTypeFlagsImmtbl(ICachedTypeFlags src)
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

    public class CachedTypeFlagsMtbl : ICachedTypeFlags
    {
        public CachedTypeFlagsMtbl()
        {
        }

        public CachedTypeFlagsMtbl(ICachedTypeFlags src)
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

    public class CachedFieldFlagsImmtbl : CachedMemberFlagsImmtbl, ICachedFieldFlags
    {
        public CachedFieldFlagsImmtbl(ICachedFieldFlags src) : base(src)
        {
            IsEditable = src.IsEditable;
            IsInitOnly = src.IsInitOnly;
            IsLiteral = src.IsLiteral;
        }

        public bool IsEditable { get; }
        public bool IsInitOnly { get; }
        public bool IsLiteral { get; }
    }

    public class CachedFieldFlagsMtbl : CachedMemberFlagsMtbl, ICachedFieldFlags
    {
        public CachedFieldFlagsMtbl()
        {
        }

        public CachedFieldFlagsMtbl(ICachedFieldFlags src) : base(src)
        {
            IsEditable = src.IsEditable;
            IsInitOnly = src.IsInitOnly;
            IsLiteral = src.IsLiteral;
        }

        public bool IsEditable { get; set; }
        public bool IsInitOnly { get; set; }
        public bool IsLiteral { get; set; }
    }

    public class CachedPropertyFlagsImmtbl : ICachedPropertyFlags
    {
        public CachedPropertyFlagsImmtbl(ICachedPropertyFlags src)
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
        public Lazy<ICachedMemberFlagsCore> Getter { get; }
        public Lazy<ICachedMemberFlagsCore> Setter { get; }
    }

    public class CachedPropertyFlagsMtbl : ICachedPropertyFlags
    {
        public CachedPropertyFlagsMtbl()
        {
        }

        public CachedPropertyFlagsMtbl(ICachedPropertyFlags src)
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
        public Lazy<ICachedMemberFlagsCore> Getter { get; set; }
        public Lazy<ICachedMemberFlagsCore> Setter { get; set; }
    }

    public class CachedEventFlagsImmtbl : ICachedEventFlags
    {
        public CachedEventFlagsImmtbl(ICachedEventFlags src)
        {
            IsMulticast = src.IsMulticast;
            AddMethod = src.AddMethod;
            RemoveMethod = src.RemoveMethod;
            InvokeMethod = src.InvokeMethod;
        }

        public bool IsMulticast { get; }
        public Lazy<ICachedMemberFlagsCore> AddMethod { get; }
        public Lazy<ICachedMemberFlagsCore> RemoveMethod { get; }
        public Lazy<ICachedMemberFlagsCore> InvokeMethod { get; }
    }

    public class CachedEventFlagsMtbl : ICachedEventFlags
    {
        public CachedEventFlagsMtbl()
        {
        }

        public CachedEventFlagsMtbl(ICachedEventFlags src)
        {
            IsMulticast = src.IsMulticast;
            AddMethod = src.AddMethod;
            RemoveMethod = src.RemoveMethod;
            InvokeMethod = src.InvokeMethod;
        }

        public bool IsMulticast { get; set; }
        public Lazy<ICachedMemberFlagsCore> AddMethod { get; set; }
        public Lazy<ICachedMemberFlagsCore> RemoveMethod { get; set; }
        public Lazy<ICachedMemberFlagsCore> InvokeMethod { get; set; }
    }

    public class CachedParameterFlagsImmtbl : ICachedParameterFlags
    {
        public CachedParameterFlagsImmtbl(ICachedParameterFlags src)
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

    public class CachedParameterFlagsMtbl : ICachedParameterFlags
    {
        public CachedParameterFlagsMtbl()
        {
        }

        public CachedParameterFlagsMtbl(ICachedParameterFlags src)
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
}
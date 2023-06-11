using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
}

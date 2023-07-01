using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.MathH;
using Turmerik.Reflection.Cache;
using Turmerik.Text;
using Turmerik.Utils;

namespace Turmerik.Reflection
{
    public static class ReflH
    {
        public static bool IsReferenceType(
            this Type type) => !type.IsValueType;

        public static bool IsPrimitiveType(
            this Type type) => type.IsValueType || type == ReflC.Types.StringType;

        public static string GetTypeFullDisplayName(
            this Type type) => GetTypeFullDisplayName(type.FullName);

        public static string GetTypeFullDisplayName(
            string typeFullName) => typeFullName?.SubStr(
                (str, len) => str.FindVal((c, i) => c == '`').Key).Item1;

        public static bool IsDefault(
            this MemberScope filter) => filter == MemberScope.None;

        public static bool IsDefault(
            this MemberVisibility filter) => filter == MemberVisibility.None;

        public static bool IsDefault(
            this FieldType filter) => filter == FieldType.None;

        public static bool IsDefault(
            this FieldAccessibilityFilter filter) => filter.Visibility.IsDefault(
                ) && filter.Scope.IsDefault() && filter.FieldType.IsDefault();

        public static bool IsDefault(
            this MethodAccessibilityFilter filter) => filter.Visibility.IsDefault(
                ) && filter.Scope.IsDefault();

        public static MemberScope SubstractIfContainsFlag(
            this MemberScope memberScope,
            MemberScope flag)
        {
            if (memberScope.HasFlag(flag))
            {
                memberScope -= flag;
            }

            return memberScope;
        }

        public static MemberVisibility SubstractIfContainsFlag(
            this MemberVisibility memberVisibility,
            MemberVisibility flag)
        {
            if (memberVisibility.HasFlag(flag))
            {
                memberVisibility -= flag;
            }

            return memberVisibility;
        }

        public static MemberVisibility SubstractContainedFlags(
            this MemberVisibility memberVisibility,
            params MemberVisibility[] flagsArr)
        {
            foreach (var flag in flagsArr)
            {
                if (memberVisibility.HasFlag(flag))
                {
                    memberVisibility -= flag;
                }
            }

            return memberVisibility;
        }

        public static MemberScope ToMemberScope(
            bool isInstanceType) => isInstanceType ? MemberScope.Instance : MemberScope.Static;

        public static MemberScope ReduceIfReq(
            this MemberScope memberScope,
            bool isInstanceType) => memberScope.SubstractIfContainsFlag(
                ToMemberScope(isInstanceType));

        public static MemberVisibility ReduceIfReq(
            this MemberVisibility memberVisibility,
            bool excludeInternal = false,
            bool excludePrivate = false) => memberVisibility.SubstractContainedFlags(
                excludeInternal ? MemberVisibility.Internal.Arr(
                    MemberVisibility.PrivateProtected,
                    MemberVisibility.Private) : (excludePrivate ? MemberVisibility.Private.Arr() : new MemberVisibility[0]));

        public static FieldAccessibilityFilter ReduceFilterIfReq(
            this FieldAccessibilityFilter filter,
            bool excludeInternal = false,
            bool excludePrivate = false) => new FieldAccessibilityFilter(
                filter.Scope,
                filter.Visibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate),
                filter.FieldType);

        public static MethodAccessibilityFilter ReduceFilterIfReq(
            this MethodAccessibilityFilter filter,
            bool excludeInternal = false,
            bool excludePrivate = false) => new MethodAccessibilityFilter(
                filter.Scope,
                filter.Visibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate));

        public static MethodAccessibilityFilter ReduceFilterIfReq(
            this MethodAccessibilityFilter filter,
            bool isInstanceType,
            bool excludeInternal = false,
            bool excludePrivate = false) => new MethodAccessibilityFilter(
                filter.Scope.ReduceIfReq(isInstanceType),
                filter.Visibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate));

        public static PropertyAccessibilityFilter ReduceFilterIfReq(
            this PropertyAccessibilityFilter filter,
            bool isInstanceType,
            bool excludeInternal = false,
            bool excludePrivate = false) => new PropertyAccessibilityFilter(
                filter.Scope.ReduceIfReq(isInstanceType),
                filter.CanRead,
                filter.CanWrite,
                filter.Getter?.ReduceFilterIfReq(
                    isInstanceType,
                    excludeInternal,
                    excludePrivate),
                filter.Setter?.ReduceFilterIfReq(
                    isInstanceType,
                    excludeInternal,
                    excludePrivate));

        public static EventAccessibilityFilter ReduceFilterIfReq(
            this EventAccessibilityFilter filter,
            bool excludeInternal = false,
            bool excludePrivate = false) => new EventAccessibilityFilter(
                filter.Adder?.ReduceFilterIfReq(
                    excludeInternal,
                    excludePrivate),
                filter.Remover?.ReduceFilterIfReq(
                    excludeInternal,
                    excludePrivate),
                filter.Invoker?.ReduceFilterIfReq(
                    excludeInternal,
                    excludePrivate));

        public static bool Matches(
            this MemberVisibility visibility,
            ICachedMemberFlagsCore arg) => (arg == null && visibility.IsDefault()) || arg.IfHasAnyFlag(
                visibility,
                new Dictionary<MemberVisibility, Func<ICachedMemberFlagsCore, MemberVisibility, bool>>
                {
                    { MemberVisibility.Public, (mmb, flag) => mmb.IsPublic },
                    { MemberVisibility.Protected, (mmb, flag) => mmb.IsFamily },
                    { MemberVisibility.Internal, (mmb, flag) => mmb.IsAssembly },
                    { MemberVisibility.ProtectedInternal, (mmb, flag) => mmb.IsFamilyOrAssembly },
                    { MemberVisibility.PrivateProtected, (mmb, flag) => mmb.IsFamilyAndAssembly },
                    { MemberVisibility.Private, (mmb, flag) => mmb.IsPrivate }
                },
                false);

        public static bool Matches(
            this MemberScope scope,
            ICachedMemberFlags fld) => (fld == null && scope.IsDefault()) || fld.IfHasAnyFlag(
                scope,
                new Dictionary<MemberScope, Func<ICachedMemberFlags, MemberScope, bool>>
                {
                    { MemberScope.Static, (mmb, flag) => mmb.IsStatic },
                    { MemberScope.Instance, (mmb, flag) => !mmb.IsStatic }
                },
                false);

        public static bool Matches(
        this FieldType fieldType,
            ICachedFieldFlags fld) => (fld == null && fieldType.IsDefault()) || fld.IfHasAnyFlag(
                fieldType,
                new Dictionary<FieldType, Func<ICachedFieldFlags, FieldType, bool>>
                {
                    { FieldType.Editable, (mmb, flag) => mmb.IsEditable },
                    { FieldType.InitOnly, (mmb, flag) => !mmb.IsInitOnly },
                    { FieldType.Literal, (mmb, flag) => !mmb.IsLiteral }
                },
                false);

        public static bool Matches(
            this MethodAccessibilityFilter filter,
            ICachedMethodInfo arg) => (arg == null && filter.IsDefault()) || arg.Flags.Value.WithValue(
                flags => filter.Visibility.Matches(
                    flags) && filter.Scope.Matches(flags));

        public static bool Matches(
            this FieldAccessibilityFilter filter,
            ICachedFieldInfo arg) => (arg == null && filter.IsDefault()) || arg.Flags.Value.WithValue(
                flags => filter.Visibility.Matches(
                     flags) && filter.Scope.Matches(
                    flags) && filter.FieldType.Matches(flags));

        public static bool Matches(
            this PropertyAccessibilityFilter filter,
            ICachedPropertyInfo arg)
        {
            bool retVal = arg.Flags.Value.IsStatic.Value.WithValue(
                isStatic => arg.IfHasAnyFlag(
                    filter.Scope,
                    new Dictionary<MemberScope, Func<ICachedPropertyInfo, MemberScope, bool>>
                    {
                        { MemberScope.Static, (mmb, flag) => isStatic },
                        { MemberScope.Instance, (mmb, flag) => !isStatic }
                    },
                    false));

            if (retVal)
            {
                retVal = filter.Getter?.Matches(arg.Getter.Value) ?? true;
                retVal = retVal && (filter.Setter?.Matches(arg.Setter.Value) ?? true);
            }

            return retVal;
        }

        public static bool Matches(
            this EventAccessibilityFilter filter,
            ICachedEventInfo arg)
        {
            bool retVal = filter.Adder?.Matches(arg.Adder.Value) ?? true;
            retVal = retVal && (filter.Remover?.Matches(arg.Remover.Value) ?? true);
            retVal = retVal && (filter.Invoker?.Matches(arg.Invoker.Value) ?? true);

            return retVal;
        }

        public static bool Matches(
            this MemberVisibility filter,
            ICachedConstructorInfo arg) => filter.Matches(arg.Flags.Value);

        public static bool IsEditable(
            this FieldInfo fieldInfo) => !(
            fieldInfo.IsInitOnly || fieldInfo.IsLiteral);

        public static bool IsStaticClass(
            this Type type) => type.IsAbstract && type.IsSealed;

        public static bool IsInternalOrPrivateProtected(
            this CachedMemberFlagsCore.IClnbl arg) => arg.IsAssembly || arg.IsFamilyAndAssembly;
    }
}

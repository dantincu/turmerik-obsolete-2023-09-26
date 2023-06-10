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
using static System.Formats.Asn1.AsnWriter;

namespace Turmerik.Reflection
{
    public static class ReflH
    {
        public static bool IsReferenceType(
            this Type type) => !type.IsValueType;

        public static bool IsPrimitiveType(
            this Type type) => type.IsValueType || type == ReflC.Types.StringType;

        public static string? GetTypeFullDisplayName(this Type type)
        {
            string? typeFullName = type.FullName?.SubStr(
                (str, len) => str.FindVal((c, i) => c == '`').Key).Item1;

            return typeFullName;
        }

        public static bool Matches(
            this ICachedMemberFlagsCore arg,
            bool notIfInternal = false,
            bool notIfPrivate = false)
        {
            bool retVal = !(notIfInternal && arg.IsInternalOrPrivateProtected());
            retVal = retVal && !(notIfPrivate && arg.IsPrivate);

            return retVal;
        }

        public static bool Matches(
            this ICachedMemberFlagsCore arg,
            MemberVisibility visibility,
            bool notIfInternal = false,
            bool notIfPrivate = false) => arg.Matches(
                notIfInternal,
                notIfPrivate) && arg.IfHasAnyFlag(
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
            ICachedMemberFlags? arg,
            MemberVisibility? visibility,
            bool notIfInternal = false,
            bool notIfPrivate = false) => ((arg == null) == (visibility == null)).IfTrue(
                () => (arg != null).IfTrue(
                    () => arg.Matches(
                        visibility.Value,
                        notIfInternal,
                        notIfPrivate),
                    () => true));

        public static bool Matches(
            this ICachedMemberFlags fld,
            MemberScope scope) => fld.IfHasAnyFlag(
                scope,
                new Dictionary<MemberScope, Func<ICachedMemberFlags, MemberScope, bool>>
                {
                    { MemberScope.Static, (mmb, flag) => mmb.IsStatic },
                    { MemberScope.Instance, (mmb, flag) => !mmb.IsStatic }
                },
                false);

        public static bool Matches(
            this ICachedFieldFlags fld,
            FieldType fieldType) => fld.IfHasAnyFlag(
                fieldType,
                new Dictionary<FieldType, Func<ICachedFieldFlags, FieldType, bool>>
                {
                    { FieldType.Editable, (mmb, flag) => mmb.IsEditable },
                    { FieldType.InitOnly, (mmb, flag) => !mmb.IsInitOnly },
                    { FieldType.Literal, (mmb, flag) => !mmb.IsLiteral }
                },
                false);

        public static bool Matches(
            this ICachedMethodInfo arg,
            MethodAccessibilityFilter filter,
            bool notIfInternal = false,
            bool notIfPrivate = false) => arg.Flags.Value.WithValue(
                flags => flags.Matches(
                    filter.Visibility,
                    notIfInternal,
                    notIfPrivate) && flags.Matches(
                filter.Scope));

        public static bool Matches(
            this ICachedMethodInfo? arg,
            MethodAccessibilityFilter? filter,
            bool notIfInternal = false,
            bool notIfPrivate = false) => ((arg == null) == (filter == null)).IfTrue(
                () => (arg != null).IfTrue(
                    () => arg.Matches(
                        filter.Value,
                        notIfInternal,
                        notIfPrivate),
                    () => true));

        public static bool Matches(
            this ICachedFieldInfo arg,
            FieldAccessibilityFilter filter,
            bool notIfInternal = false,
            bool notIfPrivate = false) => arg.Flags.Value.WithValue(
                flags => flags.Matches(
                    filter.Visibility,
                    notIfInternal,
                    notIfPrivate) && flags.Matches(
                    filter.Scope) && flags.Matches(
                    filter.FieldType));

        public static bool Matches(
            this ICachedPropertyInfo arg,
            PropertyAccessibilityFilter filter,
            bool notIfInternal = false,
            bool notIfPrivate = false)
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
                retVal = arg.Getter.Value?.Flags.Value.Matches(
                    notIfInternal,
                    notIfPrivate) ?? false;

                retVal = retVal || (arg.Setter.Value?.Flags.Value.Matches(
                    notIfInternal,
                    notIfPrivate) ?? false);
            }

            if (retVal)
            {
                retVal = arg.Getter.Value.Matches(
                    filter.Getter,
                    notIfInternal,
                    notIfPrivate);

                retVal = retVal && arg.Setter.Value.Matches(
                    filter.Setter,
                    notIfInternal,
                    notIfPrivate);
            }

            return retVal;
        }

        public static bool Matches(
            this ICachedEventInfo arg,
            EventAccessibilityFilter filter,
            bool notIfInternal = false,
            bool notIfPrivate = false)
        {
            bool retVal = arg.Adder.Value?.Flags.Value.Matches(
                    notIfInternal,
                    notIfPrivate) ?? false;

            retVal = retVal || (arg.Remover.Value?.Flags.Value.Matches(
                notIfInternal,
                notIfPrivate) ?? false);

            retVal = retVal || (arg.Invoker.Value?.Flags.Value.Matches(
                notIfInternal,
                notIfPrivate) ?? false);

            if (retVal)
            {
                retVal = arg.Adder.Value.Matches(
                    filter.Adder,
                    notIfInternal,
                    notIfPrivate);

                retVal = retVal && arg.Remover.Value.Matches(
                    filter.Remover,
                    notIfInternal,
                    notIfPrivate);

                retVal = retVal && arg.Invoker.Value.Matches(
                    filter.Invoker,
                    notIfInternal,
                    notIfPrivate);
            }

            return retVal;
        }

        public static bool IsEditable(
            this FieldInfo fieldInfo) => !(
            fieldInfo.IsInitOnly || fieldInfo.IsLiteral);

        public static bool IsStaticClass(
            this Type type) => type.IsAbstract && type.IsSealed;

        public static bool IsInternalOrPrivateProtected(
            this ICachedMemberFlagsCore arg) => arg.IsAssembly || arg.IsFamilyAndAssembly;
    }
}

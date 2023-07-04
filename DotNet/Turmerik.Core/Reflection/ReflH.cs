using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        static ReflH()
        {
            MemberVisibilityMatchers = new Dictionary<MemberVisibility, Func<CachedMemberFlagsCoreBase.IClnblCore, MemberVisibility, bool>>
                {
                    { MemberVisibility.Public, (mmb, flag) => mmb.IsPublic },
                    { MemberVisibility.Protected, (mmb, flag) => mmb.IsFamily },
                    { MemberVisibility.Internal, (mmb, flag) => mmb.IsAssembly },
                    { MemberVisibility.ProtectedInternal, (mmb, flag) => mmb.IsFamilyOrAssembly },
                    { MemberVisibility.PrivateProtected, (mmb, flag) => mmb.IsFamilyAndAssembly },
                    { MemberVisibility.Private, (mmb, flag) => mmb.IsPrivate }
                }.RdnlD();

            MemberScopeMatchers = new Dictionary<MemberScope, Func<CachedMemberFlagsBase.IClnblCore, MemberScope, bool>>
                {
                    { MemberScope.Static, (mmb, flag) => mmb.IsStatic },
                    { MemberScope.Instance, (mmb, flag) => !mmb.IsStatic }
                }.RdnlD();

            FieldTypeMatchers = new Dictionary<FieldType, Func<CachedFieldFlags.IClnbl, FieldType, bool>>
                {
                    { FieldType.Editable, (mmb, flag) => mmb.IsEditable },
                    { FieldType.InitOnly, (mmb, flag) => !mmb.IsInitOnly },
                    { FieldType.Literal, (mmb, flag) => !mmb.IsLiteral }
                }.RdnlD();

            PropertyScopeMatchers = new Dictionary<MemberScope, Func<CachedPropertyFlags.IClnbl, MemberScope, bool>>
                {
                    { MemberScope.Static, (mmb, flag) => mmb.IsStatic.Value },
                    { MemberScope.Instance, (mmb, flag) => !mmb.IsStatic.Value }
                }.RdnlD();
        }

        public static ReadOnlyDictionary<MemberVisibility, Func<CachedMemberFlagsCoreBase.IClnblCore, MemberVisibility, bool>> MemberVisibilityMatchers { get; }
        public static ReadOnlyDictionary<MemberScope, Func<CachedMemberFlagsBase.IClnblCore, MemberScope, bool>> MemberScopeMatchers { get; }
        public static ReadOnlyDictionary<MemberScope, Func<CachedPropertyFlags.IClnbl, MemberScope, bool>> PropertyScopeMatchers { get; }
        public static ReadOnlyDictionary<FieldType, Func<CachedFieldFlags.IClnbl, FieldType, bool>> FieldTypeMatchers { get; }

        public static bool IsReferenceType(
            this Type type) => !type.IsValueType;

        public static bool IsPrimitiveType(
            this Type type) => type.IsValueType || type == ReflC.Types.StringType;

        public static string GetTypeFullDisplayName(
            this Type type) => GetTypeFullDisplayName(type.FullName);

        public static string GetTypeFullDisplayName(
            string typeFullName) => typeFullName?.SubStr(
                (str, len) => str.FindVal((c, i) => c == '`').Key).Item1;

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

        public static MemberScope ToFlagOrNone(
            this MemberScope memberScope,
            MemberScope flag)
        {
            if (memberScope.HasFlag(flag))
            {
                memberScope = flag;
            }
            else
            {
                memberScope = MemberScope.None;
            }

            return memberScope;
        }

        public static MemberVisibility ToFlagOrNone(
            this MemberVisibility memberVisibility,
            MemberVisibility flag)
        {
            if (memberVisibility.HasFlag(flag))
            {
                memberVisibility = flag;
            }
            else
            {
                memberVisibility = MemberVisibility.None;
            }

            return memberVisibility;
        }

        public static MemberScope ToMemberScope(
            bool isInstanceType) => isInstanceType ? MemberScope.Instance : MemberScope.Static;

        public static MemberScope ReduceIfReq(
            this MemberScope memberScope,
            bool isInstanceType) => memberScope.SubstractIfContainsFlag(
                ToMemberScope(!isInstanceType));

        public static MemberVisibility ReduceIfReq(
            this MemberVisibility memberVisibility,
            bool excludeInternal = false,
            bool excludePrivate = false,
            bool publicOnly = false,
            bool excludeInheritables = false)
        {
            if (publicOnly)
            {
                memberVisibility = memberVisibility.ToFlagOrNone(
                    MemberVisibility.Public);
            }
            else
            {
                var removableFlags = new List<MemberVisibility>();

                if (excludeInheritables)
                {
                    removableFlags.Add(MemberVisibility.Protected);
                }

                if (excludeInternal)
                {
                    removableFlags.AddRange(
                        MemberVisibility.Internal,
                        MemberVisibility.PrivateProtected);
                }

                if (excludePrivate || excludeInternal)
                {
                    removableFlags.Add(MemberVisibility.Private);
                }

                if (excludeInternal && excludeInheritables)
                {
                    removableFlags.Add(MemberVisibility.ProtectedInternal);
                }

                memberVisibility = memberVisibility.SubstractContainedFlags(
                    removableFlags.ToArray());
            }

            return memberVisibility;
        }

        public static FieldAccessibilityFilter ReduceFilterIfReq(
            this FieldAccessibilityFilter filter,
            bool excludeInternal = false,
            bool excludePrivate = false,
            bool excludeInheritables = false) => new FieldAccessibilityFilter(
                filter.Scope,
                filter.Visibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate,
                    excludeInheritables),
                filter.FieldType);

        public static MethodAccessibilityFilter ReduceFilterIfReq(
            this MethodAccessibilityFilter filter,
            bool excludeInternal,
            bool excludePrivate,
            bool publicOnly = false,
            bool excludeInheritables = false) => new MethodAccessibilityFilter(
                filter.Scope,
                filter.Visibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate,
                    publicOnly,
                    excludeInheritables));

        public static MethodAccessibilityFilter ReduceFilterIfReq(
            this MethodAccessibilityFilter filter,
            bool isInstanceType,
            bool excludeInternal,
            bool excludePrivate,
            bool publicOnly = false,
            bool excludeInheritables = false) => new MethodAccessibilityFilter(
                filter.Scope.ReduceIfReq(isInstanceType),
                filter.Visibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate,
                    publicOnly,
                    excludeInheritables));

        public static PropertyAccessibilityFilter ReduceFilterIfReq(
            this PropertyAccessibilityFilter filter,
            bool isInstanceType,
            bool excludeInternal,
            bool excludePrivate,
            bool publicOnly = false,
            bool excludeInheritables = false) => new PropertyAccessibilityFilter(
                filter.Scope.ReduceIfReq(isInstanceType),
                filter.CanRead,
                filter.CanWrite,
                filter.GetterVisibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate,
                    publicOnly,
                    excludeInheritables),
                filter.SetterVisibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate,
                    publicOnly,
                    excludeInheritables));

        /// <summary>
        /// According to https://stackoverflow.com/questions/14885325/eventinfo-getraisemethod-always-null <br /> <br />
        /// The <see cref="System.Reflection.EventInfo.RaiseMethod"/> and <see cref="System.Reflection.EventInfo.GetRaiseMethod"/>
        /// will always return <c>null</c> value in the C# language (as opposed to VB.NET, F# and C++/CLI). <br /> <br />
        /// But let's leave just it here for now (the <see cref="EventAccessibilityFilter" /> will always be reduced to an equivallent filter
        /// that has <see cref="MemberVisibility.None"/> for the <see cref="RaiserVisibility"/> property).
        /// </summary>
        public static EventAccessibilityFilter ReduceFilterIfReq(
            this EventAccessibilityFilter filter,
            bool excludeInternal,
            bool excludePrivate,
            bool publicOnly = false) => new EventAccessibilityFilter(
                filter.AdderVisibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate,
                    publicOnly),
                filter.RemoverVisibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate,
                    publicOnly),
                MemberVisibility.None
                /* filter.RaiserVisibility.ReduceIfReq(
                    excludeInternal,
                    excludePrivate,
                    publicOnly*/);

        public static bool Matches(
            this MemberVisibility visibility,
            CachedMemberFlagsCoreBase.IClnblCore flags) => flags.IfHasAnyFlag(
                visibility,
                MemberVisibilityMatchers,
                false);

        public static bool Matches(
            this MemberScope scope,
            CachedMemberFlagsBase.IClnblCore flags) => flags.IfHasAnyFlag(
                scope,
                MemberScopeMatchers,
                false);

        public static bool Matches(
        this FieldType fieldType,
            CachedFieldFlags.IClnbl flags) => flags.IfHasAnyFlag(
                fieldType,
                FieldTypeMatchers,
                false);

        public static bool Matches(
            this ICachedMethodInfo arg,
            MemberVisibility filter) => arg.Flags.Value.WithValue(
                flags => filter.Matches(flags));

        public static bool Matches(
            this ICachedMethodInfo arg,
            MethodAccessibilityFilter filter) => arg.Flags.Value.Matches(
                filter.Visibility,
                filter.Scope);

        public static bool Matches(
            this ICachedFieldInfo arg,
            FieldAccessibilityFilter filter) => arg.Flags.Value.WithValue(
                flags => flags.Matches(
                    filter.Visibility,
                    filter.Scope) && filter.FieldType.Matches(flags));

        public static bool Matches(
            this ICachedPropertyInfo argument,
            PropertyAccessibilityFilter filter)
        {
            bool retVal = argument.Flags.Value.IfHasAnyFlag(
                filter.Scope,
                PropertyScopeMatchers,
                false);

            retVal = retVal && (filter.CanRead?.WithValue(
                canRead => argument.Flags.Value.CanRead) ?? true);

            retVal = retVal && (filter.CanWrite?.WithValue(
                canRead => argument.Flags.Value.CanWrite) ?? true);

            retVal = retVal && filter.GetterVisibility.IfMatchesNone(
                () => argument.Getter.Value == null,
                () => argument.Getter.Value?.Matches(
                    filter.GetterVisibility) ?? false);

            retVal = retVal && filter.SetterVisibility.IfMatchesNone(
                () => argument.Setter.Value == null,
                () => argument.Setter.Value?.Matches(
                    filter.SetterVisibility) ?? false);

            return retVal;
        }

        public static bool Matches(
            this ICachedEventInfo argument,
            EventAccessibilityFilter filter)
        {
            bool retVal = filter.AdderVisibility.IfMatchesNone(
                () => argument.Adder.Value == null,
                () => argument.Adder.Value?.Matches(
                    filter.AdderVisibility) ?? false);

            retVal = retVal && filter.RemoverVisibility.IfMatchesNone(
                () => argument.Remover.Value == null,
                () => argument.Remover.Value?.Matches(
                    filter.RemoverVisibility) ?? false);

            retVal = retVal && filter.RaiserVisibility.IfMatchesNone(
                () => argument.Raiser.Value == null,
                () => argument.Raiser.Value?.Matches(
                    filter.RaiserVisibility) ?? false);

            return retVal;
        }

        public static bool Matches(
            this CachedMemberFlagsBase.IClnblCore flags,
            MemberVisibility visibility,
            MemberScope scope) => visibility.Matches(flags) && scope.Matches(flags);

        public static bool Matches(
            this ICachedConstructorInfo arg,
            MemberVisibility filter) => filter.Matches(arg.Flags.Value);

        public static bool MatchesNone(
            this MemberVisibility memberVisibility) => memberVisibility == MemberVisibility.None;

        public static bool MatchesNone(
            this MemberScope memberScope) => memberScope == MemberScope.None;

        public static bool MatchesNone(
            this MemberVisibility memberVisibility,
            MemberScope memberScope) => memberVisibility.MatchesNone() || memberScope.MatchesNone();

        public static bool MatchesNone(
            this FieldType fieldType) => fieldType == FieldType.None;

        public static bool AllMatchNone(
            this MemberVisibility[] memberVisibilities) => memberVisibilities.All(
                visibility => visibility.MatchesNone());

        public static bool IfMatchesNone(
            this MemberVisibility memberVisibility,
            Func<bool> ifMatchesNone,
            Func<bool> ifCanMatch) => memberVisibility.MatchesNone().IfTrue(
                ifMatchesNone,
                ifCanMatch);

        public static bool IfMatchesNone(
            this MemberScope memberScope,
            Func<bool> ifMatchesNone,
            Func<bool> ifCanMatch) => memberScope.MatchesNone().IfTrue(
                ifMatchesNone,
                ifCanMatch);

        public static bool IfMatchesNone(
            this FieldType fieldType,
            Func<bool> ifMatchesNone,
            Func<bool> ifCanMatch) => fieldType.MatchesNone().IfTrue(
                ifMatchesNone,
                ifCanMatch);

        public static bool MatchesNone(
            this FieldAccessibilityFilter filter) => filter.Visibility.MatchesNone(
                filter.Scope) || filter.FieldType.MatchesNone();

        public static bool MatchesNone(
            this MethodAccessibilityFilter filter,
            bool? instanceOnly = null) => filter.Visibility.MatchesNone(
                filter.Scope) || (
                    instanceOnly.HasValue && filter.Scope.ReduceIfReq(
                        instanceOnly.Value).MatchesNone());

        public static bool MatchesNone(
            this PropertyAccessibilityFilter filter) => !(
            (filter.CanRead ?? true) || (
                filter.CanWrite ?? true)) || filter.GetterVisibility.Arr(
                    filter.SetterVisibility).AllMatchNone();

        public static bool MatchesNone(
            this EventAccessibilityFilter filter) => filter.AdderVisibility.Arr(
                filter.RemoverVisibility,
                filter.RaiserVisibility).AllMatchNone();

        public static bool IsEditable(
            this FieldInfo fieldInfo) => !(
            fieldInfo.IsInitOnly || fieldInfo.IsLiteral);

        public static bool IsStaticClass(
            this Type type) => type.IsAbstract && type.IsSealed;

        public static bool IsInternalOrPrivateProtected(
            this CachedMemberFlagsCore.IClnbl arg) => arg.IsAssembly || arg.IsFamilyAndAssembly;
    }
}

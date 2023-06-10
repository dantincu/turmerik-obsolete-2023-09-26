using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Utils;

namespace Turmerik.Reflection
{
    [Flags]
    public enum MemberScope
    {
        None = 0,
        Instance = 1,
        Static = 2
    }

    [Flags]
    public enum MemberVisibility
    {
        None = 0,
        Public = 1,
        Protected = 2,
        Internal = 4,
        ProtectedInternal = 8,
        PrivateProtected = 16,
        Private = 32
    }

    public enum FieldType
    {
        None = 0,
        Editable = 1,
        InitOnly = 2,
        Literal = 4
    }

    public interface IFieldAccessibiliyFilterEqualityComparer : IEqualityComparer<FieldAccessibilityFilter>
    {
    }

    public interface IPropertyAccessibiliyFilterEqualityComparer : IEqualityComparer<PropertyAccessibilityFilter>
    {
    }

    public interface IEventAccessibiliyFilterEqualityComparer : IEqualityComparer<EventAccessibilityFilter>
    {
    }

    public interface IMethodAccessibiliyFilterEqualityComparer : IEqualityComparer<MethodAccessibilityFilter>
    {
    }

    public interface IMemberAccessibiliyFilterEqualityComparerFactory
    {
        IMethodAccessibiliyFilterEqualityComparer Method();
        IFieldAccessibiliyFilterEqualityComparer Field();
        IPropertyAccessibiliyFilterEqualityComparer Property();
        IEventAccessibiliyFilterEqualityComparer Event();
    }

    public readonly struct FieldAccessibilityFilter
    {
        public readonly MemberScope Scope;
        public readonly MemberVisibility Visibility;
        public readonly FieldType FieldType;

        public FieldAccessibilityFilter(
            MemberScope scope = default,
            MemberVisibility visibility = default,
            FieldType fieldType = default)
        {
            Scope = scope;
            Visibility = visibility;
            FieldType = fieldType;
        }

        public override int GetHashCode() => (
            (int)Visibility).BasicHashCode(
                (int)Scope,
                (int)FieldType);
    }

    public readonly struct PropertyAccessibilityFilter
    {
        public readonly MemberScope Scope;
        public readonly bool CanRead;
        public readonly bool CanWrite;
        public readonly MethodAccessibilityFilter? Getter;
        public readonly MethodAccessibilityFilter? Setter;

        public PropertyAccessibilityFilter(
            MemberScope scope = default,
            bool canRead = true,
            bool canWrite = true,
            MethodAccessibilityFilter? getter = null,
            MethodAccessibilityFilter? setter = null)
        {
            Scope = scope;
            CanRead = canRead;
            CanWrite = canWrite;
            Getter = getter;
            Setter = setter;
        }

        public override int GetHashCode() => (
            (int)Scope).BasicHashCode(
                CanRead ? 128 : 0,
                CanRead ? 256 : 0,
                Setter?.GetHashCode() ?? 0,
                Getter?.GetHashCode() ?? 0);
    }

    public readonly struct EventAccessibilityFilter
    {
        public readonly MethodAccessibilityFilter? Adder;
        public readonly MethodAccessibilityFilter? Remover;
        public readonly MethodAccessibilityFilter? Invoker;

        public EventAccessibilityFilter(
            MethodAccessibilityFilter? adder,
            MethodAccessibilityFilter? remover,
            MethodAccessibilityFilter? invoker)
        {
            Adder = adder;
            Remover = remover;
            Invoker = invoker;
        }
    }

    public readonly struct MethodAccessibilityFilter
    {
        public readonly MemberScope Scope;
        public readonly MemberVisibility Visibility;

        public MethodAccessibilityFilter(
            MemberScope scope = default,
            MemberVisibility visibility = default)
        {
            Scope = scope;
            Visibility = visibility;
        }

        public override int GetHashCode() => (
            (int)Visibility).BasicHashCode(
                (int)Scope);
    }

    public class FieldAccessibilityFilterEqualityComparer : EqualityComparer<FieldAccessibilityFilter>, IFieldAccessibiliyFilterEqualityComparer
    {
        public override bool Equals(
            FieldAccessibilityFilter x,
            FieldAccessibilityFilter y)
        {
            bool areEqual = x.Scope == y.Scope;
            areEqual = areEqual && x.Visibility == y.Visibility;
            areEqual = areEqual && x.FieldType == y.FieldType;

            return areEqual;
        }

        public override int GetHashCode(
            [DisallowNull] FieldAccessibilityFilter obj) => obj.GetHashCode();
    }

    public class PropertyAccessibiliyFilterEqualityComparer : EqualityComparer<PropertyAccessibilityFilter>, IPropertyAccessibiliyFilterEqualityComparer
    {
        private readonly IEqualityComparer<MethodAccessibilityFilter> methodEqCompr;

        public PropertyAccessibiliyFilterEqualityComparer(
            IEqualityComparer<MethodAccessibilityFilter> methodEqCompr)
        {
            this.methodEqCompr = methodEqCompr ?? throw new ArgumentNullException(
                nameof(methodEqCompr));
        }

        public override bool Equals(
            PropertyAccessibilityFilter x,
            PropertyAccessibilityFilter y)
        {
            bool areEqual = x.Scope == y.Scope;
            areEqual = areEqual && x.CanRead == y.CanRead;
            areEqual = areEqual && x.CanWrite == y.CanWrite;

            areEqual = areEqual && (x.Getter == null) == (y.Getter == null);
            areEqual = areEqual && (x.Setter == null) == (y.Setter == null);

            if (areEqual)
            {
                if (x.Setter != null)
                {
                    areEqual = areEqual && methodEqCompr.Equals(
                        x.Setter.Value,
                        y.Setter!.Value);
                }

                if (x.Getter != null)
                {
                    areEqual = areEqual && methodEqCompr.Equals(
                        x.Getter.Value,
                        y.Getter!.Value);
                }
            }

            return areEqual;
        }

        public override int GetHashCode(
            [DisallowNull] PropertyAccessibilityFilter obj) => obj.GetHashCode();
    }

    public class EventAccessibiliyFilterEqualityComparer : EqualityComparer<EventAccessibilityFilter>, IEventAccessibiliyFilterEqualityComparer
    {
        private readonly IEqualityComparer<MethodAccessibilityFilter> memberEqCompr;

        public EventAccessibiliyFilterEqualityComparer(
            IEqualityComparer<MethodAccessibilityFilter> memberEqCompr)
        {
            this.memberEqCompr = memberEqCompr ?? throw new ArgumentNullException(
                nameof(memberEqCompr));
        }

        public override bool Equals(
            EventAccessibilityFilter x,
            EventAccessibilityFilter y)
        {
            bool areEqual = (x.Adder == null) == (y.Adder == null);
            areEqual = areEqual && (x.Remover == null) == (y.Remover == null);
            areEqual = areEqual && (x.Invoker == null) == (y.Invoker == null);

            if (areEqual)
            {
                if (x.Adder != null)
                {
                    areEqual = areEqual && memberEqCompr.Equals(
                        x.Adder.Value,
                        y.Adder!.Value);
                }

                if (x.Remover != null)
                {
                    areEqual = areEqual && memberEqCompr.Equals(
                        x.Remover.Value,
                        y.Remover!.Value);
                }

                if (x.Invoker != null)
                {
                    areEqual = areEqual && memberEqCompr.Equals(
                        x.Invoker.Value,
                        y.Invoker!.Value);
                }
            }

            return areEqual;
        }

        public override int GetHashCode(
            [DisallowNull] EventAccessibilityFilter obj) => obj.GetHashCode();
    }

    public class MethodAccessibiliyFilterEqualityComparer : EqualityComparer<MethodAccessibilityFilter>, IMethodAccessibiliyFilterEqualityComparer
    {
        public override bool Equals(
            MethodAccessibilityFilter x,
            MethodAccessibilityFilter y)
        {
            bool areEqual = x.Scope == y.Scope;
            areEqual = areEqual && x.Visibility == y.Visibility;

            return areEqual;
        }

        public override int GetHashCode(
            [DisallowNull] MethodAccessibilityFilter obj) => obj.GetHashCode();
    }

    public class MemberAccessibiliyFilterEqualityComparerFactory : IMemberAccessibiliyFilterEqualityComparerFactory
    {
        private readonly MethodAccessibiliyFilterEqualityComparer method;
        private readonly FieldAccessibilityFilterEqualityComparer field;
        private readonly PropertyAccessibiliyFilterEqualityComparer property;
        private readonly EventAccessibiliyFilterEqualityComparer @event;

        public MemberAccessibiliyFilterEqualityComparerFactory()
        {
            method = new MethodAccessibiliyFilterEqualityComparer();
            field = new FieldAccessibilityFilterEqualityComparer();
            property = new PropertyAccessibiliyFilterEqualityComparer(method);
            @event = new EventAccessibiliyFilterEqualityComparer(method);
        }

        public IMethodAccessibiliyFilterEqualityComparer Method() => method;
        public IFieldAccessibiliyFilterEqualityComparer Field() => field;
        public IPropertyAccessibiliyFilterEqualityComparer Property() => property;
        public IEventAccessibiliyFilterEqualityComparer Event() => @event;
    }
}

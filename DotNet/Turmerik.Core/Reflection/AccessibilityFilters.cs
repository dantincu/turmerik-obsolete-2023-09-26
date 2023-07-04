using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Utils;
using Turmerik.MathH;
using static Turmerik.Reflection.ReflC.Filter;

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

    public interface IMemberVisibilityFilterEqualityComparer : IEqualityComparer<MemberVisibility>
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
            MemberScope? scope = null,
            MemberVisibility? visibility = null,
            FieldType? fieldType = null)
        {
            Scope = scope ?? ReflC.Filter.Scope.All;
            Visibility = visibility ?? ReflC.Filter.Visibility.All;
            FieldType = fieldType ?? ReflC.Filter.FieldTypes.All;
        }

        public override int GetHashCode() => (
            (int)Visibility).BasicHashCode(
                (int)Scope,
                (int)FieldType);
    }

    public readonly struct PropertyAccessibilityFilter
    {
        public readonly MemberScope Scope;
        public readonly bool? CanRead;
        public readonly bool? CanWrite;
        public readonly MemberVisibility GetterVisibility;
        public readonly MemberVisibility SetterVisibility;

        public PropertyAccessibilityFilter(
            MemberScope? scope = null,
            bool? canRead = null,
            bool? canWrite = null,
            MemberVisibility? getterVisibility = null,
            MemberVisibility? setterVisibility = null)
        {
            Scope = scope ?? ReflC.Filter.Scope.All;
            CanRead = canRead;
            CanWrite = canWrite;
            GetterVisibility = getterVisibility ?? Visibility.All;
            SetterVisibility = setterVisibility ?? Visibility.All;
        }

        public override int GetHashCode() => (
            (int)Scope).BasicHashCode(
                CanRead.HasValue ? CanRead.Value ? 128 : 64 : 0,
                CanWrite.HasValue ? CanWrite.Value ? 192 : 96 : 0,
                (int)SetterVisibility,
                (int)GetterVisibility);
    }

    public readonly struct EventAccessibilityFilter
    {
        public readonly MemberVisibility AdderVisibility;
        public readonly MemberVisibility RemoverVisibility;
        public readonly MemberVisibility RaiserVisibility;

        public EventAccessibilityFilter(
            MemberVisibility? adderVisibility,
            MemberVisibility? removerVisibility,
            MemberVisibility? invokerVisibility)
        {
            AdderVisibility = adderVisibility ?? Visibility.All;
            RemoverVisibility = removerVisibility ?? Visibility.All;
            RaiserVisibility = invokerVisibility ?? Visibility.All;
        }

        public override int GetHashCode() => (
            (int)AdderVisibility).BasicHashCode(
                (int)RemoverVisibility,
                (int)RaiserVisibility);
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
            FieldAccessibilityFilter obj) => obj.GetHashCode();
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

            areEqual = areEqual && x.GetterVisibility == y.GetterVisibility;
            areEqual = areEqual && x.SetterVisibility == y.SetterVisibility;

            return areEqual;
        }

        public override int GetHashCode(
            PropertyAccessibilityFilter obj) => obj.GetHashCode();
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
            bool areEqual = x.AdderVisibility == y.AdderVisibility;
            areEqual = areEqual && x.RemoverVisibility == y.RemoverVisibility;
            areEqual = areEqual && x.RaiserVisibility == y.RaiserVisibility;

            return areEqual;
        }

        public override int GetHashCode(
            EventAccessibilityFilter obj) => obj.GetHashCode();
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
            MethodAccessibilityFilter obj) => obj.GetHashCode();
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

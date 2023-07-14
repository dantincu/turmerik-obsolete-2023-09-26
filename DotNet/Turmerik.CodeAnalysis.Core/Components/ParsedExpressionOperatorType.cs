using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public enum ParsedExpressionOperatorType
    {
        None = 0,
        Not,
        Increment,
        Decrement,
        LeftShift,
        RightShift,
        UnsignedRightShift,

        AddresOff,
        PointerIndirection,
        PointerMemberAccess,

        NullCoallescing,
        NullCoallescingAssignment,
        MemberAccess,
        NullConditional,
        NullForgiving,

        Assignment,
        Equality,
        NonEquality,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Plus,
        Minus,
        Times,
        DivideToQuot,
        DivideToRest,
        LogicalAnd,
        LogicalOr,
        LogicalExclusiveOr,
        BitwiseAnd,
        BitwiseOr,

        From,
        Join,
        Group,
        By,
        Into,
        OrderBy,
        Select,
        Ascending,
        Descending,

        TernaryConditional,
        Indexer,
        MethodCall,
        New,
        Stackalloc,
        In,
        Is,
        Typeof,
        TypeCast,
        NullableTypeCast,
        Nameof,
        Sizeof,
        With,
        Delegate,
        DefaultValue,
        Await,
        NamespaceAlias,

        SwitchPatternMatch,
        LambdaExpression,
        TupleConstruct,
        TupleDeconstruct
    }
}

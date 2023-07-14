using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public enum ParsedLiteralExpressionType
    {
        None = 0,
        NumberLiteral,
        StringLiteral,
        InterpolatedStringLiteral,
        RegexLiteral,
        LambdaExpression,
        AnonymousDelegateExpression
    }
}

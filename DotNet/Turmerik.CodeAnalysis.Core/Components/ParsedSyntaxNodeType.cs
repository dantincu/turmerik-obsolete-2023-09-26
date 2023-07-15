using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public enum ParsedSyntaxNodeType
    {
        None = 0,
        Keyword,
        Operator,
        OperatorSymbol,
        NamespaceDeclaration,
        TypeDefinition,
        TypeMemberDeclaration,
        AttributeDecoration,
        LocalVariableDeclaration,
        ParameterDefinition,
        Expression,
        Statement,
        StatementsBlock,
        SwitchCase
    }
}

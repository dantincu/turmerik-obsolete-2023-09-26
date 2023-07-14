using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public enum ParsedStatementType
    {
        None = 0,
        Assignment,
        MethodCall,
        If,
        Else,
        ElseIf,
        For,
        ForEach,
        Break,
        Continue,
        Return,
        YieldReturn,
        Switch,
    }
}

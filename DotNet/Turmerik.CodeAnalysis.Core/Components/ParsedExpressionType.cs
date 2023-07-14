using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public enum ParsedExpressionType
    {
        None = 0,
        TypeofOp,
        ThrowException,
        MethodCall,
        UnaryOp,
        BinaryOp,
        TernaryOp,
        PatternSwitch,
        Literal
    }
}

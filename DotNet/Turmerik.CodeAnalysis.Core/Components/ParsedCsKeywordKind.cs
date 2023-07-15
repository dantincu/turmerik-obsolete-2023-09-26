using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.CodeAnalysis.Core.Components
{
    /// <summary>
    /// A complete list of C# keywords taken from https://www.programiz.com/csharp-programming/keywords-identifiers <br />
    /// </summary>
    public enum ParsedCsKeywordKind
    {
        None = 0,

        This,
        Base,

        Object,
        Bool,
        Byte,
        Char,
        Decimal,
        Double,
        Float,
        Int,
        Long,
        Sbyte,
        Short,
        String,
        Uint,
        Ulong,
        Ushort,

        False,
        True,
        Null,

        Goto,
        Switch,
        Case,
        For,
        Foreach,
        Do,
        While,
        Break,
        Continue,
        Try,
        Catch,
        Finally,
        If,
        Else,
        Lock,
        Return,
        Yield,
        Throw,
        Using,

        Internal,
        Private,
        Protected,
        Public,
        Partial,

        Volatile,
        Abstract,
        Sealed,
        Static,
        Const,
        Readonly,
        Override,
        Operator,

        Class,
        Interface,
        Struct,
        Enum,
        Delegate,
        Event,
        Namespace,

        New,
        Stackalloc,
        As,
        Is,
        Out,
        In,
        Params,
        Ref,
        Default,
        Void,
        Nameof,
        Sizeof,
        Typeof,

        Explicit,
        Implicit,
        Extern,
        Checked,
        Unchecked,
        Unsafe,
        Fixed,
    }
}

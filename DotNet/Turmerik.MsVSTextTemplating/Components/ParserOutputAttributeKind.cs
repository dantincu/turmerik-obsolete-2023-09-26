using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.MsVSTextTemplating.Components
{
    public enum ParserOutputAttributeKind
    {
        None = 0,
        ClnblNsType,
        ClnblType,
        ImmtblType,
        MtblType,
        ClnblIgnoreMethod,
        ClnblIgnoreProperty
    }
}

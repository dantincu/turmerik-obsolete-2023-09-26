using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.CodeAnalysis.Core.Components
{
    public enum ParsedMemberKind
    {
        None = 0,
        Property,
        Method,
        Field,
        Event
    }
}

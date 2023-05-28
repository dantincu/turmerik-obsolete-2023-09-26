using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Collections
{
    public delegate int IdxRetriever<T, TNmrbl>(TNmrbl nmrbl, int count) where TNmrbl : IEnumerable<T>;
}

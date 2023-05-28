using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Collections
{
    public static class FilterH
    {
        public static IEnumerable<T> NotNull<T>(
            this IEnumerable<T> nmrbl) => nmrbl.Where(
            item => item != null);
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Utils
{
    public delegate void RefAction<T>(ref T t);

    public delegate bool TryParse<TInput, TOutput>(
        TInput input,
        out TOutput output);
}

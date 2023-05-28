using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Utils;

namespace Turmerik.Core.Collections
{
    public delegate void ForEachCallback<T>(T value, MutableValueWrapper<bool> @break);
    public delegate void ForCallback<T>(T value, int idx, MutableValueWrapper<bool> @break);
    public delegate void ForIdxCallback(int idx, MutableValueWrapper<bool> @break);
}

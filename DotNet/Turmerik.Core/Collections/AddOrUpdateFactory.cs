using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Collections
{
    public delegate TValue UpdateDictnrValue<TKey, TValue>(
        TKey key,
        bool isUpdate,
        TValue value);
}

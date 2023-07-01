﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Utils
{
    public delegate void RefAction<T>(ref T t);

    public delegate bool TryRetrieve<TInput, TOutput>(
        TInput input,
        out TOutput output);

    public delegate bool TryRetrieve<TObj, TInput, TOutput>(
        TObj @obj,
        TInput input,
        out TOutput output);
}

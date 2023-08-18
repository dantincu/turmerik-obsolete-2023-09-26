﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Utils
{
    public static class EnumsH
    {
        public static TEnum AsEnum<TEnum>(
            this int intVal)
            where TEnum : struct, Enum => (TEnum)(object)intVal;
    }
}

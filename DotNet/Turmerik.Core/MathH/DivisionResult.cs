﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.MathH
{
    public interface IDivisionResult<T>
        where T : INumber<T>
    {
        public T Quotient { get; }
        public T Remainder { get; }

        Tuple<T, T> ToTuple();
    }

    public class DivisionResult<T> : IDivisionResult<T>
        where T : INumber<T>
    {
        public DivisionResult(T quotient, T remainder)
        {
            this.Quotient = quotient;
            this.Remainder = remainder;
        }

        public T Quotient { get; }
        public T Remainder { get; }

        public Tuple<T, T> ToTuple() => new Tuple<T, T>(this.Quotient, this.Remainder);
    }
}

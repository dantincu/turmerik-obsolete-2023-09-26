using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.MathH
{
    public readonly struct SignedNumber<TNumber>
        where TNumber : INumber<TNumber>
    {
        public SignedNumber(TNumber value, TNumber sign)
        {
            Value = value;
            Sign = sign;
        }

        public TNumber Value { get; }
        public TNumber Sign { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests.Components
{
    [CAttr3]
    internal class C3 : MidC2<int>, IC3
    {
        public const long C3_PUB_CONST_LONG_VAL = 1;
        internal const long C3_INTERNAL_CONST_LONG_VAL = 2;
        protected internal const long C3_PROT_INTERNAL_CONST_LONG_VAL = 3;
        protected const long C3_PROT_CONST_LONG_VAL = 4;
        private protected const long C3_PRIV_PROT_CONST_LONG_VAL = 5;
        private const long C3_PRV_CONST_LONG_VAL = 6;

        public static readonly long C3PubStaticReadonlyLongVal = 7;
        internal static readonly long C3InternalStaticReadonlyLongVal = 8;
        protected internal static readonly long C3ProtInternalStaticReadonlyLongVal = 9;
        protected static readonly long C3ProtStaticReadonlyLongVal = 10;
        private protected static readonly long C3PrvProtStaticReadonlyLongVal = 11;
        private static readonly long C3PrvStaticReadonlyLongVal = 12;

        public static long C3PubStaticLongVal = 13;
        internal static long C3InternalStaticLongVal = 14;
        protected internal static long C3ProtInternalStaticLongVal = 15;
        protected static long C3ProtStaticLongVal = 16;
        private protected static long C3PrvProtStaticLongVal = 17;
        private static long C3PrvStaticLongVal = 18;

        public readonly long C3PubReadonlyLongVal = 19;
        internal readonly long C3InternalReadonlyLongVal = 20;
        protected internal readonly long C3ProtInternalReadonlyLongVal = 21;
        protected readonly long C3ProtReadonlyLongVal = 22;
        private protected readonly long C3PrvProtReadonlyLongVal = 23;
        private readonly long C3PrvReadonlyLongVal = 24;

        public long C3PubLongVal = 19;
        internal long C3InternalLongVal = 20;
        protected internal long C3ProtInternalLongVal = 21;
        protected long C3ProtLongVal = 22;
        private protected long C3PrvProtLongVal = 23;
        private long C3PrvLongVal = 24;

        private Action<short> c3Event;

        public C3(string pubStrVal)
        {
        }

        [Attr3]
        protected C3()
        {
        }

        private C3(int pubIntVal)
        {
        }

        public static string C3PubStaticStrVal { get; set; }
        internal static string C3InternalStaticStrVal { get; set; }
        protected internal static string C3ProtInternalStaticStrVal { get; set; }
        protected static string C3ProtStaticStrVal { get; set; }
        private protected static string C3PrvProtStaticStrVal { get; set; }
        private static string C3PrvStaticStrVal { get; set; }

        public virtual string C3PubStrVal { get; set; }
        internal virtual string C3InternalStrVal { get; set; }
        protected internal virtual string C3ProtInternalStrVal { get; set; }
        protected virtual string C3ProtStrVal { get; set; }
        private protected virtual string C3PrvProtStrVal { get; set; }
        private string C3PrvStrVal { get; set; }

        [Attr3]
        public virtual int C3PubIntVal { get; set; }

        public event Action<short> C3Event
        {
            add => c3Event += value;
            remove => c3Event -= value;
        }

        protected event Action<short> C3ProtEvent;
        private event Action<short> C3PrvEvent;

        public static string GetC3PubStaticStrVal() => default;
        internal static string GetC3InternalStaticStrVal() => default;
        protected internal static string GetC3ProtInternalStaticStrVal() => default;
        protected static string GetC3ProtStaticStrVal() => default;
        private protected static string GetC3PrvProtStaticStrVal() => default;
        private static string GetC3PrvStaticStrVal() => default;

        public virtual string GetC3PubStrVal() => default;
        internal virtual string GetC3InternalStrVal() => default;
        protected internal virtual string GetC3ProtInternalStrVal() => default;
        protected virtual string GetC3ProtStrVal() => default;
        private protected virtual string GetC3PrvProtStrVal() => default;
        private string GetC3PrvStrVal() => default;

        [Attr3]
        public virtual int GetC3PubIntVal() => default;
    }

    public static class Sttc1
    {
        public const long C3_PUB_CONST_LONG_VAL = 1;
        internal const long C3_INTERNAL_CONST_LONG_VAL = 2;
        private const long C3_PRV_CONST_LONG_VAL = 6;

        public static readonly long C3PubStaticReadonlyLongVal = 7;
        internal static readonly long C3InternalStaticReadonlyLongVal = 8;
        private static readonly long C3PrvStaticReadonlyLongVal = 12;

        public static long C3PubStaticLongVal = 13;
        internal static long C3InternalStaticLongVal = 14;
        private static long C3PrvStaticLongVal = 18;

        static Sttc1()
        {
        }

        public static string C3PubStaticStrVal { get; set; }
        internal static string C3InternalStaticStrVal { get; set; }
        private static string C3PrvStaticStrVal { get; set; }

        public static string GetC3PubStaticStrVal() => default;
        internal static string GetC3InternalStaticStrVal() => default;
        private static string GetC3PrvStaticStrVal() => default;
    }
}

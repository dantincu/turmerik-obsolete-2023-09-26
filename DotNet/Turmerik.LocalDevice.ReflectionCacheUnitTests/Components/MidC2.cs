using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests.Components
{
    [MidCAttr2]
    internal class MidC2<T> : BaseC1<T, string>, IC2<T>
    {
        public const long C2_PUB_CONST_LONG_VAL = 1;
        internal const long C2_INTERNAL_CONST_LONG_VAL = 2;
        protected internal const long C2_PROT_INTERNAL_CONST_LONG_VAL = 3;
        protected const long C2_PROT_CONST_LONG_VAL = 4;
        private protected const long C2_PRIV_PROT_CONST_LONG_VAL = 5;
        private const long C2_PRV_CONST_LONG_VAL = 6;

        public static readonly long C2PubStaticReadonlyLongVal = 7;
        internal static readonly long C2InternalStaticReadonlyLongVal = 8;
        protected internal static readonly long C2ProtInternalStaticReadonlyLongVal = 9;
        protected static readonly long C2ProtStaticReadonlyLongVal = 10;
        private protected static readonly long C2PrvProtStaticReadonlyLongVal = 11;
        private static readonly long C2PrvStaticReadonlyLongVal = 12;

        public static long C2PubStaticLongVal = 13;
        internal static long C2InternalStaticLongVal = 14;
        protected internal static long C2ProtInternalStaticLongVal = 15;
        protected static long C2ProtStaticLongVal = 16;
        private protected static long C2PrvProtStaticLongVal = 17;
        private static long C2PrvStaticLongVal = 18;

        public readonly long C2PubReadonlyLongVal = 19;
        internal readonly long C2InternalReadonlyLongVal = 20;
        protected internal readonly long C2ProtInternalReadonlyLongVal = 21;
        protected readonly long C2ProtReadonlyLongVal = 22;
        private protected readonly long C2PrvProtReadonlyLongVal = 23;
        private readonly long C2PrvReadonlyLongVal = 24;

        public long C2PubLongVal = 19;
        internal long C2InternalLongVal = 20;
        protected internal long C2ProtInternalLongVal = 21;
        protected long C2ProtLongVal = 22;
        private protected long C2PrvProtLongVal = 23;
        private long C2PrvLongVal = 24;

        public MidC2(string pubStrVal)
        {
        }

        [MidAttr2]
        protected MidC2()
        {
        }

        private MidC2(int pubIntVal)
        {
        }

        public static string C2PubStaticStrVal { get; set; }
        internal static string C2InternalStaticStrVal { get; set; }
        protected internal static string C2ProtInternalStaticStrVal { get; set; }
        protected static string C2ProtStaticStrVal { get; set; }
        private protected static string C2PrvProtStaticStrVal { get; set; }
        private static string C2PrvStaticStrVal { get; set; }

        public virtual string C2PubStrVal { get; set; }
        internal virtual string C2InternalStrVal { get; set; }
        protected internal virtual string C2ProtInternalStrVal { get; set; }
        protected virtual string C2ProtStrVal { get; set; }
        private protected virtual string C2PrvProtStrVal { get; set; }
        private string C2PrvStrVal { get; set; }

        [MidAttr2]
        public virtual int C2PubIntVal { get; set; }

        public event Action<short> C2Event;
        protected event Action<short> C2ProtEvent;
        private event Action<short> C2PrvEvent;

        public static string GetC2PubStaticStrVal() => default;
        internal static string GetC2InternalStaticStrVal() => default;
        protected internal static string GetC2ProtInternalStaticStrVal() => default;
        protected static string GetC2ProtStaticStrVal() => default;
        private protected static string GetC2PrvProtStaticStrVal() => default;
        private static string GetC2PrvStaticStrVal() => default;

        public virtual string GetC2PubStrVal() => default;
        internal virtual string GetC2InternalStrVal() => default;
        protected internal virtual string GetC2ProtInternalStrVal() => default;
        protected virtual string GetC2ProtStrVal() => default;
        private protected virtual string GetC2PrvProtStrVal() => default;
        private string GetC2PrvStrVal() => default;

        [MidAttr2]
        public virtual T GetC2PubIntVal() => default;
    }
}

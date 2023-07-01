using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests.Components
{
    public class BaseAttr1 : Attribute
    {
    }

    public class MidAttr2 : BaseAttr1
    {
    }

    public class Attr3 : MidAttr2
    {
    }

    public class BaseIAttr1 : Attribute
    {
    }

    public class MidIAttr2 : BaseCAttr1
    {
    }

    public class IAttr3 : MidCAttr2
    {
    }

    public class BaseCAttr1 : Attribute
    {
    }

    public class MidCAttr2 : BaseCAttr1
    {
    }

    public class CAttr3 : MidCAttr2
    {
    }

    [BaseIAttr1]
    public interface IC1<T1, T2>
    {
        [BaseAttr1]
        T2 C1PubStrVal { get; }
        T1 C1PubIntVal { get; }

        event Action<short> C1Event;
    }

    [MidIAttr2]
    public interface IC2<T> : IC1<T, string>
    {
        [MidAttr2]
        string C2PubStrVal { get; }
        int C2PubIntVal { get; }

        event Action<short> C2Event;
    }

    [IAttr3]
    public interface IC3 : IC2<int>
    {
        [Attr3]
        string C3PubStrVal { get; }
        int C3PubIntVal { get; }

        event Action<short> C3Event;
    }

    [BaseCAttr1]
    public class BaseC1<T1, T2> : IC1<T1, T2>
    {
        public const long C1_PUB_CONST_LONG_VAL = 1;
        internal const long C1_INTERNAL_CONST_LONG_VAL = 2;
        protected internal const long C1_PROT_INTERNAL_CONST_LONG_VAL = 3;
        protected const long C1_PROT_CONST_LONG_VAL = 4;
        private protected const long C1_PRIV_PROT_CONST_LONG_VAL = 5;
        private const long C1_PRV_CONST_LONG_VAL = 6;

        public static readonly long C1PubStaticReadonlyLongVal = 7;
        internal static readonly long C1InternalStaticReadonlyLongVal = 8;
        protected internal static readonly long C1ProtInternalStaticReadonlyLongVal = 9;
        protected static readonly long C1ProtStaticReadonlyLongVal = 10;
        private protected static readonly long C1PrvProtStaticReadonlyLongVal = 11;
        private static readonly long C1PrvStaticReadonlyLongVal = 12;

        public static long C1PubStaticLongVal = 13;
        internal static long C1InternalStaticLongVal = 14;
        protected internal static long C1ProtInternalStaticLongVal = 15;
        protected static long C1ProtStaticLongVal = 16;
        private protected static long C1PrvProtStaticLongVal = 17;
        private static long C1PrvStaticLongVal = 18;

        public readonly long C1PubReadonlyLongVal = 19;
        internal readonly long C1InternalReadonlyLongVal = 20;
        protected internal readonly long C1ProtInternalReadonlyLongVal = 21;
        protected readonly long C1ProtReadonlyLongVal = 22;
        private protected readonly long C1PrvProtReadonlyLongVal = 23;
        private readonly long C1PrvReadonlyLongVal = 24;

        public long C1PubLongVal = 19;
        internal long C1InternalLongVal = 20;
        protected internal long C1ProtInternalLongVal = 21;
        protected long C1ProtLongVal = 22;
        private protected long C1PrvProtLongVal = 23;
        private long C1PrvLongVal = 24;

        public BaseC1(string pubStrVal)
        {
        }

        [BaseAttr1]
        protected BaseC1()
        {
        }

        private BaseC1(int pubIntVal)
        {
        }

        public static string C1PubStaticStrVal { get; set; }
        internal static string C1InternalStaticStrVal { get; set; }
        protected internal static string C1ProtInternalStaticStrVal { get; set; }
        protected static string C1ProtStaticStrVal { get; set; }
        private protected static string C1PrvProtStaticStrVal { get; set; }
        private static string C1PrvStaticStrVal { get; set; }

        public virtual T2 C1PubStrVal { get; set; }
        internal virtual string C1InternalStrVal { get; set; }
        protected internal virtual string C1ProtInternalStrVal { get; set; }
        protected virtual string C1ProtStrVal { get; set; }
        private protected virtual string C1PrvProtStrVal { get; set; }
        private string C1PrvStrVal { get; set; }

        [BaseAttr1]
        public virtual T1 C1PubIntVal { get; set; }

        public event Action<short> C1Event;
        protected event Action<short> C1ProtEvent;
        private event Action<short> C1PrvEvent;

        public static string GetC1PubStaticStrVal() => default;
        internal static string GetC1InternalStaticStrVal() => default;
        protected internal static string GetC1ProtInternalStaticStrVal() => default;
        protected static string GetC1ProtStaticStrVal() => default;
        private protected static string GetC1PrvProtStaticStrVal() => default;
        private static string GetC1PrvStaticStrVal() => default;

        public virtual string GetC1PubStrVal() => default;
        internal virtual string GetC1InternalStrVal() => default;
        protected internal virtual string GetC1ProtInternalStrVal() => default;
        protected virtual string GetC1ProtStrVal() => default;
        private protected virtual string GetC1PrvProtStrVal() => default;
        private string GetC1PrvStrVal() => default;

        [BaseAttr1]
        public virtual int GetC1PubIntVal() => default;
    }

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
        public virtual int GetC2PubIntVal() => default;
    }

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

        public event Action<short> C3Event;
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
}

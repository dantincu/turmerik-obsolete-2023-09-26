using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Reflection;
using Turmerik.Testing;
using Turmerik.Utils;

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

        T2 GetC1PubStrVal();
        T1 GetC1PubIntVal();
    }

    [MidIAttr2]
    public interface IC2<T> : IC1<T, string>
    {
        [MidAttr2]
        string C2PubStrVal { get; }
        int C2PubIntVal { get; }

        event Action<short> C2Event;

        string GetC2PubStrVal();
        T GetC2PubIntVal();
    }

    [IAttr3]
    public interface IC3 : IC2<int>
    {
        [Attr3]
        string C3PubStrVal { get; }
        int C3PubIntVal { get; }

        event Action<short> C3Event;

        string GetC3PubStrVal();
        int GetC3PubIntVal();
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

        public static readonly ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> OwnEventsTestData = new ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>>(
            new Dictionary<EventAccessibilityFilter, string[]>
            {
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Public | MemberVisibility.Protected,
                        MemberVisibility.Public | MemberVisibility.Private,
                        MemberVisibility.None),
                    nameof(C1Event).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Public | MemberVisibility.Protected,
                        MemberVisibility.Public | MemberVisibility.Protected,
                        MemberVisibility.None),
                    nameof(C1Event).Arr(nameof(C1ProtEvent))
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Protected,
                        MemberVisibility.Protected,
                        MemberVisibility.None),
                    nameof(C1ProtEvent).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Protected | MemberVisibility.Private,
                        MemberVisibility.Protected,
                        MemberVisibility.None),
                    nameof(C1ProtEvent).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Protected | MemberVisibility.Private,
                        MemberVisibility.Protected | MemberVisibility.Private,
                        MemberVisibility.None),
                    nameof(C1ProtEvent).Arr(nameof(C1PrvEvent))
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Private,
                        MemberVisibility.Private,
                        MemberVisibility.None),
                    nameof(C1PrvEvent).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.None,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    new string[0]
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Public,
                        MemberVisibility.Public,
                        MemberVisibility.None),
                    nameof(IC1<int, string>.C1Event).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Private,
                        MemberVisibility.Public | MemberVisibility.Protected,
                        MemberVisibility.None),
                    new string[0]
                }
            }.RdnlD(),
            new Dictionary<EventAccessibilityFilter, string[]>
            {
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Public,
                        MemberVisibility.Public,
                        MemberVisibility.Public),
                    nameof(IC1<int, string>.C1Event).Arr()
                },
                {
                    new EventAccessibilityFilter(
                        MemberVisibility.Private,
                        MemberVisibility.Public | MemberVisibility.Protected,
                        MemberVisibility.Public | MemberVisibility.Private),
                    new string[0]
                }
            }.RdnlD());

        public static readonly ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> AllVisibleEventsTestData = OwnEventsTestData.ExceptPrivate();
        public static readonly ExpectedContents<IDictionary<EventAccessibilityFilter, string[]>> AsmVisibleEventsTestData = OwnEventsTestData.ExceptPrivate();

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

        public virtual T2 GetC1PubStrVal() => default;
        internal virtual string GetC1InternalStrVal() => default;
        protected internal virtual string GetC1ProtInternalStrVal() => default;
        protected virtual string GetC1ProtStrVal() => default;
        private protected virtual string GetC1PrvProtStrVal() => default;
        private string GetC1PrvStrVal() => default;

        [BaseAttr1]
        public virtual T1 GetC1PubIntVal() => default;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Turmerik.LocalDevice.ReflectionCacheUnitTests.Components;
using Turmerik.Reflection;
using Turmerik.Reflection.Cache;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class MainUnitTest : UnitTestBase
    {
        private readonly IEqualityComparer<IDictionary<string, Type>> methodParamsDictnrEqCompr;
        private readonly IEqualityComparer<IDictionary<string, Type>[]> methodParamsDictnrArrEqCompr;
        private readonly IEqualityComparer<IDictionary<MemberVisibility, IDictionary<string, Type>[]>> constructorsDictnrArrEqCompr;

        private readonly IEqualityComparer<IDictionary<MemberVisibility, string[]>> memberNamesDictnrEqCompr;
        private readonly IEqualityComparer<IDictionary<EventAccessibilityFilter, string[]>> eventNamesDictnrEqCompr;
        private readonly IEqualityComparer<IDictionary<MethodAccessibilityFilter, string[]>> methodNamesDictnrEqCompr;
        private readonly IEqualityComparer<IDictionary<PropertyAccessibilityFilter, string[]>> propNamesDictnrEqCompr;
        private readonly IEqualityComparer<IDictionary<FieldAccessibilityFilter, string[]>> fieldNamesDictnrEqCompr;

        public MainUnitTest()
        {
            methodParamsDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparerCore<string, Type>();

            methodParamsDictnrArrEqCompr = BasicEqComprFactory.GetArrayBasicEqualityComparer<IDictionary<string, Type>>(
                methodParamsDictnrEqCompr.Equals);

            constructorsDictnrArrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparerCore<MemberVisibility, IDictionary<string, Type>[]>(
                methodParamsDictnrArrEqCompr.Equals);

            memberNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparerCore<MemberVisibility, string[]>(
                StringArrEqCompr.Equals);

            eventNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparerCore<EventAccessibilityFilter, string[]>(
                StringArrEqCompr.Equals,
                EventAccessFilterEqCompr.Equals);

            methodNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparerCore<MethodAccessibilityFilter, string[]>(
                StringArrEqCompr.Equals,
                MethodAccessFilterEqCompr.Equals);

            propNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparerCore<PropertyAccessibilityFilter, string[]>(
                StringArrEqCompr.Equals,
                PropAccessFilterEqCompr.Equals);

            fieldNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparerCore<FieldAccessibilityFilter, string[]>(
                StringArrEqCompr.Equals,
                FieldAccessFilterEqCompr.Equals);
        }
    }
}
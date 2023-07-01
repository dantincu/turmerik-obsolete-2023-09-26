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
        private readonly IEqualityComparer<Dictionary<string, Type>> methodParamsDictnrEqCompr;
        private readonly IEqualityComparer<Dictionary<string, Type>[]> methodParamsDictnrArrEqCompr;
        private readonly IEqualityComparer<Dictionary<MemberVisibility, Dictionary<string, Type>[]>> constructorsDictnrArrEqCompr;

        private readonly IEqualityComparer<Dictionary<MemberVisibility, string[]>> memberNamesDictnrEqCompr;
        private readonly IEqualityComparer<Dictionary<EventAccessibilityFilter, string[]>> eventNamesDictnrEqCompr;
        private readonly IEqualityComparer<Dictionary<MethodAccessibilityFilter, string[]>> methodNamesDictnrEqCompr;
        private readonly IEqualityComparer<Dictionary<PropertyAccessibilityFilter, string[]>> propNamesDictnrEqCompr;
        private readonly IEqualityComparer<Dictionary<FieldAccessibilityFilter, string[]>> fieldNamesDictnrEqCompr;

        public MainUnitTest()
        {
            methodParamsDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparer<string, Type>();

            methodParamsDictnrArrEqCompr = BasicEqComprFactory.GetArrayBasicEqualityComparer<Dictionary<string, Type>>(
                methodParamsDictnrEqCompr.Equals);

            constructorsDictnrArrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparer<MemberVisibility, Dictionary<string, Type>[]>(
                methodParamsDictnrArrEqCompr.Equals);

            memberNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparer<MemberVisibility, string[]>(
                StringArrEqCompr.Equals);

            eventNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparer<EventAccessibilityFilter, string[]>(
                StringArrEqCompr.Equals,
                EventAccessFilterEqCompr.Equals);

            methodNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparer<MethodAccessibilityFilter, string[]>(
                StringArrEqCompr.Equals,
                MethodAccessFilterEqCompr.Equals);

            propNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparer<PropertyAccessibilityFilter, string[]>(
                StringArrEqCompr.Equals,
                PropAccessFilterEqCompr.Equals);

            fieldNamesDictnrEqCompr = BasicEqComprFactory.GetDictionaryBasicEqualityComparer<FieldAccessibilityFilter, string[]>(
                StringArrEqCompr.Equals,
                FieldAccessFilterEqCompr.Equals);
        }

        [Fact]
        public void MainTest()
        {

        }
    }
}
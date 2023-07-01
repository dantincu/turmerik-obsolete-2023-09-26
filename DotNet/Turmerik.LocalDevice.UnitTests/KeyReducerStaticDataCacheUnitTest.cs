using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cache;
using Turmerik.Collections;
using Turmerik.MathH;

namespace Turmerik.LocalDevice.UnitTests
{
    public class KeyReducerStaticDataCacheUnitTest : UnitTestBase
    {
        private static readonly ReadOnlyCollection<TestKey> allDefinedTestKeys;
        private static readonly ReadOnlyCollection<TestKey> allDefinedTestKeysExceptLast;

        private readonly INonSynchronizedStaticDataCacheFactory staticDataCacheFactory;

        static KeyReducerStaticDataCacheUnitTest()
        {
            allDefinedTestKeys = Enum.GetValues<TestKey>().RdnlC();

            allDefinedTestKeysExceptLast = allDefinedTestKeys.Except(
                TestKey.P16.Arr()).RdnlC();
        }

        public KeyReducerStaticDataCacheUnitTest()
        {
            staticDataCacheFactory = ServiceProvider.GetRequiredService<INonSynchronizedStaticDataCacheFactory>();
        }

        [Fact]
        public void MainTest()
        {
            PerformTest<TestKey, string>(
                keyReducer =>
                {
                    PerformTestAssertions(
                        keyReducer,
                        allDefinedTestKeysExceptLast.ToDictionary(
                            value => value,
                            value => value.ToString()),
                        allDefinedTestKeysExceptLast.ToDictionary(
                                value => Tuple.Create(TestKey.P16, value).ReduceEnums(
                                    tuple => tuple.Item1 + tuple.Item2),
                                value => value.ToString()));
                },
            factory: key => key.ToString(),
            createKeyReducer: ReduceKey);
        }

        private void PerformTest<TKey, TValue>(
            Action<IKeyReducerStaticDataCache<TKey, TValue>> assertAction,
            Func<TKey, TValue> factory,
            IEqualityComparer<TKey> keyEqCompr = null,
            Func<TKey, TKey> createKeyReducer = null,
            Func<TKey, TKey> removeKeyReducer = null,
            Func<TKey, TKey> containsKeyReducer = null)
        {
            var keyReducer = staticDataCacheFactory.CreateKeyReducer(
                factory,
                keyEqCompr,
                createKeyReducer,
                removeKeyReducer,
                containsKeyReducer);

            assertAction(keyReducer);
        }

        private void PerformTestAssertions<TKey, TValue>(
            IKeyReducerStaticDataCache<TKey, TValue> keyReducer,
            Dictionary<TKey, TValue> expectedData,
            Dictionary<TKey, TValue> expectedReducedData = null)
        {
            foreach (var kvp in expectedData)
            {
                keyReducer.Get(kvp.Key);
            }

            if (expectedReducedData != null)
            {
                foreach (var kvp in expectedReducedData)
                {
                    keyReducer.Get(kvp.Key);
                }
            }

            this.AssertSequenceEqual(
                expectedData.Keys,
                keyReducer.GetKeys());

            PerformTestAssertionsCore(
                keyReducer,
                expectedData);

            if (expectedReducedData != null)
            {
                PerformTestAssertionsCore(
                    keyReducer,
                    expectedReducedData);
            }
        }

        private void PerformTestAssertionsCore<TKey, TValue>(
            IKeyReducerStaticDataCache<TKey, TValue> keyReducer,
            Dictionary<TKey, TValue> expectedData)
        {
            foreach (var key in expectedData.Keys)
            {
                bool hasKey = keyReducer.ContainsKey(key);
                Assert.True(hasKey);

                var expectedValue = expectedData[key];
                var actualValue = keyReducer.Get(key);

                Assert.Equal(
                    expectedValue,
                    actualValue);
            }
        }

        private TestKey ReduceKey(TestKey testKey) => Tuple.Create(
            testKey,
            TestKey.P16).ReduceEnums(
                tuple => tuple.Item1 % tuple.Item2);

        private enum TestKey
        {
            None = 0,
            P0 = 1,
            P1 = 2,
            P2 = 4,
            P3 = 8,
            P4 = 16,
            P5 = 32,
            P6 = 64,
            P7 = 128,
            P8 = 256,
            P9 = 512,
            P10 = 1024,
            P12 = 2048,
            P13 = 4096,
            P14 = 8192,
            P15 = 16384,
            P16 = 32768
        }
    }
}

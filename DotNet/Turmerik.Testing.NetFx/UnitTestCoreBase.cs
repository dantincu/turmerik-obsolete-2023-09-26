﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Utils;

namespace Turmerik.Testing.NetFx
{
    public class UnitTestCoreBase
    {
        public UnitTestCoreBase()
        {
        }

        protected virtual IServiceProvider ServiceProvider => ServiceProviderContainer.Instance.Value.Services;

        protected T AssertEqual<T>(Func<T> valueFactory, T expectedValue, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            T actualValue = valueFactory();

            Assert.AreEqual(expectedValue, actualValue, comparer);
            return actualValue;
        }

        protected T AssertTrue<T>(Func<T> valueFactory, Func<T, bool> validator)
        {
            T actualValue = valueFactory();
            bool isValid = validator(actualValue);

            Assert.IsTrue(isValid);
            return actualValue;
        }

        protected void AssertEqualCore<T>(T expected, T actual)
           where T : class
        {
            if (expected != null)
            {
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected, actual);
            }
            else
            {
                Assert.IsNull(actual);
            }
        }

        protected void AssertSequenceEqual<T>(
            IEnumerable<T> expectedSequence,
            IEnumerable<T> actualSequence,
            IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            bool expectedIsNull = expectedSequence == null;
            bool actualIsNull = actualSequence == null;

            Assert.AreEqual(expectedIsNull, actualIsNull);

            if (!(expectedIsNull || actualIsNull))
            {
                bool isValid = expectedSequence.SequenceEqual(actualSequence, comparer);
                Assert.IsTrue(isValid);
            }
        }

        protected void AssertSequenceEqual<T>(
            IEnumerable<T> expectedSequence,
            IEnumerable<T> actualSequence,
            IBasicEqualityComparerFactory comparerFactory,
            Func<T, T, bool> equalsFunc,
            bool valuesCanBeNull,
            Func<T, int> hashCodeFunc = null)
        {
            var comparer = comparerFactory.GetEqualityComparer(
                equalsFunc, valuesCanBeNull, hashCodeFunc);

            AssertSequenceEqual(
                expectedSequence,
                actualSequence,
                comparer);
        }

        protected void AssertValueSequenceEqual<T>(
            IEnumerable<T> expectedSequence,
            IEnumerable<T> actualSequence,
            IBasicEqualityComparerFactory comparerFactory,
            Func<T, T, bool> equalsFunc,
            Func<T, int> hashCodeFunc = null)
            where T : struct
        {
            var comparer = comparerFactory.GetEqualityComparer(
                equalsFunc, false, hashCodeFunc);

            AssertSequenceEqual(
                expectedSequence,
                actualSequence,
                comparer);
        }

        protected void AssertObjectSequenceEqual<T>(
            IEnumerable<T> expectedSequence,
            IEnumerable<T> actualSequence,
            IBasicEqualityComparerFactory comparerFactory,
            Func<T, T, bool> equalsFunc,
            Func<T, int> hashCodeFunc = null)
            where T : class
        {
            var comparer = comparerFactory.GetEqualityComparer(
                equalsFunc, true, hashCodeFunc);

            AssertSequenceEqual(
                expectedSequence,
                actualSequence,
                comparer);
        }

        protected static class StrC
        {
            public const string ASDF = "ASDF";
            public const string QWER = "QWER";
            public const string ZXCV = "ZXCV";
            public const string TYUI = "TYUI";
            public const string GHJK = "GHJK";
            public const string BNMLOP = "BNMLOP";

            public const string ASDF1 = "ASDF1";
            public const string QWER1 = "QWER1";
            public const string ZXCV1 = "ZXCV1";
            public const string TYUI1 = "TYUI1";
            public const string GHJK1 = "GHJK1";
            public const string BNMLOP1 = "BNMLOP1";
        }
    }
}

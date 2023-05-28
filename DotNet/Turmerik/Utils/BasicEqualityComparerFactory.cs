using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Utils
{
    public interface IBasicEqualityComparerFactory
    {
        SimpleEqualityComparer<T> GetEqualityComparer<T>(
            Func<T, T, bool> equalsFunc = null,
            bool valuesCanBeNull = false,
            Func<T, int> hashCodeFunc = null);

        ValueEqualityComparer<T> GetValueEqualityComparer<T>(
            Func<T, T, bool> equalsFunc = null,
            Func<T, int> hashCodeFunc = null)
            where T : struct;

        ObjectEqualityComparer<T> GetObjectEqualityComparer<T>(
            Func<T, T, bool> equalsFunc = null,
            Func<T, int> hashCodeFunc = null)
            where T : class;
    }

    public class BasicEqualityComparerFactory : IBasicEqualityComparerFactory
    {
        public SimpleEqualityComparer<T> GetEqualityComparer<T>(
            Func<T, T, bool> equalsFunc = null,
            bool valuesCanBeNull = false,
            Func<T, int> hashCodeFunc = null)
        {
            var comparer = new SimpleEqualityComparer<T>(
                equalsFunc, valuesCanBeNull, hashCodeFunc);

            return comparer;
        }

        public ObjectEqualityComparer<T> GetObjectEqualityComparer<T>(
            Func<T, T, bool> equalsFunc = null,
            Func<T, int> hashCodeFunc = null) where T : class
        {
            var comparer = new ObjectEqualityComparer<T>(
                equalsFunc, hashCodeFunc);

            return comparer;
        }

        public ValueEqualityComparer<T> GetValueEqualityComparer<T>(
            Func<T, T, bool> equalsFunc = null,
            Func<T, int> hashCodeFunc = null) where T : struct
        {
            var comparer = new ValueEqualityComparer<T>(
                equalsFunc, hashCodeFunc);

            return comparer;
        }
    }

    public class SimpleEqualityComparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> equalsFunc;
        private readonly bool valuesCanBeNull;
        private readonly Func<T, int> hashCodeFunc;

        public SimpleEqualityComparer(
            Func<T, T, bool> equalsFunc = null,
            bool valuesCanBeNull = false,
            Func<T, int> hashCodeFunc = null)
        {
            this.equalsFunc = equalsFunc.FirstNotNull((a, b) => a?.Equals(b) ?? b == null);
            this.valuesCanBeNull = valuesCanBeNull;
            this.hashCodeFunc = hashCodeFunc?.FirstNotNull(val => val?.GetHashCode() ?? 0);
        }

        public override bool Equals(T x, T y)
        {
            bool retVal;

            if (valuesCanBeNull)
            {
                if (x != null && y != null)
                {
                    retVal = equalsFunc(x, y);
                }
                else
                {
                    retVal = x == null && y == null;
                }
            }
            else
            {
                retVal = equalsFunc(x, y);
            }

            return retVal;
        }

        public override int GetHashCode(T obj)
        {
            int hashCode;

            if (valuesCanBeNull)
            {
                if (obj != null)
                {
                    hashCode = hashCodeFunc(obj);
                }
                else
                {
                    hashCode = 0;
                }
            }
            else
            {
                hashCode = hashCodeFunc(obj);
            }

            return hashCode;
        }
    }

    public class ValueEqualityComparer<T> : SimpleEqualityComparer<T>
        where T : struct
    {
        public ValueEqualityComparer(
            Func<T, T, bool> equalsFunc,
            Func<T, int> hashCodeFunc = null) : base(equalsFunc, false, hashCodeFunc)
        {
        }
    }

    public class ObjectEqualityComparer<T> : SimpleEqualityComparer<T>
        where T : class
    {
        public ObjectEqualityComparer(
            Func<T, T, bool> equalsFunc,
            Func<T, int> hashCodeFunc = null) : base(equalsFunc, true, hashCodeFunc)
        {
        }
    }
}

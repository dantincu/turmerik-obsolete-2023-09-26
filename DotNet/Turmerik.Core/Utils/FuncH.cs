using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.Collections;

namespace Turmerik.Core.Utils
{
    public static class FuncH
    {
        public static T FirstNotNull<T>(this T first, params T[] next)
        {
            T retVal = first;

            if (retVal == null)
            {
                retVal = next.FirstOrDefault(
                    item => item != null);
            }

            return retVal;
        }

        public static TOut WithValue<TIn, TOut>(
            this TIn inVal,
            Func<TIn, TOut> callback)
        {
            TOut outVal = callback(inVal);
            return outVal;
        }

        public static TVal ActWithValue<TVal>(
            this TVal value,
            Action<TVal>? callback)
        {
            callback?.Invoke(value);
            return value;
        }

        public static TVal SafeCast<TVal>(
            this object src,
            Func<TVal>? dfValFactory = null)
        {
            TVal retVal;

            if (src != null && src is TVal val)
            {
                retVal = val;
            }
            else if (dfValFactory != null)
            {
                retVal = dfValFactory();
            }
            else
            {
                retVal = default;
            }

            return retVal;
        }

        public static bool IfTrue(
            this bool condition,
            Action mainAction,
            Action? elseAction = null)
        {
            if (condition)
            {
                mainAction();
            }
            else if (elseAction != null)
            {
                elseAction();
            }

            return condition;
        }

        public static TVal IfTrue<TVal>(
            this bool condition,
            Func<TVal> mainValFactory,
            Func<TVal>? defaultValFactory = null)
        {
            TVal val;

            if (condition)
            {
                val = mainValFactory();
            }
            else if (defaultValFactory != null)
            {
                val = defaultValFactory();
            }
            else
            {
                val = default;
            }

            return val;
        }

        public static T CreateInstance<T>(this object src, Type type = null, params object[] nextArgs)
        {
            object[] argsArr = src.Arr(nextArgs);
            object retObj = Activator.CreateInstance(type ?? typeof(T), argsArr);

            return (T)retObj;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Turmerik.Collections;

namespace Turmerik.Utils
{
    public static class FuncH
    {
        public static TOut Switch<TIn, TOut>(
            this TIn inVal,
            IDictionary<TIn, Func<TIn, TOut>> actionsMap,
            Func<TIn, TOut> defaultAction = null,
            IEqualityComparer<TIn> inValEqCompr = null) => (
                inValEqCompr = inValEqCompr ?? EqualityComparer<TIn>.Default).WithValue(
                    eqCompr => inVal.Switch(
                    actionsMap.ToDictnr<TIn, Func<TIn, TOut>, Func<TIn, bool>, Func<TIn, TOut>>(
                        key => val => eqCompr.Equals(key, val),
                        value => value),
                    defaultAction));

        public static TOut Switch<TIn, TOut>(
            this TIn inVal,
            IDictionary<Func<TIn, bool>, Func<TIn, TOut>> actionsMap,
            Func<TIn, TOut> defaultAction = null)
        {
            TOut retVal = default;
            bool foundMatch = false;

            foreach (var kvp in actionsMap)
            {
                if (kvp.Key(inVal))
                {
                    foundMatch = true;
                    retVal = kvp.Value(inVal);
                    break;
                }
            }

            if (!foundMatch && defaultAction != null)
            {
                retVal = defaultAction(inVal);
            }

            return retVal;
        }

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
            Action<TVal> callback)
        {
            callback?.Invoke(value);
            return value;
        }

        public static TVal SafeCast<TVal>(
            this object src,
            Func<TVal> dfValFactory = null)
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
            Action elseAction = null)
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
            Func<TVal> defaultValFactory = null)
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

        public static bool ActIfTrue(
            this bool condition,
            Action mainAction,
            Action defaultAction = null)
        {
            if (condition)
            {
                mainAction();
            }
            else if (defaultAction != null)
            {
                defaultAction();
            }

            return condition;
        }

        public static TOut IfNotNull<TIn, TOut>(
            this TIn inVal,
            Func<TIn, TOut> mainValueFactory,
            Func<TIn, TOut> defaultValueFactory = null) => (inVal != null).IfTrue(
                () => mainValueFactory(inVal),
                () => (defaultValueFactory.FirstNotNull(
                    val => default)).Invoke(inVal));

        public static TOut IfNotDefault<TIn, TOut>(
            this TIn inVal,
            Func<TIn, TOut> mainValueFactory,
            Func<TIn, TOut> defaultValueFactory = null,
            IEqualityComparer<TIn> inValEqCompr = null) => (
                inValEqCompr ?? EqualityComparer<TIn>.Default).WithValue(
                    eqCompr => eqCompr.Equals(inVal, default).IfTrue(
                        () => mainValueFactory(inVal),
                        () => (defaultValueFactory.FirstNotNull(
                            val => default))(inVal)));

        public static TVal ActIfNotNull<TVal>(
            this TVal value,
            Action<TVal> mainAction,
            Action<TVal> defaultAction = null) => IfNotNull(
                value,
                val =>
                {
                    mainAction?.Invoke(val);
                    return val;
                },
                val =>
                {
                    defaultAction?.Invoke(val);
                    return val;
                });

        public static TVal ActIfNotDefault<TVal>(
            this TVal value,
            Action<TVal> mainAction,
            Action<TVal> defaultAction = null) => IfNotDefault(
                value,
                val =>
                {
                    mainAction?.Invoke(val);
                    return val;
                },
                val =>
                {
                    defaultAction?.Invoke(val);
                    return val;
                });

        public static T CreateInstance<T>(this object src, Type type = null, params object[] nextArgs)
        {
            object[] argsArr = src.Arr(nextArgs);
            object retObj = Activator.CreateInstance(type ?? typeof(T), argsArr);

            return (T)retObj;
        }
    }
}

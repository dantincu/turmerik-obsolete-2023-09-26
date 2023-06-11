﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;

namespace Turmerik.Utils
{
    public static class FlagsH
    {
        public static bool IfHasFlag<TData, TFlag>(
            this TData data,
            TFlag actualFlag,
            TFlag expectedFlag,
            Func<TData, TFlag, bool> ifHasFlagCallback,
            Func<TData, TFlag, bool> ifDoesNotHaveFlagCallback)
            where TFlag : struct, Enum
        {
            bool matches;
            bool hasFlag = actualFlag.HasFlag(expectedFlag);

            if (hasFlag)
            {
                matches = ifHasFlagCallback(data, actualFlag);
            }
            else if (ifDoesNotHaveFlagCallback != null)
            {
                matches = ifDoesNotHaveFlagCallback(data, actualFlag);
            }
            else
            {
                throw new ArgumentNullException(
                    nameof(ifDoesNotHaveFlagCallback));
            }

            return matches;
        }

        public static bool IfHasFlag<TData, TFlag>(
            this TData data,
            TFlag actualFlag,
            TFlag expectedFlag,
            Func<TData, TFlag, bool> ifHasFlagCallback,
            bool ifDoesNotHaveFlagRetValue)
            where TFlag : struct, Enum => IfHasFlag(
                data,
                actualFlag,
                expectedFlag,
                ifHasFlagCallback,
                (obj, flag) => ifDoesNotHaveFlagRetValue);

        public static bool IfHasAnyFlag<TData, TFlag>(
            this TData data,
            TFlag actualFlag,
            IDictionary<TFlag, Func<TData, TFlag, bool>> flagCallbacksMap,
            bool defaultRetValue)
            where TFlag : struct, Enum
        {
            bool matches = defaultRetValue;

            foreach (var kvp in flagCallbacksMap)
            {
                if (actualFlag.HasFlag(kvp.Key))
                {
                    matches = kvp.Value(
                        data, actualFlag);

                    break;
                }
            }

            return matches;
        }
    }
}
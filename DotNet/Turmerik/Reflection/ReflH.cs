using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Text;

namespace Turmerik.Reflection
{
    public static class ReflH
    {
        public static bool IsReferenceType(
            this Type type) => !type.IsValueType;

        public static bool IsPrimitiveType(
            this Type type) => type.IsValueType || type == ReflC.Types.StringType;

        public static string? GetTypeFullDisplayName(this Type type)
        {
            string? typeFullName = type.FullName?.SubStr(
                (str, len) => str.FindVal((c, i) => c == '`').Key).Item1;

            return typeFullName;
        }

        public static bool PropAccessorIs(
            this PropertyInfo propInfo,
            Func<PropertyInfo, bool> hasAccessorFunc,
            Func<PropertyInfo, MethodInfo?> accessorFunc,
            Func<MethodInfo, bool> predicate,
            bool retTrueIfNotExtists = false)
        {
            bool retVal = hasAccessorFunc(propInfo);

            if (retVal)
            {
                MethodInfo accessor = accessorFunc(propInfo);
                retVal = predicate(accessor);
            }
            else if (retTrueIfNotExtists)
            {
                retVal = true;
            }

            return retVal;
        }

        public static bool PropGetterIs(
            this PropertyInfo propInfo,
            Func<MethodInfo, bool> predicate,
            bool retTrueIfNotExtists = false)
        {
            bool retVal = propInfo.PropAccessorIs(
                pi => pi.CanRead,
                pi => pi.GetMethod,
                predicate,
                retTrueIfNotExtists);

            return retVal;
        }

        public static bool PropSetterIs(
            this PropertyInfo propInfo,
            Func<MethodInfo, bool> predicate,
            bool retTrueIfNotExtists = false)
        {
            bool retVal = propInfo.PropAccessorIs(
                pi => pi.CanWrite,
                pi => pi.SetMethod,
                predicate,
                retTrueIfNotExtists);

            return retVal;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;

namespace Turmerik.Utils
{
    public static class EqComprH
    {
        /// <summary>
        /// Multiplies args with 13 and 17, respectively.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int BasicHashCode(
            this int a,
            int b) => 13 * a + 17 * b;

        /// <summary>
        /// Multiplies args with 13, 17 and 29, respectively.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int BasicHashCode(
            this int a,
            int b,
            int c) => 13 * a + 17 * b + 29 * c;

        /// <summary>
        /// Multiplies args with 13, 17, 29 and 47, respectively.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int BasicHashCode(
            this int a,
            int b,
            int c,
            int d) => 13 * a + 17 * b + 29 * c + 47 * d;

        /// <summary>
        /// Multiplies args with 13, 17, 29, 47 and 71, respectively.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int BasicHashCode(
            this int a,
            int b,
            int c,
            int d,
            int e) => 13 * a + 17 * b + 29 * c + 47 * d + 71 * e;

        /// <summary>
        /// Multiplies args with 13, 17, 29, 47, 71 and 97, respectively.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int BasicHashCode(
            this int a,
            int b,
            int c,
            int d,
            int e,
            int f) => 13 * a + 17 * b + 29 * c + 47 * d + 71 * e + 97 * f;
    }
}

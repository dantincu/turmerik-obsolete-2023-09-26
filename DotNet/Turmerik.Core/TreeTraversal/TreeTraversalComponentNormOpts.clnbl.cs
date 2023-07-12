using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.TreeTraversal
{
    public static class TreeTraversalComponentNormOpts
    {
        public interface IClnbl<T> : TreeTraversalComponentOptsCore.IClnbl<T>
        {
            bool DisposeTreeNodes { get; }
        }

        public class Immtbl<T> : TreeTraversalComponentOptsCore.Immtbl<T>, IClnbl<T>
        {
            public Immtbl(IClnbl<T> src) : base(src)
            {
                DisposeTreeNodes = src.DisposeTreeNodes;
            }

            public bool DisposeTreeNodes { get; }
        }

        public class Mtbl<T> : TreeTraversalComponentOptsCore.Mtbl<T>, IClnbl<T>
        {
            public Mtbl()
            {
            }

            public Mtbl(TreeTraversalComponentOptsCore.IClnbl<T> src) : base(src)
            {
            }

            public Mtbl(IClnbl<T> src) : base(src)
            {
                DisposeTreeNodes = src.DisposeTreeNodes;
            }

            public bool DisposeTreeNodes { get; set; }
        }

        public static Immtbl<T> ToImmtbl<T>(
            this IClnbl<T> src) => new Immtbl<T>(src);

        public static Immtbl<T> AsImmtbl<T>(
            this IClnbl<T> src) => (src as Immtbl<T>) ?? src?.ToImmtbl();

        public static Mtbl<T> ToMtbl<T>(
            this IClnbl<T> src) => new Mtbl<T>(src);

        public static Mtbl<T> AsMtbl<T>(
            this IClnbl<T> src) => (src as Mtbl<T>) ?? src?.ToMtbl();
    }
}

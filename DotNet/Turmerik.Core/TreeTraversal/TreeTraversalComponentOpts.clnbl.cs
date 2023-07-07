using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.TreeTraversal
{
    public static class TreeTraversalComponentOpts
    {
        public interface IClnbl<T>
        {
            Func<TreeTraversalComponent<T>.Args, T, IEnumerator<T>> ChildNodesNmrtrRetriever { get; }
            Func<TreeTraversalComponent<T>.Args, T, bool> GoNextPredicate { get; }
            T RootNode { get; }
            Action<TreeTraversalComponent<T>.Args, T> OnDescend { get; }
            Action<TreeTraversalComponent<T>.Args, T> OnAscend { get; }
            Action<TreeTraversalComponent<T>.Args> ArgsCreated { get; }
        }

        public class Immtbl<T> : IClnbl<T>
        {
            public Immtbl(IClnbl<T> src)
            {
                ChildNodesNmrtrRetriever = src.ChildNodesNmrtrRetriever;
                GoNextPredicate = src.GoNextPredicate;
                RootNode = src.RootNode;
                OnDescend = src.OnDescend;
                OnAscend = src.OnAscend;
                ArgsCreated = src.ArgsCreated;
            }

            public Func<TreeTraversalComponent<T>.Args, T, IEnumerator<T>> ChildNodesNmrtrRetriever { get; }
            public Func<TreeTraversalComponent<T>.Args, T, bool> GoNextPredicate { get; }
            public T RootNode { get; }
            public Action<TreeTraversalComponent<T>.Args, T> OnDescend { get; }
            public Action<TreeTraversalComponent<T>.Args, T> OnAscend { get; }
            public Action<TreeTraversalComponent<T>.Args> ArgsCreated { get; }
        }

        public class Mtbl<T> : IClnbl<T>
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl<T> src)
            {
                ChildNodesNmrtrRetriever = src.ChildNodesNmrtrRetriever;
                GoNextPredicate = src.GoNextPredicate;
                RootNode = src.RootNode;
                OnDescend = src.OnDescend;
                OnAscend = src.OnAscend;
                ArgsCreated = src.ArgsCreated;
            }

            public Func<TreeTraversalComponent<T>.Args, T, IEnumerator<T>> ChildNodesNmrtrRetriever { get; set; }
            public Func<TreeTraversalComponent<T>.Args, T, bool> GoNextPredicate { get; set; }
            public T RootNode { get; set; }
            public Action<TreeTraversalComponent<T>.Args, T> OnDescend { get; set; }
            public Action<TreeTraversalComponent<T>.Args, T> OnAscend { get; set; }
            public Action<TreeTraversalComponent<T>.Args> ArgsCreated { get; set; }
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

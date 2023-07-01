using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.MathH
{
    public interface IMatrixBuilder<T1, T2>
    {
        IEnumerable<Tuple<T1, T2>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            Func<Tuple<T1, T2>, bool> predicate);
    }

    public interface IMatrixBuilder<T1, T2, T3>
    {
        IEnumerable<Tuple<T1, T2, T3>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            Func<Tuple<T1, T2, T3>, bool> predicate);
    }

    public interface IMatrixBuilder<T1, T2, T3, T4>
    {
        IEnumerable<Tuple<T1, T2, T3, T4>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            Func<Tuple<T1, T2, T3, T4>, bool> predicate);
    }

    public interface IMatrixBuilder<T1, T2, T3, T4, T5>
    {
        IEnumerable<Tuple<T1, T2, T3, T4, T5>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            IEnumerable<T5> nmrbl5,
            Func<Tuple<T1, T2, T3, T4, T5>, bool> predicate);
    }

    public interface IMatrixBuilder<T1, T2, T3, T4, T5, T6>
    {
        IEnumerable<Tuple<T1, T2, T3, T4, T5, T6>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            IEnumerable<T5> nmrbl5,
            IEnumerable<T6> nmrbl6,
            Func<Tuple<T1, T2, T3, T4, T5, T6>, bool> predicate);
    }

    public interface IMatrixBuilder<T1, T2, T3, T4, T5, T6, T7>
    {
        IEnumerable<Tuple<T1, T2, T3, T4, T5, T6, T7>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            IEnumerable<T5> nmrbl5,
            IEnumerable<T6> nmrbl6,
            IEnumerable<T7> nmrbl7,
            Func<Tuple<T1, T2, T3, T4, T5, T6, T7>, bool> predicate);
    }

    public interface IMatrixBuilder<T1, T2, T3, T4, T5, T6, T7, TRest>
    {
        IEnumerable<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            IEnumerable<T5> nmrbl5,
            IEnumerable<T6> nmrbl6,
            IEnumerable<T7> nmrbl7,
            IEnumerable<TRest> nmrblRest,
            Func<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>, bool> predicate);
    }

    public interface IMatrixBuilderFactory
    {
        IMatrixBuilder<T1, T2> Create2D<T1, T2>();
        IMatrixBuilder<T1, T2, T3> Create3D<T1, T2, T3>();
        IMatrixBuilder<T1, T2, T3, T4> Create4D<T1, T2, T3, T4>();
        IMatrixBuilder<T1, T2, T3, T4, T5> Create5D<T1, T2, T3, T4, T5>();
        IMatrixBuilder<T1, T2, T3, T4, T5, T6> Create6D<T1, T2, T3, T4, T5, T6>();
        IMatrixBuilder<T1, T2, T3, T4, T5, T6, T7> Create7D<T1, T2, T3, T4, T5, T6, T7>();
        IMatrixBuilder<T1, T2, T3, T4, T5, T6, T7, TRest> Create8D<T1, T2, T3, T4, T5, T6, T7, TRest>();
    }

    public class MatrixBuilder<T1, T2> : IMatrixBuilder<T1, T2>
    {
        public IEnumerable<Tuple<T1, T2>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            Func<Tuple<T1, T2>, bool> predicate)
        {
            foreach (var item1 in nmrbl1)
            {
                foreach (var item2 in nmrbl2)
                {
                    var tuple = Tuple.Create(
                        item1,
                        item2);

                    if (predicate(tuple))
                    {
                        yield return tuple;
                    }
                }
            }
        }
    }

    public class MatrixBuilder<T1, T2, T3> : IMatrixBuilder<T1, T2, T3>
    {
        public IEnumerable<Tuple<T1, T2, T3>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            Func<Tuple<T1, T2, T3>, bool> predicate)
        {
            foreach (var item1 in nmrbl1)
            {
                foreach (var item2 in nmrbl2)
                {
                    foreach (var item3 in nmrbl3)
                    {
                        var tuple = Tuple.Create(
                            item1,
                            item2,
                            item3);

                        if (predicate(tuple))
                        {
                            yield return tuple;
                        }
                    }
                }
            }
        }
    }

    public class MatrixBuilder<T1, T2, T3, T4> : IMatrixBuilder<T1, T2, T3, T4>
    {
        public IEnumerable<Tuple<T1, T2, T3, T4>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            Func<Tuple<T1, T2, T3, T4>, bool> predicate)
        {
            foreach (var item1 in nmrbl1)
            {
                foreach (var item2 in nmrbl2)
                {
                    foreach (var item3 in nmrbl3)
                    {
                        foreach (var item4 in nmrbl4)
                        {
                            var tuple = Tuple.Create(
                                item1,
                                item2,
                                item3,
                                item4);

                            if (predicate(tuple))
                            {
                                yield return tuple;
                            }
                        }
                    }
                }
            }
        }
    }

    public class MatrixBuilder<T1, T2, T3, T4, T5> : IMatrixBuilder<T1, T2, T3, T4, T5>
    {
        public IEnumerable<Tuple<T1, T2, T3, T4, T5>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            IEnumerable<T5> nmrbl5,
            Func<Tuple<T1, T2, T3, T4, T5>, bool> predicate)
        {
            foreach (var item1 in nmrbl1)
            {
                foreach (var item2 in nmrbl2)
                {
                    foreach (var item3 in nmrbl3)
                    {
                        foreach (var item4 in nmrbl4)
                        {
                            foreach (var item5 in nmrbl5)
                            {
                                var tuple = Tuple.Create(
                                    item1,
                                    item2,
                                    item3,
                                    item4,
                                    item5);

                                if (predicate(tuple))
                                {
                                    yield return tuple;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class MatrixBuilder<T1, T2, T3, T4, T5, T6> : IMatrixBuilder<T1, T2, T3, T4, T5, T6>
    {
        public IEnumerable<Tuple<T1, T2, T3, T4, T5, T6>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            IEnumerable<T5> nmrbl5,
            IEnumerable<T6> nmrbl6,
            Func<Tuple<T1, T2, T3, T4, T5, T6>, bool> predicate)
        {
            foreach (var item1 in nmrbl1)
            {
                foreach (var item2 in nmrbl2)
                {
                    foreach (var item3 in nmrbl3)
                    {
                        foreach (var item4 in nmrbl4)
                        {
                            foreach (var item5 in nmrbl5)
                            {
                                foreach (var item6 in nmrbl6)
                                {
                                    var tuple = Tuple.Create(
                                        item1,
                                        item2,
                                        item3,
                                        item4,
                                        item5,
                                        item6);

                                    if (predicate(tuple))
                                    {
                                        yield return tuple;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class MatrixBuilder<T1, T2, T3, T4, T5, T6, T7> : IMatrixBuilder<T1, T2, T3, T4, T5, T6, T7>
    {
        public IEnumerable<Tuple<T1, T2, T3, T4, T5, T6, T7>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            IEnumerable<T5> nmrbl5,
            IEnumerable<T6> nmrbl6,
            IEnumerable<T7> nmrbl7,
            Func<Tuple<T1, T2, T3, T4, T5, T6, T7>, bool> predicate)
        {
            foreach (var item1 in nmrbl1)
            {
                foreach (var item2 in nmrbl2)
                {
                    foreach (var item3 in nmrbl3)
                    {
                        foreach (var item4 in nmrbl4)
                        {
                            foreach (var item5 in nmrbl5)
                            {
                                foreach (var item6 in nmrbl6)
                                {
                                    foreach (var item7 in nmrbl7)
                                    {
                                        var tuple = Tuple.Create(
                                            item1,
                                            item2,
                                            item3,
                                            item4,
                                            item5,
                                            item6,
                                            item7);

                                        if (predicate(tuple))
                                        {
                                            yield return tuple;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class MatrixBuilder<T1, T2, T3, T4, T5, T6, T7, TRest> : IMatrixBuilder<T1, T2, T3, T4, T5, T6, T7, TRest>
    {
        public IEnumerable<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>> Generate(
            IEnumerable<T1> nmrbl1,
            IEnumerable<T2> nmrbl2,
            IEnumerable<T3> nmrbl3,
            IEnumerable<T4> nmrbl4,
            IEnumerable<T5> nmrbl5,
            IEnumerable<T6> nmrbl6,
            IEnumerable<T7> nmrbl7,
            IEnumerable<TRest> nmrblRest,
            Func<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>, bool> predicate)
        {
            foreach (var item1 in nmrbl1)
            {
                foreach (var item2 in nmrbl2)
                {
                    foreach (var item3 in nmrbl3)
                    {
                        foreach (var item4 in nmrbl4)
                        {
                            foreach (var item5 in nmrbl5)
                            {
                                foreach (var item6 in nmrbl6)
                                {
                                    foreach (var item7 in nmrbl7)
                                    {
                                        foreach (var itemRest in nmrblRest)
                                        {
                                            var tuple = new Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(
                                                item1,
                                                item2,
                                                item3,
                                                item4,
                                                item5,
                                                item6,
                                                item7,
                                                itemRest);

                                            if (predicate(tuple))
                                            {
                                                yield return tuple;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class MatrixBuilderFactory : IMatrixBuilderFactory
    {
        public IMatrixBuilder<T1, T2> Create2D<T1, T2>(
            ) => new MatrixBuilder<T1, T2>();

        public IMatrixBuilder<T1, T2, T3> Create3D<T1, T2, T3>(
            ) => new MatrixBuilder<T1, T2, T3>();

        public IMatrixBuilder<T1, T2, T3, T4> Create4D<T1, T2, T3, T4>(
            ) => new MatrixBuilder<T1, T2, T3, T4>();

        public IMatrixBuilder<T1, T2, T3, T4, T5> Create5D<T1, T2, T3, T4, T5>(
            ) => new MatrixBuilder<T1, T2, T3, T4, T5>();

        public IMatrixBuilder<T1, T2, T3, T4, T5, T6> Create6D<T1, T2, T3, T4, T5, T6>(
            ) => new MatrixBuilder<T1, T2, T3, T4, T5, T6>();

        public IMatrixBuilder<T1, T2, T3, T4, T5, T6, T7> Create7D<T1, T2, T3, T4, T5, T6, T7>(
            ) => new MatrixBuilder<T1, T2, T3, T4, T5, T6, T7>();

        public IMatrixBuilder<T1, T2, T3, T4, T5, T6, T7, TRest> Create8D<T1, T2, T3, T4, T5, T6, T7, TRest>(
            ) => new MatrixBuilder<T1, T2, T3, T4, T5, T6, T7, TRest>();
    }
}

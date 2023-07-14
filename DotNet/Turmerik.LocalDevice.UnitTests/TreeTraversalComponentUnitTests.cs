using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.TreeTraversal;
using Turmerik.Utils;

namespace Turmerik.LocalDevice.UnitTests
{
    public class TreeTraversalComponentUnitTests : UnitTestBase
    {
        private readonly ITreeTraversalComponentFactory factory;

        public TreeTraversalComponentUnitTests()
        {
            factory = ServiceProvider.GetRequiredService<ITreeTraversalComponentFactory>();
        }

        [Fact]
        public void MainTest()
        {
            var component = factory.Create<int>();
            var mx = new List<Tuple<int, List<Tuple<int, List<Tuple<int, List<int>>>>>>>();
            int mxDim = 4;

            var mxL1 = new List<Tuple<int, List<Tuple<int, List<int>>>>>();
            var mxL2 = new List<Tuple<int, List<int>>>();
            var mxL3 = new List<int>();

            int sum = 0;

            component.Traverse(
                new TreeTraversalComponentOpts.Mtbl<int>
                {
                    RootNode = 0,
                    ChildNodesNmrtrRetriever = (args, val) => LazyH.Lazy(() => Enumerable.Range(0, mxDim).GetEnumerator()),
                    GoNextPredicate = (args, val) => args.CurrentTreeNode.CurrentLevel < mxDim,
                    OnDescend = (args, val) =>
                    {
                        switch (args.CurrentTreeNode.CurrentLevel)
                        {
                            case 1:
                                mxL1 = new List<Tuple<int, List<Tuple<int, List<int>>>>>();
                                mx.Add(Tuple.Create(val, mxL1));
                                break;
                            case 2:
                                mxL2 = new List<Tuple<int, List<int>>>();
                                mxL1.Add(Tuple.Create(val, mxL2));
                                break;
                            case 3:
                                mxL3 = new List<int>();
                                mxL2.Add(Tuple.Create(val, mxL3));
                                break;
                            case 4:
                                mxL3.Add(val);
                                break;
                        }
                    },
                    OnAscend = (args, val) =>
                    {
                        sum += val;
                    }
                });

            Assert.Equal(sum, 504);
            var exMxL3 = new List<int>() { 0, 1, 2, 3 };

            var exMxL2 = exMxL3.Select(
                (val, idx) => Tuple.Create(idx, exMxL3)).ToList();

            var exMxL1 = exMxL2.Select(
                (val, idx) => Tuple.Create(idx, exMxL2)).ToList();

            var exMx = exMxL1.Select(
                (val, idx) => Tuple.Create(idx, exMxL1)).ToList();

            Assert.Equal(exMx.Count, mx.Count);

            foreach (var acTL1 in mx)
            {
                var exTL1 = exMx[acTL1.Item1];
                Assert.Equal(exTL1.Item1, acTL1.Item1);

                Assert.Equal(exTL1.Item2.Count, acTL1.Item2.Count);

                foreach (var acTL2 in acTL1.Item2)
                {
                    var exTL2 = exTL1.Item2[acTL2.Item1];
                    Assert.Equal(exTL2.Item1, acTL2.Item1);

                    Assert.Equal(exTL2.Item2.Count, acTL2.Item2.Count);

                    foreach (var acTL3 in acTL2.Item2)
                    {
                        var exTL3 = exTL2.Item2[acTL3.Item1];
                        Assert.Equal(exTL3.Item1, acTL3.Item1);

                        Assert.Equal(exTL3.Item2.Count, acTL3.Item2.Count);

                        foreach (var acTL4 in acTL3.Item2)
                        {
                            var exTL4 = exTL3.Item2[acTL4];
                            Assert.Equal(exTL4, acTL4);
                        }
                    }
                }
            }
        }
    }
}

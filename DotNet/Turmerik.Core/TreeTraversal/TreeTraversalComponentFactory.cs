using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.TreeTraversal
{
    public interface ITreeTraversalComponentFactory
    {
        ITreeTraversalComponent<T> Create<T>();
    }

    public class TreeTraversalComponentFactory : ITreeTraversalComponentFactory
    {
        public ITreeTraversalComponent<T> Create<T>() => new TreeTraversalComponent<T>();
    }
}

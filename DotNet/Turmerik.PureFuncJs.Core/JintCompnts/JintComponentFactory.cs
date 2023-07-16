using Jint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public interface IJintComponentFactory
    {
        IJintComponent Create(string jsCode);

        IJintComponent<TBehaviour> Create<TBehaviour>(
            string jsCode,
            Func<Engine, ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>>, TBehaviour> behaviourFactory);
    }

    public class JintComponentFactory : IJintComponentFactory
    {
        public IJintComponent Create(string jsCode) => new JintComponent(jsCode);

        public IJintComponent<TBehaviour> Create<TBehaviour>(
            string jsCode,
            Func<Engine, ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>>, TBehaviour> behaviourFactory) => new JintComponent<TBehaviour>(
                jsCode, behaviourFactory);
    }
}

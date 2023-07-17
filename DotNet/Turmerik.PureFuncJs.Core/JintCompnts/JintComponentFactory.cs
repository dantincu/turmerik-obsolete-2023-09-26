using Jint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public interface IJintComponentFactory
    {
        IJintComponent Create(
            string jsCode);

        IJintComponent<TBehaviour> Create<TBehaviour>(
            string jsCode,
            Func<IJintComponent<TBehaviour>, ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>>, TBehaviour> behaviourFactory);
    }

    public class JintComponentFactory : IJintComponentFactory
    {
        private readonly IJintConsoleFactory consoleFactory;

        public JintComponentFactory(
            IJintConsoleFactory consoleFactory)
        {
            this.consoleFactory = consoleFactory ?? throw new ArgumentNullException(nameof(consoleFactory));
        }

        public IJintComponent Create(
            string jsCode) => new JintComponent(
                consoleFactory.Create(),
                jsCode);

        public IJintComponent<TBehaviour> Create<TBehaviour>(
            string jsCode,
            Func<IJintComponent<TBehaviour>, ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>>, TBehaviour> behaviourFactory) => new JintComponent<TBehaviour>(
                consoleFactory.Create(),
                jsCode,
                behaviourFactory);
    }
}

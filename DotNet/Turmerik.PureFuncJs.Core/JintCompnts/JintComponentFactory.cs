using Jint;
using Jint.Native.Object;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public interface IJintComponentFactory
    {
        IJintComponent Create(
            string jsCode,
            string cfgObjRetrieverCode,
            bool includeConsoleObj = true);

        IJintComponent Create(
            string jsCode,
            string cfgObjRetrieverCode,
            IJintConsole jintConsole);

        IJintComponent<TCfg> Create<TCfg>(
            string jsCode,
            string cfgObjRetrieverCode,
            bool includeConsoleObj = true,
            Func<IJintComponent<TCfg>, ObjectInstance, TCfg> cfgFactory = null);

        IJintComponent<TCfg> Create<TCfg>(
            string jsCode,
            string cfgObjRetrieverCode,
            IJintConsole jintConsole,
            Func<IJintComponent<TCfg>, ObjectInstance, TCfg> cfgFactory = null);
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
            string jsCode,
            string cfgObjRetrieverCode,
            bool includeConsoleObj = true) => new JintComponent(
                jsCode,
                cfgObjRetrieverCode,
                includeConsoleObj ? consoleFactory.Create() : null);

        public IJintComponent Create(
            string jsCode,
            string cfgObjRetrieverCode,
            IJintConsole jintConsole) => new JintComponent(
                jsCode,
                cfgObjRetrieverCode,
                jintConsole);

        public IJintComponent<TCfg> Create<TCfg>(
            string jsCode,
            string cfgObjRetrieverCode,
            bool includeConsoleObj = true,
            Func<IJintComponent<TCfg>, ObjectInstance, TCfg> cfgFactory = null) => new JintComponent<TCfg>(
                jsCode,
                cfgObjRetrieverCode,
                includeConsoleObj ? consoleFactory.Create() : null,
                cfgFactory);

        public IJintComponent<TCfg> Create<TCfg>(
            string jsCode,
            string cfgObjRetrieverCode,
            IJintConsole jintConsole,
            Func<IJintComponent<TCfg>, ObjectInstance, TCfg> cfgFactory = null) => new JintComponent<TCfg>(
                jsCode,
                cfgObjRetrieverCode,
                jintConsole,
                cfgFactory);
    }
}

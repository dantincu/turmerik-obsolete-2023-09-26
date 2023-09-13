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
        IJintComponent Create(JintComponentOpts.IClnbl opts);

        IJintComponent<TCfg> Create<TCfg>(
            JintComponentOpts.IClnbl<TCfg> opts);
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
            JintComponentOpts.IClnbl opts) => new JintComponent(
                opts.JsCode,
                opts.CfgObjRetrieverCode,
                opts.GlobalThisObjName,
                opts.JintConsole ?? (opts.IncludeDefaultConsoleObj ? consoleFactory.Create() : null));

        public IJintComponent<TCfg> Create<TCfg>(
            JintComponentOpts.IClnbl<TCfg> opts) => new JintComponent<TCfg>(
                opts.JsCode,
                opts.CfgObjRetrieverCode,
                opts.GlobalThisObjName,
                opts.JintConsole ?? (opts.IncludeDefaultConsoleObj ? consoleFactory.Create() : null),
                opts.CfgFactory);
    }
}

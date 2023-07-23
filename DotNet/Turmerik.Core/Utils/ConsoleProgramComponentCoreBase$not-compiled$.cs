using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Utils
{
    public interface IConsoleProgramComponent<TArgs>
    {
        void Run(string[] args);
    }

    public abstract class ConsoleProgramComponentCoreBase<TArgs> : IConsoleProgramComponent<TArgs>
    {
        public ConsoleProgramComponentCoreBase(IProgramArgsBuilderFactory programArgsBuilderFactory)
        {
            ArgsBuilder = programArgsBuilderFactory.Create<TArgs>();
        }

        protected IProgramArgsBuilder<TArgs> ArgsBuilder { get; }

        public virtual void Run(string[] rawArgs)
        {
            var builderArgs = GetArgsBuilderOpts(rawArgs);
            TArgs args = ArgsBuilder.BuildArgs(builderArgs);

            Run(args);
        }

        protected abstract ProgramArgsBuilder<TArgs>.Opts GetArgsBuilderOpts(string[] rawArgs);

        protected abstract void Run(TArgs args);
    }
}

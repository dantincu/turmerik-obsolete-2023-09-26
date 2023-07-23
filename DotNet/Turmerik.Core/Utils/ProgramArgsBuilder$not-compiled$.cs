using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Turmerik.Collections;
using Turmerik.Reflection.Cache;
using static Turmerik.Utils.ProgramArgs;

namespace Turmerik.Utils
{
    public interface IProgramArgsBuilder
    {
        TArgs BuildArgs<TArgs>(Opts<TArgs> opts);
    }

    public class ProgramArgsBuilder : IProgramArgsBuilder
    {
        public ProgramArgsBuilder(ICachedTypesMap cachedTypesMap)
        {
            CachedTypesMap = cachedTypesMap ?? throw new ArgumentNullException(nameof(cachedTypesMap));
        }

        protected ICachedTypesMap CachedTypesMap { get; }

        public TArgs BuildArgs<TArgs>(Opts<TArgs> opts)
        {
            var args = GetWorkArgs(opts);
            args.RawArgMsg = args.NextRawArgMsgFactory(args);

            while (args.RawArgMsg != null)
            {
                if (args.RawArgIdx < args.InitialRawArgs.Count)
                {
                    args.RawArg = args.InitialRawArgs[args.RawArgIdx];
                }
                else
                {
                    args.RawArg = args.NextRawArgFactory(args);
                }

                args.RawArgErr = args.NextRawArgCallback(args);

                if (args.RawArgErr != null)
                {
                    throw new ArgumentException(args.RawArgErr);
                }
                else
                {
                    args.RawArgIdx++;
                    args.RawArgMsg = args.NextRawArgMsgFactory(args);
                }
            }

            return args.Args;
        }

        protected virtual ProgramArgsBuilderOpts.Immtbl<TArgs> NormalizeOpts<TArgs>(
            ProgramArgsBuilderOpts.Mtbl<TArgs> opts)
        {
            opts.ArgsFactory = opts.ArgsFactory.FirstNotNull(
                o => Activator.CreateInstance<TArgs>());

            opts.NextRawArgFactory = opts.NextRawArgFactory.FirstNotNull(
                (args) =>
                {
                    Console.WriteLine(args.RawArgMsg);
                    Console.WriteLine("> ");

                    string rawArg = Console.ReadLine();
                    return rawArg;
                });
        }

        private WorkArgs<TArgs> GetWorkArgs<TArgs>(ProgramArgsBuilderOpts.Immtbl<TArgs> opts)
        {
            var optsImmtbl = NormalizeOpts(opts.AsMtbl());

            var args = new WorkArgs<TArgs>(
                opts.InitialRawArgs.RdnlC(),
                opts.ArgsFactory(opts),
                opts.NextRawArgMsgFactory,
                opts.NextRawArgFactory,
                opts.NextRawArgCallback);

            return args;
        }
    }

    public static class ProgramArgs
    {
        public struct WorkArgs<TArgs>
        {
            public WorkArgs(
                ProgramArgsBuilderOpts.Immtbl<TArgs> opts,
                TArgs args) : this()
            {
                Opts = opts;
                Args = args;
            }

            public ProgramArgsBuilderOpts.Immtbl<TArgs> Opts { get; }
            public TArgs Args { get; }

            public string RawArgMsg { get; set; }
            public string RawArgErr { get; set; }
            public string RawArg { get; set; }
            public int RawArgIdx { get; set; }
        }
    }
}

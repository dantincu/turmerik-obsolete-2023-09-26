using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Collections;
using static Turmerik.Utils.ProgramArgs;

namespace Turmerik.Utils
{
    public static class ProgramArgsBuilderOpts
    {
        public interface IClnbl<TArgs>
        {
            ReadOnlyCollection<string> InitialRawArgs { get; }
            Func<Opts<TArgs>, TArgs> ArgsFactory { get; }
            Func<WorkArgs<TArgs>, string> NextRawArgMsgFactory { get; }
            Func<WorkArgs<TArgs>, string> NextRawArgFactory { get; }
            Func<WorkArgs<TArgs>, string> NextRawArgCallback { get; }

            IEnumerable<ProgramArgsPropBuilderOpts.IClnbl<TArgs>> GetProps();
        }

        public class Immtbl<TArgs> : IClnbl<TArgs>
        {
            public Immtbl(IClnbl<TArgs> src)
            {
                InitialRawArgs = src.InitialRawArgs;
                ArgsFactory = src.ArgsFactory;
                NextRawArgMsgFactory = src.NextRawArgMsgFactory;
                NextRawArgFactory = src.NextRawArgFactory;
                NextRawArgCallback = src.NextRawArgCallback;

                Props = src.GetProps().AsImmtblCllctn();
            }

            public ReadOnlyCollection<string> InitialRawArgs { get; }
            public Func<Opts<TArgs>, TArgs> ArgsFactory { get; }
            public Func<WorkArgs<TArgs>, string> NextRawArgMsgFactory { get; }
            public Func<WorkArgs<TArgs>, string> NextRawArgFactory { get; }
            public Func<WorkArgs<TArgs>, string> NextRawArgCallback { get; }

            public ReadOnlyCollection<ProgramArgsPropBuilderOpts.Immtbl<TArgs>> Props { get; }

            public IEnumerable<ProgramArgsPropBuilderOpts.IClnbl<TArgs>> GetProps() => Props;
        }

        public class Mtbl<TArgs> : IClnbl<TArgs>
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl<TArgs> src)
            {
                InitialRawArgs = src.InitialRawArgs;
                ArgsFactory = src.ArgsFactory;
                NextRawArgMsgFactory = src.NextRawArgMsgFactory;
                NextRawArgFactory = src.NextRawArgFactory;
                NextRawArgCallback = src.NextRawArgCallback;

                Props = src.GetProps().AsMtblList();
            }

            public ReadOnlyCollection<string> InitialRawArgs { get; set; }
            public Func<Opts<TArgs>, TArgs> ArgsFactory { get; set; }
            public Func<WorkArgs<TArgs>, string> NextRawArgMsgFactory { get; set; }
            public Func<WorkArgs<TArgs>, string> NextRawArgFactory { get; set; }
            public Func<WorkArgs<TArgs>, string> NextRawArgCallback { get; set; }

            public List<ProgramArgsPropBuilderOpts.Mtbl<TArgs>> Props { get; set; }

            public IEnumerable<ProgramArgsPropBuilderOpts.IClnbl<TArgs>> GetProps() => Props;
        }

        public static Immtbl<TArgs> ToImmtbl<TArgs>(
            this IClnbl<TArgs> src) => new Immtbl<TArgs>(src);

        public static Immtbl<TArgs> AsImmtbl<TArgs>(
            this IClnbl<TArgs> src) => (src as Immtbl<TArgs>) ?? src?.ToImmtbl();

        public static Mtbl<TArgs> ToMtbl<TArgs>(
            this IClnbl<TArgs> src) => new Mtbl<TArgs>(src);

        public static Mtbl<TArgs> AsMtbl<TArgs>(
            this IClnbl<TArgs> src) => (src as Mtbl<TArgs>) ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl<TArgs>> ToImmtblCllctn<TArgs>(
            this IEnumerable<IClnbl<TArgs>> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl<TArgs>> AsImmtblCllctn<TArgs>(
            this IEnumerable<IClnbl<TArgs>> src) => (
            src as ReadOnlyCollection<Immtbl<TArgs>>) ?? src?.ToImmtblCllctn();

        public static List<Mtbl<TArgs>> ToMtblList<TArgs>(
            this IEnumerable<IClnbl<TArgs>> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl<TArgs>> AsMtblList<TArgs>(
            this IEnumerable<IClnbl<TArgs>> src) => (src as List<Mtbl<TArgs>>) ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl<TArgs>> AsImmtblDictnr<TKey, TArgs>(
            IDictionaryCore<TKey, IClnbl<TArgs>> src) => (src as ReadOnlyDictionary<TKey, Immtbl<TArgs>>) ?? (src as Dictionary<TKey, Mtbl<TArgs>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl<TArgs>> AsMtblDictnr<TKey, TArgs>(
            IDictionaryCore<TKey, IClnbl<TArgs>> src) => (src as Dictionary<TKey, Mtbl<TArgs>>) ?? (src as ReadOnlyDictionary<TKey, Immtbl<TArgs>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());
    }

    public static class ProgramArgsPropBuilderOpts
    {
        public interface IClnbl<TArgs>
        {
            string PropName { get; }
            string PropDisplayName { get; }
            Func<ProgramArgs.WorkArgs<TArgs>, Immtbl<TArgs>, int, object> PropValFactory { get; }
        }

        public class Immtbl<TArgs> : IClnbl<TArgs>
        {
            public Immtbl(IClnbl<TArgs> src)
            {
                PropName = src.PropName;
                PropDisplayName = src.PropDisplayName;
                PropValFactory = src.PropValFactory;
            }

            public string PropName { get; }
            public string PropDisplayName { get; }
            public Func<ProgramArgs.WorkArgs<TArgs>, Immtbl<TArgs>, int, object> PropValFactory { get; }
        }

        public class Mtbl<TArgs> : IClnbl<TArgs>
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl<TArgs> src)
            {
                PropName = src.PropName;
                PropDisplayName = src.PropDisplayName;
                PropValFactory = src.PropValFactory;
            }

            public string PropName { get; set; }
            public string PropDisplayName { get; set; }
            public Func<ProgramArgs.WorkArgs<TArgs>, Immtbl<TArgs>, int, object> PropValFactory { get; set; }
        }

        public static Immtbl<TArgs> ToImmtbl<TArgs>(
            this IClnbl<TArgs> src) => new Immtbl<TArgs>(src);

        public static Immtbl<TArgs> AsImmtbl<TArgs>(
            this IClnbl<TArgs> src) => (src as Immtbl<TArgs>) ?? src?.ToImmtbl();

        public static Mtbl<TArgs> ToMtbl<TArgs>(
            this IClnbl<TArgs> src) => new Mtbl<TArgs>(src);

        public static Mtbl<TArgs> AsMtbl<TArgs>(
            this IClnbl<TArgs> src) => (src as Mtbl<TArgs>) ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl<TArgs>> ToImmtblCllctn<TArgs>(
            this IEnumerable<IClnbl<TArgs>> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl<TArgs>> AsImmtblCllctn<TArgs>(
            this IEnumerable<IClnbl<TArgs>> src) => (
            src as ReadOnlyCollection<Immtbl<TArgs>>) ?? src?.ToImmtblCllctn();

        public static List<Mtbl<TArgs>> ToMtblList<TArgs>(
            this IEnumerable<IClnbl<TArgs>> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl<TArgs>> AsMtblList<TArgs>(
            this IEnumerable<IClnbl<TArgs>> src) => (src as List<Mtbl<TArgs>>) ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl<TArgs>> AsImmtblDictnr<TKey, TArgs>(
            IDictionaryCore<TKey, IClnbl<TArgs>> src) => (src as ReadOnlyDictionary<TKey, Immtbl<TArgs>>) ?? (src as Dictionary<TKey, Mtbl<TArgs>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl<TArgs>> AsMtblDictnr<TKey, TArgs>(
            IDictionaryCore<TKey, IClnbl<TArgs>> src) => (src as Dictionary<TKey, Mtbl<TArgs>>) ?? (src as ReadOnlyDictionary<TKey, Immtbl<TArgs>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());
    }
}

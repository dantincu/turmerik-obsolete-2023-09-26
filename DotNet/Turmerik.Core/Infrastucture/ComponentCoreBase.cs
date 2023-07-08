using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.Infrastucture
{
    public abstract class ComponentCoreBase
    {
        protected TResult Run<TOpts, TNormOpts, TArgs, TResult>(
            TOpts opts,
            Func<TOpts, TNormOpts> optsNormalizer,
            Func<TNormOpts, TArgs> argsFactory,
            Action<TArgs> action,
            Func<TArgs, TResult> resultFactory)
        {
            var normOpts = optsNormalizer(opts);
            var args = argsFactory(normOpts);

            action(args);
            var result = resultFactory(args);

            return result;
        }
    }
}

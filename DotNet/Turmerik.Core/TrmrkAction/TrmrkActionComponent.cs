using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Logging;
using Turmerik.Utils;

namespace Turmerik.TrmrkAction
{
    public interface ITrmrkActionComponent
    {
        ITrmrkActionResult Execute(
            ITrmrkActionComponentOpts opts);

        ITrmrkActionResult<TData> Execute<TData>(
            ITrmrkActionComponentOpts<TData> opts);

        Task<ITrmrkActionResult> ExecuteAsync(
            ITrmrkAsyncActionComponentOpts opts);

        Task<ITrmrkActionResult<TData>> ExecuteAsync<TData>(
            ITrmrkAsyncActionComponentOpts<TData> opts);
    }

    public class TrmrkActionComponent : TrmrkActionComponentCore<ITrmrkActionComponentsManagerCore>, ITrmrkActionComponent
    {
        public TrmrkActionComponent(
            ITrmrkActionComponentsManagerCore manager,
            IAppLogger logger) : base(
                manager,
                logger)
        {
        }

        public ITrmrkActionResult Execute(
            ITrmrkActionComponentOpts opts) => ExecuteCore(opts);

        public ITrmrkActionResult<TData> Execute<TData>(
            ITrmrkActionComponentOpts<TData> opts) => ExecuteCore(opts);

        public Task<ITrmrkActionResult> ExecuteAsync(
            ITrmrkAsyncActionComponentOpts opts) => ExecuteCoreAsync(opts);

        public Task<ITrmrkActionResult<TData>> ExecuteAsync<TData>(
            ITrmrkAsyncActionComponentOpts<TData> opts) => ExecuteCoreAsync(opts);
    }
}

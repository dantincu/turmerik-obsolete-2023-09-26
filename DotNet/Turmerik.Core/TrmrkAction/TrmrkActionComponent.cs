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

    public class TrmrkActionComponent<TManager> : TrmrkActionComponentCore<TManager>, ITrmrkActionComponent
        where TManager : class, ITrmrkActionComponentsManager
    {
        public TrmrkActionComponent(
            TManager manager,
            IAppLogger logger) : base(
                manager,
                logger)
        {
        }

        public ITrmrkActionResult Execute(
            ITrmrkActionComponentOpts opts) => ExecuteCore(
                opts, new TrmrkActionResult(),
                new TrmrkActionResult
                {
                    HasError = true
                });

        public ITrmrkActionResult<TData> Execute<TData>(
            ITrmrkActionComponentOpts<TData> opts) => ExecuteCore(
                opts, new TrmrkActionResult<TData>(),
                new TrmrkActionResult<TData>
                {
                    HasError = true
                });

        public Task<ITrmrkActionResult> ExecuteAsync(
            ITrmrkAsyncActionComponentOpts opts) => ExecuteCoreAsync(
                opts, new TrmrkActionResult(),
                new TrmrkActionResult
                {
                    HasError = true
                });

        public Task<ITrmrkActionResult<TData>> ExecuteAsync<TData>(
            ITrmrkAsyncActionComponentOpts<TData> opts) => ExecuteCoreAsync(
                opts, new TrmrkActionResult<TData>(),
                new TrmrkActionResult<TData>
                {
                    HasError = true
                });
    }
}
